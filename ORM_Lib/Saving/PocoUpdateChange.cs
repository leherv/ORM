using ORM_Lib.DbSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Lib.Saving
{
    class PocoUpdateChange
    {
        public Dictionary<string, object> NewValues { get; set; }
        public Entity EntityChanged { get; set; }

        public object Pk { get; set; }

        public PocoUpdateChange(Dictionary<string, object> newValues, Entity entity, object pk)
        {
            NewValues = newValues;
            EntityChanged = entity;
            Pk = pk;
        }
    }
}
