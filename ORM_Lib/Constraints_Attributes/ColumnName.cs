using System;

namespace ORM_Lib.Constraints_Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnName : Attribute
    {
        public string CName { get; set; }
        public ColumnName(string name)
        {
            CName = name;
        }
    }
}