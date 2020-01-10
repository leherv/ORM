using System;
using ORM_Lib.DbSchema;

namespace ORM_Lib.Attributes
{

    [AttributeUsage(AttributeTargets.Property)]
    public class ManyToMany : Relation
    {
        // TODO: maybe we do not need FromPocoType and FromEntity 
        public string TableName { get; set; }


        public Type ToPocoType { get; set; }



        internal Entity ToEntity { get; set; }
        public string ForeignKeyNear { get; set; }
        public string ForeignKeyFar { get; set; }
    }
}