using System;
using System.Reflection;
using ORM_Lib.DbSchema;

namespace ORM_Lib.Constraints_Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OneToMany : Attribute, IConstraint
    {
        // TODO: not sure if we will need all this
        public PropertyInfo MappedByProperty { get; set; }
        public Entity MappedByEntity { get; set; }
        public Type MappedByPocoType { get; set; }
    }
}