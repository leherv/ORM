using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ORM_Lib
{
    public class DbSetBuilder
    {
        public static object BuildDbSet(Type t)
        {
            return typeof(DbSetBuilder)
                .GetTypeInfo()
                .GetDeclaredMethod(nameof(CreateDbSet))
                .MakeGenericMethod(t)
                .Invoke(null, new object[] { });
        }

        private static DbSet<T> CreateDbSet<T>() where T : class => new InternalDbSet<T>();

       
    }
}