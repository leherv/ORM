using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Lib.Cache
{
    class CacheEntry
    {

        public object Poco { get; }

        public CacheEntry(object poco)
        {
            Poco = poco;
        }



    }
}
