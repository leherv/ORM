using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ORM_Lib
{
    class ListBuilder
    {

        public static object BuildList(Type t, object item)
        {
            return typeof(ListBuilder)
                .GetTypeInfo()
                .GetDeclaredMethod(nameof(CreateList))
                .MakeGenericMethod(t)
                .Invoke(null, new object[] { item });
        }

        private static List<T> CreateList<T>(T item) where T : class => new List<T>() { item };
    }
}
