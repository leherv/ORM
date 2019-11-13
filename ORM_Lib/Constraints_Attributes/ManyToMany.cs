using System;

namespace ORM_Lib.Constraints_Attributes
{
    
    [AttributeUsage(AttributeTargets.Property)]
    public class ManyToMany : Attribute, IConstraint
    {
        public string TableName { get; set; }
        public string ForeignKeyNear { get; set; }
        public string ForeignKeyFar { get; set; }
    }
}