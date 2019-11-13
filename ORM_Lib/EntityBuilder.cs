using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ORM_Lib.Constraints_Attributes;
using ORM_Lib.Extensions;

namespace ORM_Lib
{
    public class EntityBuilder
    {
        public static Entity BuildEntity(Type tableType, IEnumerable<PropertyInfo> propertyInfos)
        {
            var propInfos = propertyInfos.ToList();
            if (!ContainsPk(propInfos)) throw new InvalidOperationException("Entity has no primary key");
            var columns = propInfos.Select(ColumnBuilder.BuildColumn);
            
            return new Entity(
                BuildEntityName(tableType),
                columns.ToList()
            );
        }

        private static string BuildEntityName(MemberInfo t)
        {
            var customName = t.GetCustomAttributes()
                .Where(cA => cA.GetType() == typeof(TableName))
                .Select(cA => cA as TableName)
                .Select(cA => cA?.TName)
                .SingleOrDefault();

            return string.IsNullOrEmpty(customName) ? t.Name : customName;
        }
        
        private static bool ContainsPk(IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos
                .Any(propInfo => 
                    "id".Equals(propInfo.Name.ToLower()) || 
                    propInfo.GetCustomAttributes().Any(ca => ca.GetType() == typeof(Pk)));
        }
    }
}