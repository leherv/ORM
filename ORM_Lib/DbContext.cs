using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using ORM_Lib.DbSchema;
using ORM_Lib.TypeMapper;
using ORM_Lib.Cache;
using ORM_Lib.Query.Update;
using ORM_Lib.Saving;
using ORM_Lib.Query.Insert;
using ORM_Lib.Query.Create;
using ORM_Lib.Ddl;

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
            Cache = new PocoCache(this);

            if (Configuration.CreateDB)
                Database.ExecuteDDL(new DdlStatement(SchemaSqlBuilder.BuildDdl(Schema, Configuration.TypeMapper)));
        }

        public void SaveChanges()
        {
            var updateStatements = new List<UpdateStatement>();
            (List<PocoUpdateChange> updates, List<PocoInsertChange> inserts) = Cache.GetChanges();

            if(inserts.Count > 0)
            {
                // first we want to insert into the intermediate tables per table (tablename)
                var manyToManyInsertStmts = new List<ManyToManyInsertStatement>();
                var insertGroups = inserts.GroupBy(i => i.TableName);
                foreach (var group in insertGroups)
                {
                    manyToManyInsertStmts.Add(new ManyToManyInsertStatementBuilder(group.Key, group.ToList()).Build());
                }
                Database.SaveInsertChanges(new ManyToManyInsertBatch(manyToManyInsertStmts));
            }
            if(updates.Count > 0)
            {
                // now we update all the values
                foreach (var update in updates)
                {
                    updateStatements.AddRange(new UpdateStatementBuilder(this, update).Build());
                }
                Database.SaveUpdateChanges(new UpdateBatch(updateStatements));
            }
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