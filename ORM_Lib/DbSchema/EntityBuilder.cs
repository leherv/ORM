using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ORM_Lib.Constraints_Attributes;

namespace ORM_Lib.DbSchema
{
    public static class EntityBuilder
    {
        public static Entity BuildEntity(Type tableType, IEnumerable<PropertyInfo> propertyInfos, List<Type> tableTypes)
        {
            var propInfos = propertyInfos.ToList();
            if (!ContainsPk(propInfos)) throw new InvalidOperationException("Entity has no primary key");
            var columns = propInfos.Select(propInfo => ColumnBuilder.BuildColumn(propInfo, tableType, tableTypes)).ToList();
            // TODO: maybe return tuple of (PkColumn, Columns) in columnBuilder instead
            var pkColumn = columns.First(c => c.Constraints.Any(cons => typeof(Pk) == cons.GetType()));
            return new Entity(
                BuildEntityName(tableType),
                columns,
                tableType,
                pkColumn
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