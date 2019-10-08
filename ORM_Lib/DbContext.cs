using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ORM_Lib
{
    public class DbContext
    {
        public Schema Schema { get; set; }

        public DbContext()
        {
            var propertyInfos = PropertyInfos();
            var types = Types(propertyInfos);
            BuildDbSets(propertyInfos, types);
            Schema = BuildSchema(types);
        }

        private void BuildDbSets(IEnumerable<PropertyInfo> propertyInfos, IEnumerable<Type> types)
        {
            var dbSets = types.Select(DbSetBuilder.BuildDbSet);
            var tup = dbSets.Zip(propertyInfos, Tuple.Create);
            foreach (var (dbSet, method) in tup)
            {
                method.GetSetMethod().Invoke(this, new[] {dbSet});
            }
        }

        private static Schema BuildSchema(IEnumerable<Type> types)
        {
            return SchemaBuilder.BuildSchema(types);
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