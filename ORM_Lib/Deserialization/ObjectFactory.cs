using System;

namespace ORM_Lib.Deserialization
{
    class ObjectFactory
    {
        public static object Create(Type t)
        {
            return t.GetConstructor(new Type[] { })?
                .Invoke(new object[0]);
        }
    }
}
