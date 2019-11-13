using System.Collections.Generic;
using System.Reflection;
using ORM_Lib.Constraints_Attributes;

namespace ORM_Lib
{
    public class Column
    {
        
        // saved here when building the schema to later fill objects with information from database
        public PropertyInfo PropInfo { get; set; }
        public string Name { get; set; }
        public List<IConstraint> Constraints { get; set; }


        public Column(string cName, List<IConstraint> constraints, PropertyInfo propInfo)
        {
            Name = cName;
            Constraints = constraints;
            PropInfo = propInfo;
        }
        
        
    }
}