using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ORM_Lib.Constraints_Attributes;
using ORM_Lib.TypeMapper;

namespace ORM_Lib.DbSchema
{
    internal static class EntityBuilder
    {
        public static Entity BuildEntity(Type tableType, IEnumerable<PropertyInfo> propertyInfos, List<Type> tableTypes, ITypeMapper typeMapper)
        {
            // to handle inheritance
            var superClasses = tableTypes.Where(t => t.IsAssignableFrom(tableType) && t != tableType).ToList();
            var propInfos = HandleInheritance(propertyInfos, tableType);


            if (!ContainsPk(propInfos)) throw new InvalidOperationException("Entity has no primary key");
            var columns = propInfos.Select(propInfo => ColumnBuilder.BuildColumn(propInfo, tableType, tableTypes, typeMapper)).ToList();
            // TODO: maybe return tuple of (PkColumn, Columns) in columnBuilder instead
            var pkColumn = columns.First(c => c.Constraints.Any(cons => typeof(Pk) == cons.GetType()));
            return new Entity(
                BuildEntityName(tableType),
                columns,
                tableType,
                pkColumn,
                superClasses
            );
        }

        private static string BuildEntityName(MemberInfo t)
        {
            var customName = t.GetCustomAttributes()
                .Where(cA => cA.GetType() == typeof(TableName))
                .Select(cA => cA as TableName)
                .Select(cA => cA?.TName)
                .SingleOrDefault();

            return string.IsNullOrEmpty(customName) ? t.Name.ToLower() : customName.ToLower();
        }

        private static bool ContainsPk(IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos
                .Any(IsPk);
        }

        private static bool IsPk(PropertyInfo propInfo)
        {
            return "id".Equals(propInfo.Name.ToLower()) ||
                    propInfo.GetCustomAttributes().Any(ca => ca.GetType() == typeof(Pk));
        }

        // includes only those properties declared in the subclass (columns in db would be duplicates otherwise) but leaves the primary key because we need it to reference the superclass
        private static IEnumerable<PropertyInfo> HandleInheritance(IEnumerable<PropertyInfo> propInfos, Type tableType)
        {
            return propInfos.Where(propInfo => propInfo.DeclaringType == tableType || IsPk(propInfo));
        }
    }
}