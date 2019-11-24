using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ORM_Lib
{
    internal class DbSetBuilder
    {
        public static object BuildDbSet(Type t, DbContext ctx)
        {
            return typeof(DbSetBuilder)
                .GetTypeInfo()
                .GetDeclaredMethod(nameof(CreateDbSet))
                .MakeGenericMethod(t)
                .Invoke(null, new object[] {ctx});
        }

        private static DbSet<T> CreateDbSet<T>(DbContext ctx) where T : class => new InternalDbSet<T>(ctx);

       
    }
}