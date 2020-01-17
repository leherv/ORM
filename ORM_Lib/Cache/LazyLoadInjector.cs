using ORM_Lib.DbSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORM_Lib.Cache
{
    internal static class LazyLoadInjector
    {


        public static void InjectLazyLoader<T>(object poco, DbContext ctx, Entity entity)
        {
            var type = typeof(T);
            var lazyLoaderProperty = type
                .GetProperties(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .FirstOrDefault(p => p.PropertyType.IsAssignableFrom(typeof(ILazyLoader)));
            // if it is null we dont have a ladyloader in this object and dont need to do more
            if (lazyLoaderProperty != null)
            {
                var lazyLoader = lazyLoaderProperty.GetGetMethod(true).Invoke(poco, new object[0]);
                // cool way to check and set at the same time!
                if(lazyLoader is LazyLoader lazy)
                {
                    lazy.InternalLazyLoader = new InternalLazyLoader(ctx, entity);
                }
            }
        }
    }
}
