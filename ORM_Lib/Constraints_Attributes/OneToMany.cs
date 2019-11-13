using System;

namespace ORM_Lib.Constraints_Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OneToMany : Attribute, IConstraint
    {
        public Entity From { get; set; }

        public Entity To { get; set; }
    }
}