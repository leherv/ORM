using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using ORM_Lib.DbSchema;
using ORM_Lib.TypeMapper;

namespace ORM_Lib
{
    public abstract class DbContext
    {
        // abstract IDbConnection (ConnectionPooling macht selten Sinn) normalerweise reicht eigentlich immer eine DBConnection und man überlässt dem Hersteller die Arbeit
        // das genauer zu handeln - ConnectionPooling macht fast nie Sinn wenn man es nicht explizit braucht!
        public abstract IDbConnection DbConnection { get; }
        
        // abstract TypeMapper vom User verlangen (ITypeMapper to be exact) und wir haben halt schon eine konkrete implementation bereitliegen für postgres
        protected abstract ITypeMapper TypeMapper { get; }
        internal Schema Schema { get; set; }

        protected DbContext()
        {
            var propertyInfos = PropertyInfos().ToList();
            var types = Types(propertyInfos).ToList();
            BuildDbSets(propertyInfos, types);
            Schema = BuildSchema(types, TypeMapper);
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