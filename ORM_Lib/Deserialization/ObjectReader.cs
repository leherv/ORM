using ORM_Lib.Cache;
using ORM_Lib.DbSchema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace ORM_Lib.Deserialization
{
    class ObjectReader<T>
    {

        private IDataReader DataReader { get; }
        private Entity Entity { get; }

        private List<Column> QueriedColumns { get; }

        private DbContext _ctx;

        public ObjectReader(DbContext ctx, IDataReader dataReader, Entity entity, List<Column> queriedColumns)
        {
            DataReader = dataReader;
            Entity = entity;
            QueriedColumns = queriedColumns;
            _ctx = ctx;
        }


        public IEnumerable<T> Serialize()
        {
            var result = new List<T>();
            while (DataReader.Read())
            {
                result.Add(CreateObject(DataReader));
            }
            return result;
        }

        // we always refill the object with the current values from the DB even if we find the object (via PK in our cache)
        // as the expensive operation (fetching from DB is already done) through the cache we just ensure object identity
        private T CreateObject(IDataReader reader)
        {
            // create a new one of T
            var poco = ObjectFactory.Create(Entity.PocoType);
            var cache = _ctx.Cache;
            var pkValue = reader[Entity.PkColumn.Name];
            // we now get a new empty cacheEntry or it already was present
            var cacheEntry = cache.GetOrInsert(Entity, (long)pkValue, poco);

            foreach (var col in QueriedColumns)
            {
                if (col.IsDbColumn)
                {
                    if (col.IsShadowAttribute)
                    {
                        // foreign key / ManyToOne (exists in db and in object, but is a key - can not be set directly and needs to be lazily loaded)
                        cacheEntry.ShadowAttributes.Add(col.Name, reader[col.Name]);
                    }
                    else
                    {
                        var value = reader[col.Name];
                        // specially handle enums
                        if (col.PropInfo.PropertyType.IsEnum)
                        {
                            value = Enum.Parse(col.PropInfo.PropertyType, value as string);
                        }
                        // fill the originalEntries with values for changeTracking later
                        if (!cacheEntry.OriginalPoco.ContainsKey(col.Name)) cacheEntry.OriginalPoco.Add(col.Name, value);
                        // fill the object from the cache with values
                        col.PropInfo.GetSetMethod(true).Invoke(cacheEntry.Poco, new[] { value });
                    }
                }
            }
            // instantiate the inner lazy loader in the object
            LazyLoadInjector.InjectLazyLoader<T>(poco, _ctx, Entity);
            return (T)cacheEntry.Poco;
        }



     

        
    }
}
