using System;
using System.Reflection;
using ORM_Lib.DbSchema;

namespace ORM_Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OneToMany : Relation
    {
        // TODO: not sure if we will need all this
        public PropertyInfo MappedByProperty { get; set; }
        internal Entity MappedByEntity { get; set; }
        public Type MappedByPocoType { get; set; }
    }
}