using System;

namespace ORM_Lib.Constraints_Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableName: Attribute
    {
        public string TName { get; set; }
        public TableName(string name)
        {
            TName = name;
        }
    }
}