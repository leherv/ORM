using System;
using ORM_Lib.DbSchema;

namespace ORM_Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ManyToOne : Relation
    {
        internal Column To { get; set; }

        internal Entity ToEntity { get; set; }
        internal Type ToPocoType { get; set; }

    }
}

//TODO: nearly the same as Foreignkey - think about needing it still