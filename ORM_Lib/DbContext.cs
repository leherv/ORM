using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using ORM_Lib.DbSchema;
using ORM_Lib.TypeMapper;
using ORM_Lib.Cache;
using ORM_Lib.Query.Update;

namespace ORM_Lib
{
    public abstract class DbContext
    {
        protected abstract ORMConfiguration Configuration { get; }
        
        // not visible for user
        internal Schema Schema { get; }
        internal Database Database { get; }

        internal PocoCache Cache { get; }

        protected DbContext()
        {
            var propertyInfos = PropertyInfos().ToList();
            var types = Types(propertyInfos).ToList();
            BuildDbSets(propertyInfos, types);
            Schema = BuildSchema(types, Configuration.TypeMapper);
            Database = new Database(Configuration.Connection, this);

            if(Configuration.CreateDB)
            {
                var rows = Database.ExecuteDDL(Ddl.SchemaSqlBuilder.BuildDdl(Schema, Configuration.TypeMapper));
            }
            Cache = new PocoCache(this);
        }

        public void SaveChanges()
        {
            var updateStatements = new List<UpdateStatement>();
            var changes = Cache.GetChanges();
            foreach(var change in changes)
            {
                updateStatements.AddRange(new UpdateStatementBuilder(this, change).Build());
            }
            Database.SaveChanges(new UpdateBatch(updateStatements));
        }
        
        private void BuildDbSets(IEnumerable<PropertyInfo> propertyInfos, IEnumerable<Type> types)
        {
            var dbSets = types.Select(t => DbSetBuilder.BuildDbSet(t,this));
            var tup = dbSets.Zip(propertyInfos, Tuple.Create);
            foreach (var (dbSet, method) in tup)
            {
                method.GetSetMethod().Invoke(this, new[] {dbSet});
            }
        }

        private static Schema BuildSchema(IEnumerable<Type> types, ITypeMapper typeMapper)
        {
            return SchemaBuilder.BuildSchema(types, typeMapper);
        }

        private IEnumerable<PropertyInfo> PropertyInfos()
        {
            return GetType()
                .GetProperties()
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));
        }

        private static IEnumerable<Type> Types(IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.Select(p => p
                .PropertyType
                .GetGenericArguments()[0]);
        }
    }
}