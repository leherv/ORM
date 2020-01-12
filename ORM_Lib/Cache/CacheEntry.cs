using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Lib.Cache
{
    class CacheEntry
    {

        public object Poco { get; }
        public Dictionary<string, object> OriginalPoco = new Dictionary<string, object>();
        public Dictionary<string, object> ShadowAttributes = new Dictionary<string, object>();

        public CacheEntry(object poco)
        {
            Poco = poco;
        }


        



    }
}
