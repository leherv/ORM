using ORM_Lib.DbSchema;
using System.Collections.Generic;
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

        public (List<PocoUpdateChange>, List<PocoInsertChange>) GetChanges()
        {
            SavingChanges = true;
            PrepareCollectChanges();
            var (updateChanges, insertChanges) = (new List<PocoUpdateChange>(), new List<PocoInsertChange>());
            // iterate all cacheEntries
            foreach (var (entity, dict) in _entityPocoCache)
            {
                foreach(var (key, cacheEntry) in dict)
                {
                    // if there are no changes to the cachedobject we dont add it to the list of pocoChanges
                    (PocoUpdateChange updateChange, List<PocoInsertChange> pocoInsertChanges) = cacheEntry.CalculateChange();
                    if (updateChange.NewValues.Count > 0)
                        updateChanges.Add(updateChange);
                    insertChanges.AddRange(pocoInsertChanges);
                }
            }
            SavingChanges = false;
            return (updateChanges, insertChanges);
        }

    }
}
