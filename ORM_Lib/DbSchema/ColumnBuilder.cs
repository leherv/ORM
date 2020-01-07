using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ORM_Lib.Constraints_Attributes;
using ORM_Lib.Extensions;
using ORM_Lib.TypeMapper;

namespace ORM_Lib.DbSchema
{
    internal static class ColumnBuilder
    {
        public static Column BuildColumn(PropertyInfo propertyInfo, Type tableType, List<Type> tableTypes, ITypeMapper typeMapper)
        {
            // property that is present in the db and in the object but needs to be initialized later
            var isShadowAttribute = false;
            var manyToOneUsed = false;
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
                    constraints.Add(manyToMany);
                }
                else if (propertyInfoCustomAttribute.GetType() == typeof(OneToMany))
                {
                    var oneToMany = propertyInfoCustomAttribute as OneToMany;
                    oneToMany.MappedByPocoType = propertyInfo.PropertyType.GetGenericArguments()[0];
                    constraints.Add(propertyInfoCustomAttribute as IConstraint);
                }
                else if (propertyInfoCustomAttribute.GetType() == typeof(ManyToOne))
                {
                    var manyToOne = propertyInfoCustomAttribute as ManyToOne;
                    manyToOne.ToPocoType = propertyInfo.PropertyType;
                    manyToOneUsed = true;
                    constraints.Add(manyToOne);
                    isShadowAttribute = true;

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
                DbType(propertyInfo.PropertyType, tableTypes, constraints, typeMapper),
                isShadowAttribute
            );
        }


        public static List<Column> BuildInterimColumns(ManyToMany m, Type from, Type to, ITypeMapper typeMapper)
        {
            return new List<Column>()
            {
                new Column(
                    m.ForeignKeyNear,
                    new HashSet<IConstraint>() {new Fk(from)},
                    null,
                    typeMapper.GetForeignKeyType(),
                    true
                ),
                new Column(
                    m.ForeignKeyFar,
                    new HashSet<IConstraint>() {new Fk(to)},
                    null,
                    typeMapper.GetForeignKeyType(),
                    true
                )
            };
        }

        private static string BuildColumnName(MemberInfo t)
        {
            var customName = t.GetCustomAttributes()
                .Where(cA => cA.GetType() == typeof(ColumnName))
                .Select(cA => cA as ColumnName)
                .Select(cA => cA?.CName)
                .SingleOrDefault();

            return string.IsNullOrEmpty(customName) ? t.Name.ToLower() : customName.ToLower();
        }

        private static string DbType(Type t, List<Type> tableTypes, HashSet<IConstraint> constraints, ITypeMapper typeMapper)
        {
            if (constraints.Any(c => c.GetType() == typeof(Pk))) return "serial";
            if (t.IsEnum) return "text";
            //was just to determine if valid but has no dbtype
            //if (t.IsNonStringEnumerable()) t = t.GetGenericArguments()[0];  
            var dbType = typeMapper.GetDbType(t);
            if (!string.IsNullOrEmpty(dbType)) return dbType;
            if (tableTypes.Any(tType => tType == t))
            {
                dbType = typeMapper.GetForeignKeyType();
            }

            return dbType;
        }
    }
}