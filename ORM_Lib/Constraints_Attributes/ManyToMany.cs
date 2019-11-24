using System;
using ORM_Lib.DbSchema;

namespace ORM_Lib.Constraints_Attributes
{
    
    [AttributeUsage(AttributeTargets.Property)]
    public class ManyToMany : Attribute, IConstraint
    {
        // TODO: maybe we do not need FromPocoType and FromEntity 
        public string TableName { get; set; }
        

        public Type ToPocoType { get; set; }
        
     
        
        internal Entity ToEntity { get; set; }
        public string ForeignKeyNear { get; set; }
        public string ForeignKeyFar { get; set; }
    }
}