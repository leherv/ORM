using ORM_Lib.DbSchema;
using System;
using System.Collections.Generic;
using System.Text;
using ORM_Lib.Extensions;
using ORM_Lib.Saving;

namespace ORM_Lib.Cache
{
    class PocoCache
    {
        private DbContext _ctx;
        private Dictionary<Entity, Dictionary<long, CacheEntry>> _entityPocoCache = new Dictionary<Entity, Dictionary<long, CacheEntry>>();
        public bool SavingChanges { get; private set; } = false;

        public PocoCache(DbContext ctx)
        {
            _ctx = ctx;
        }

        public CacheEntry GetOrInsert(Entity entity, long primaryKey, object poco)
        {
            var pocoCache = _entityPocoCache.GetOrInsert(entity, new Dictionary<long, CacheEntry>());
            var cacheEntry = pocoCache.GetOrInsert(primaryKey, new CacheEntry(poco, entity, _ctx));
            return cacheEntry;
        }


        public void PrepareCollectChanges()
        {
            foreach (var (_, dict) in _entityPocoCache)
            {
                foreach (var (_, cacheEntry) in dict)
                {
                    cacheEntry.PrepareForCollectChanges();
                }
            }
        }

        public List<PocoChange> GetChanges()
        {
            SavingChanges = true;
            PrepareCollectChanges();
            var changes = new List<PocoChange>();
            // iterate all cacheEntries
            foreach(var (entity, dict) in _entityPocoCache)
            {
                foreach(var (key, cacheEntry) in dict)
                {
                    // if there are no changes to the cachedobject we dont add it to the list of pocoChanges
                    var newValues = cacheEntry.CalculateChange();
                    if (newValues.Count > 0)
                        changes.Add(new PocoChange(cacheEntry.Poco, entity, newValues, key));
                }
            }
            SavingChanges = false;
            return changes;
        }

    }
}
