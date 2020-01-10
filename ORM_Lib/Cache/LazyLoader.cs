﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ORM_Lib.Cache
{
    public class LazyLoader : ILazyLoader
    {
        internal ILazyLoader InternalLazyLoader { get; set; }
        public ICollection<T> Load<T>(object poco, ref ICollection<T> loadTo)
        {
            // user uses his own collection without database or it was already loaded
            if (loadTo != null) return loadTo;
            return InternalLazyLoader?.Load(poco, ref loadTo) ?? loadTo;

        }

        public T Load<T>(object poco, ref T loadTo)
        {
            throw new NotImplementedException();
        }
    }
}