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

        public PocoChange(Dictionary<string, object> newValues)
        {
            NewValues = newValues;
        }


    }
}
