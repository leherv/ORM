using System;
using System.Collections;
using System.Reflection;

namespace ORM_Lib.Extensions
{
    internal static class NonStringEnumerableHelper
    {
        public static bool IsNonStringEnumerable(this PropertyInfo pi) {
            return pi != null && pi.PropertyType.IsNonStringEnumerable();
        }

        public static bool IsNonStringEnumerable(this object instance) {
            return instance != null && instance.GetType().IsNonStringEnumerable();
        }

        public static bool IsNonStringEnumerable(this Type type) {
            if (type == null || type == typeof(string))
                return false;
            return typeof(IEnumerable).IsAssignableFrom((Type) type);
        }
    }
}