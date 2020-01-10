using System;

namespace ORM_Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableName : Constraint
    {
        public string TName { get; set; }
        public TableName(string name)
        {
            TName = name;
        }
    }
}