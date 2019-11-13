using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ORM_Lib.Constraints_Attributes;

namespace ORM_Lib
{
    public class ColumnBuilder
    {
        public static Column BuildColumn(PropertyInfo propertyInfo)
        {
            var constraints = new List<IConstraint>();
            if ("id".Equals(propertyInfo.Name.ToLower())) constraints.Add(new Pk());
            foreach (var propertyInfoCustomAttribute in propertyInfo.GetCustomAttributes())
            {
                if (propertyInfoCustomAttribute.GetType() == typeof(Pk))
                {
                    if (constraints.Any(con => typeof(Pk) == con.GetType()))
                        throw new InvalidOperationException("A class can have only one Primary Key!");
                    constraints.Add(propertyInfoCustomAttribute as IConstraint);
                }
                else if (propertyInfoCustomAttribute.GetType() == typeof(ManyToMany))
                {
                    constraints.Add(propertyInfoCustomAttribute as IConstraint);
                }
            }

            return new Column(
                BuildColumnName(propertyInfo),
                constraints,
                propertyInfo
            );
        }


        private static string BuildColumnName(MemberInfo t)
        {
            var customName = t.GetCustomAttributes()
                .Where(cA => cA.GetType() == typeof(ColumnName))
                .Select(cA => cA as ColumnName)
                .Select(cA => cA?.CName)
                .SingleOrDefault();

            return string.IsNullOrEmpty(customName) ? t.Name : customName;
        }
    }
}