using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ORM_Lib.Attributes;
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
            var constraints = new HashSet<Constraint>();
            Relation relation = null;

            // TODO: FK are not handled at all?!
            foreach (var propertyInfoCustomAttribute in propertyInfo.GetCustomAttributes())
            {
                if (propertyInfoCustomAttribute.GetType() == typeof(Pk))
                {
                    constraints.Add(propertyInfoCustomAttribute as Constraint);
                    var superClass = tableTypes.Where(tableType.IsSubclassOf).ToList();
                    // if its a primary key and it is a subclass then the teacher for example is also a person and needs a foreignkey on the personclass primary id
                    if (superClass.Any())
                    {
                        relation = new Fk(superClass.First());
                    }
                }
                else if (propertyInfoCustomAttribute.GetType() == typeof(ManyToMany))
                {
                    var manyToMany = propertyInfoCustomAttribute as ManyToMany;
                    manyToMany.ToPocoType = propertyInfo.PropertyType.GetGenericArguments()[0];
                    relation = manyToMany;
                }
                else if (propertyInfoCustomAttribute.GetType() == typeof(OneToMany))
                {
                    var oneToMany = propertyInfoCustomAttribute as OneToMany;
                    oneToMany.MappedByPocoType = propertyInfo.PropertyType.GetGenericArguments()[0];
                    relation = propertyInfoCustomAttribute as Relation;
                }
                else if (propertyInfoCustomAttribute.GetType() == typeof(ManyToOne))
                {
                    var manyToOne = propertyInfoCustomAttribute as ManyToOne;
                    manyToOne.ToPocoType = propertyInfo.PropertyType;
                    manyToOneUsed = true;
                    relation = manyToOne;
                    isShadowAttribute = true;
                }
                else
                {
                    constraints.Add(propertyInfoCustomAttribute as Constraint);
                }
            }

            // TODO: think about avoiding checking by customattribute and naming separately (problem if no customattributes no loop)
            if ("id".Equals(propertyInfo.Name.ToLower()))
            {
                constraints.Add(new Pk());
                var superClass = tableTypes.Where(tableType.IsSubclassOf).ToList();
                if (superClass.Any())
                {
                    relation = new Fk(superClass.First());
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
                    relation = new Fk(oneToOne.First());
                }
            }

            return new Column(
                BuildColumnName(propertyInfo),
                relation,
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
                    new Fk(from),
                    new HashSet<Constraint>(),
                    null,
                    new OrmDbType(typeMapper.GetForeignKeyType(), PreparedStatementTypeMapper.Map(from)),
                    true
                ),
                new Column(
                    m.ForeignKeyFar,
                    new Fk(to),
                    new HashSet<Constraint>(),
                    null,
                    new OrmDbType(typeMapper.GetForeignKeyType(), PreparedStatementTypeMapper.Map(from)),
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

        private static OrmDbType DbType(Type t, List<Type> tableTypes, HashSet<Constraint> constraints, ITypeMapper typeMapper)
        {
            var ddlType = typeMapper.GetDbType(t);
            var pstmtType = PreparedStatementTypeMapper.Map(t);
            if (tableTypes.Any(tType => tType == t))
            {
                ddlType = typeMapper.GetForeignKeyType();
                pstmtType = PreparedStatementTypeMapper.GetForeignKeyType();
            }
            if (constraints.Any(c => c.GetType() == typeof(Pk))) ddlType = typeMapper.GetPrimaryKeyType();
            // because we always want to enforce enums to be text
            if (t.IsEnum) ddlType = typeMapper.GetEnumType();

            return new OrmDbType(
                ddlType,
                pstmtType
            );
        }
    }
}