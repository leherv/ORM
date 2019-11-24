using System;
using ORM_Lib.DbSchema;

namespace ORM_Lib.Constraints_Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ManyToOne : Attribute, IConstraint
    {
        internal Column To { get; set; }
        
        internal Entity ToEntity { get; set; }
        public Type ToPocoType { get; set; }
        
    }
}

//TODO: nearly the same as Foreignkey - think about needing it still