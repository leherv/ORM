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
        
        public Entity ToEntity { get; set; }
        public Column To { get; set; }
        public Type ToPocoType { get; set; }
    }
}