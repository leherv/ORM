using ORM_Lib.DbSchema;
using ORM_Lib.Saving;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Lib.Cache
{
    class CacheEntry
    {

        private Entity _entity ;
        public object Poco { get; }
        public Dictionary<string, object> OriginalPoco = new Dictionary<string, object>();
        public Dictionary<string, object> ShadowAttributes = new Dictionary<string, object>();

        public CacheEntry(object poco, Entity entity)
        {
            Poco = poco;
            _entity = entity;
        }

        public Dictionary<string, object> CalculateChange()
        {
            Dictionary<string, object> newValues = new Dictionary<string, object>();
            foreach(var col in _entity.Columns)
            {
                if(col.IsDbColumn)
                {
                    if(!col.IsShadowAttribute)
                    {
                        var value = col.PropInfo.GetMethod.Invoke(Poco, new object[0]);
                        if(OriginalPoco[col.Name] != value)
                        {
                            newValues[col.Name] = value;
                        }
                    }
                }
            }
            return newValues;
        }
    }
}
