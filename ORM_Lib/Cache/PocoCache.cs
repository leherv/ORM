using ORM_Lib.DbSchema;
using System;
using System.Collections.Generic;
using System.Text;
using ORM_Lib.Extensions;

namespace ORM_Lib.Cache
{
    class PocoCache
    {
        private Dictionary<Entity, Dictionary<long, CacheEntry>> _entityPocoCache = new Dictionary<Entity, Dictionary<long, CacheEntry>>();


        public CacheEntry GetOrInsert(Entity entity, long primaryKey, object poco)
        {
            var pocoCache = _entityPocoCache.GetOrInsert(entity, new Dictionary<long, CacheEntry>());
            var cacheEntry = pocoCache.GetOrInsert(primaryKey, new CacheEntry(poco));
            return cacheEntry;
        }

        
    

    }
}
