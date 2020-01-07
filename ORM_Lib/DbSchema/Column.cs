using System.Collections.Generic;
using System.Reflection;
using ORM_Lib.Constraints_Attributes;

namespace ORM_Lib.DbSchema
{
    internal class Column
    {
        public bool IsDbColumn => DbType != null;
        public bool IsShadowAttribute { get; set; }
        // saved here when building the schema to later fill objects with information from database
        public PropertyInfo PropInfo { get; set; }
        public Entity Entity { get; set; }
        public string DbType { get; set; }
        public string Name { get; set; }
        public HashSet<IConstraint> Constraints { get; set; }


        public Column(string cName, HashSet<IConstraint> constraints, PropertyInfo propInfo, string dbType, bool isShadowAttribute)
        {
            Name = cName;
            Constraints = constraints;
            PropInfo = propInfo;
            DbType = dbType;
            IsShadowAttribute = isShadowAttribute;
        }


    }
}