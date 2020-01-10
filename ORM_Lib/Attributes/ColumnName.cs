using System;

namespace ORM_Lib.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnName : Constraint
    {
        public string CName { get; set; }
        public ColumnName(string name)
        {
            CName = name;
        }
    }
}