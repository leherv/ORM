using ORM_Lib.DbSchema;
using System;
using System.Collections.Generic;

namespace ORM_Lib.Cache
{
    class CacheEntry
    {
        private DbContext _ctx;
        private Entity _entity;
        public object Poco { get; }
        public Dictionary<string, object> OriginalPoco = new Dictionary<string, object>();
        public Dictionary<string, object> ShadowAttributes = new Dictionary<string, object>();

        public CacheEntry(object poco, Entity entity, DbContext ctx)
        {
            Poco = poco;
            _entity = entity;
            _ctx = ctx;
        }

        public Dictionary<string, object> CalculateChange()
        {
            Dictionary<string, object> newValues = new Dictionary<string, object>();
            foreach (var col in _entity.CombinedColumns())
            {
                if (col.IsDbColumn)
                {
                    if (col.IsShadowAttribute)
                    {
                        var currentObj = col.PropInfo.GetMethod.Invoke(Poco, new object[0]);
                        if(currentObj != null)
                        {
                            var cEntity = _ctx.Schema.GetByType(currentObj.GetType());
                            var fk = cEntity.PkColumn.PropInfo.GetMethod.Invoke(currentObj, new object[0]);
                            if(!ValuesEqual(ShadowAttributes[col.Name], fk))
                            {
                                newValues[col.Name] = fk;
                            }
                        }
                        
                    }
                    else
                    {
                        var value = col.PropInfo.GetMethod.Invoke(Poco, new object[0]);
                        // handle null in dictionary here
                        if (!ValuesEqual(OriginalPoco[col.Name], value))
                        {
                            newValues[col.Name] = value;
                        }
                    }
                }
            }
            return newValues;
        }


        private static bool ValuesEqual(object value1, object value2)
        {
            bool areValuesEqual = true;
            IComparable selfValueComparer = value1 as IComparable;

            // one of the values is null            
            if (value1 == null && value2 != null || value1 != null && value2 == null)
                areValuesEqual = false;
            else if (selfValueComparer != null && selfValueComparer.CompareTo(value2) != 0)
                areValuesEqual = false;
            else if (!object.Equals(value1, value2))
                areValuesEqual = false;

            return areValuesEqual;
        }
    }

}
