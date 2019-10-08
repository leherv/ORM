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
            // gets public private fields and also backing fields in case of attributes
            HandleFields(t.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(fieldI => !IncludesIgnore(fieldI.CustomAttributes)));

            return typeof(DbSetBuilder)
                .GetTypeInfo()
                .GetDeclaredMethod(nameof(CreateDbSet))
                .MakeGenericMethod(t)
                .Invoke(null, new object[] { });
        }

        private static DbSet<T> CreateDbSet<T>() where T : class => new InternalDbSet<T>();

        private static void HandleFields(IEnumerable<FieldInfo> fieldInfos)
        {
        }

        private static bool IncludesIgnore(IEnumerable<object> customAttributes)
        {
            return customAttributes.Any(cA => cA.GetType() == typeof(Ignore));
        }
    }
}