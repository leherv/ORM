using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ORM_Lib.Constraints_Attributes;
using ORM_Lib.Extensions;

namespace ORM_Lib
{
    public static class ColumnBuilder
    {
        public static Column BuildColumn(PropertyInfo propertyInfo, Type tableType, List<Type> tableTypes)
        {
            var manyToOneUsed = false;
            var isDbColumn = true;
            var constraints = new HashSet<IConstraint>();

            foreach (var propertyInfoCustomAttribute in propertyInfo.GetCustomAttributes())
            {
                if (propertyInfoCustomAttribute.GetType() == typeof(Pk))
                {
                    constraints.Add(propertyInfoCustomAttribute as IConstraint);
                    var superClass = tableTypes.Where(tableType.IsSubclassOf).ToList();
                    // if its a primary key and it is a subclass then the teacher for example is also a person and needs a foreignkey on the personclass primary id
                    if (superClass.Any())
                    {
                        constraints.Add(new Fk(superClass.First()));
                    }
                }
                else if (propertyInfoCustomAttribute.GetType() == typeof(ManyToMany))
                {
                    var manyToMany = propertyInfoCustomAttribute as ManyToMany;
                    manyToMany.ToPocoType = propertyInfo.PropertyType.GetGenericArguments()[0];
                    manyToMany.FromPocoType = tableType;
                    isDbColumn = false;
                    constraints.Add(manyToMany);
                }
                else if (propertyInfoCustomAttribute.GetType() == typeof(OneToMany))
                {
                    var oneToMany = propertyInfoCustomAttribute as OneToMany;
                    oneToMany.MappedByPocoType = propertyInfo.PropertyType.GetGenericArguments()[0];
                    constraints.Add(propertyInfoCustomAttribute as IConstraint);
                    isDbColumn = false;
                }
                else if (propertyInfoCustomAttribute.GetType() == typeof(ManyToOne))
                {
                    var manyToOne = propertyInfoCustomAttribute as ManyToOne;
                    manyToOne.ToPocoType = propertyInfo.PropertyType;
                    constraints.Add(manyToOne);
                    manyToOneUsed = true;
                }
            }

            // TODO: think about avoiding checking by customattribute and naming separately (problem if no customattributes no loop)
            if ("id".Equals(propertyInfo.Name.ToLower()))
            {
                constraints.Add(new Pk());
                var superClass = tableTypes.Where(tableType.IsSubclassOf).ToList();
                if (superClass.Any())
                {
                    constraints.Add(new Fk(superClass.First()));
                }
            }

            // now check if it is a regular 1-1 relationship
            // beware of doing this double later!
            // TODO: think about avoiding doing it like this too
            if (!manyToOneUsed)
            {
                var oneToOne = tableTypes.Where(tType => propertyInfo.PropertyType == tType).ToList();
                if (oneToOne.Any())
                {
                    constraints.Add(new Fk(oneToOne.First()));
                }
            }

            return new Column(
                BuildColumnName(propertyInfo),
                constraints,
                propertyInfo,
                DbType(propertyInfo.PropertyType, tableTypes, constraints),
                isDbColumn
            );
        }


        public static List<Column> BuildInterimColumns(ManyToMany m, Type from, Type to)
        {
            return new List<Column>()
            {
                new Column(
                    m.ForeignKeyNear,
                    new HashSet<IConstraint>() {new Fk(from)},
                    null,
                    TypeMapper.GetForeignKeyType(),
                    true),
                new Column(
                    m.ForeignKeyFar,
                    new HashSet<IConstraint>() {new Fk(to)},
                    null,
                    TypeMapper.GetForeignKeyType(),
                    true)
            };
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

        private static string DbType(Type t, List<Type> tableTypes, HashSet<IConstraint> constraints)
        {
            if (constraints.Any(c => c.GetType() == typeof(Pk))) return "serial";
            if (t.IsEnum) return "text";
            if (t.IsNonStringEnumerable()) t = t.GetGenericArguments()[0];
            var dbType = TypeMapper.GetDbType(t);
            if (!string.IsNullOrEmpty(dbType)) return dbType;
            if (tableTypes.Any(tType => tType == t))
            {
                dbType = TypeMapper.GetForeignKeyType();
            }

            return dbType;
        }
    }
}