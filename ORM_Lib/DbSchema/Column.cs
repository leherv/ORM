using System.Collections.Generic;
using System.Reflection;
using ORM_Lib.Constraints_Attributes;
using ORM_Lib.TypeMapper;

namespace ORM_Lib.DbSchema
{
    internal class Column
    {
        public bool IsDbColumn => DbType.DdlDbType != null;
        public bool IsShadowAttribute { get; set; }
        // saved here when building the schema to later fill objects with information from database
        public PropertyInfo PropInfo { get; set; }
        public Entity Entity { get; set; }
        public OrmDbType DbType { get; set; }
        public string Name { get; set; }
        public HashSet<IConstraint> Constraints { get; set; }


        public Column(string cName, HashSet<IConstraint> constraints, PropertyInfo propInfo, OrmDbType dbType, bool isShadowAttribute)
        {
            Name = cName;
            Constraints = constraints;
            PropInfo = propInfo;
            DbType = dbType;
            IsShadowAttribute = isShadowAttribute;
        }


    }
}