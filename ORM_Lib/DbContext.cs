using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ORM_Lib
{
    public class DbContext
    {
        public DbContext()
        {
            BuildDbSets()
                .Zip(GetDbSetProperties(),
                    (dbSet, setP) => setP.SetMethod.Invoke(this, new[] {dbSet}));
        }

        private IEnumerable<DbSet<object>> BuildDbSets()
        {
            return GetDbSetProperties()
                .Select(p => p
                    .GetMethod
                    .ReturnType
                    .GenericTypeArguments[0])
                .Select(DbSetBuilder.BuildDbSet) as IEnumerable<DbSet<object>>;
        }

        private IEnumerable<PropertyInfo> GetDbSetProperties()
        {
            return GetType()
                .GetProperties()
                .Where(p => p.PropertyType == typeof(DbSet<>));
        }
    }
}