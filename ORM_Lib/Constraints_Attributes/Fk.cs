using System;
using ORM_Lib.DbSchema;

namespace ORM_Lib.Constraints_Attributes
{
    public class Fk : Attribute, IConstraint
    {

        public Fk(Type toPocoType)
        {
            ToPocoType = toPocoType;
        }
        
        internal Entity ToEntity { get; set; }
        internal Column To { get; set; }
        internal Type ToPocoType { get; set; }
    }
}