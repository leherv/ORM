using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ORM_Lib.Cache
{
    public class LazyLoader : ILazyLoader
    {
        internal ILazyLoader InternalLazyLoader { get; set; }
        public ICollection<T> Load<T>(object poco, ref ICollection<T> loadTo, [CallerMemberName] string name = "")
        {
            if (!ShouldLoad(loadTo)) return loadTo;
            return InternalLazyLoader?.Load(poco, ref loadTo, name) ?? loadTo;

        }

        public T Load<T>(object poco, ref T loadTo, [CallerMemberName] string name = "")
        {
            if (!ShouldLoad(loadTo)) return loadTo;
            return InternalLazyLoader.Load(poco, ref loadTo, name) ?? loadTo;
        }



        // we only want to lazily load if 
        // 1. the InternalLazyLoader was already set because the object was read from the database or inserted into the database
        // 2. the collection was not already instantiated (either by a user) or by a first lazyload
        // 3. we are not collecting changes at the moment - to avoid lazily fetching everything when updating relations!
        public bool ShouldLoad<T>(T loadTo)
        {
            return (loadTo == null && InternalLazyLoader != null);
            
        }
    }
}
