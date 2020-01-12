using System;
using System.Reflection;
using ORM_Lib.DbSchema;

namespace ORM_Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OneToMany : Relation
    {
        internal PropertyInfo MappedByProperty { get; set; }
        internal Entity MappedByEntity { get; set; }
        internal Type MappedByPocoType { get; set; }
        public string mappedByColumnName { get; set; }

    }
}