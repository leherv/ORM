using ORM_Lib.DbSchema;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Lib.Saving
{
    class PocoChange
    {
        public Entity EntityChanged { get; }
        public Dictionary<string, object> NewValues { get; }
        public object Poco { get; }

        public long Pk { get; }

        public PocoChange(object poco, Entity entityChanged, Dictionary<string, object> newValues, long pk)
        {
            EntityChanged = entityChanged;
            NewValues = newValues;
            Poco = poco;
            Pk = pk;
        }


    }
}
