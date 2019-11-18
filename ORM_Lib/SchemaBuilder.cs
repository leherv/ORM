using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ORM_Lib.Constraints_Attributes;
using ORM_Lib.Extensions;

namespace ORM_Lib
{
    public static class SchemaBuilder
    {
        public static Schema BuildSchema(IEnumerable<Type> types)
        {
            var tableTypes = types.ToList();
            var entities = new List<Entity>();

            foreach (var type in tableTypes)
            {
                var propertyInfos = FilterProperties(type).ToList();
                var returnTypes = propertyInfos.Select(pInfo => pInfo.PropertyType);
                if (!ValidColumnTypes(returnTypes, tableTypes))
                    throw new InvalidOperationException("Type is no DBSet or not supported by the Database");

                // we seem to have to pass tableTypes 
                entities.Add(EntityBuilder.BuildEntity(type, propertyInfos, tableTypes));
            }

            foreach (var entity in entities)
            {
                foreach (var column in entity.Columns)
                {
                    foreach (var constraint in column.Constraints)
                    {
                        if (typeof(Fk) == constraint.GetType())
                        {
                            var fk = constraint as Fk;
                            var toType = fk.ToPocoType;
                            var toEntity = entities.First(e => e.PocoType == toType);
                            fk.ToEntity = toEntity;
                            fk.To = toEntity.PkColumn;
                        }
                        else if (typeof(OneToMany) == constraint.GetType())
                        {
                            var oneToMany = constraint as OneToMany;
                            var mappingEntity = entities.First(e => e.PocoType == oneToMany.MappedByPocoType);
                            oneToMany.MappedByEntity = mappingEntity;
//                            get the Column with the ManyToOne annotation - if we dont find exactly one we can throw because something is seriously wrong
                            // TODO: may need to change later - at the moment we only allow bidirectional ManyToOne and OneToMany!
                            oneToMany.MappedByProperty = mappingEntity.Columns.Single(c => c.PropInfo.PropertyType == entity.PocoType).PropInfo;
                        }
                        else if (typeof(ManyToOne) == constraint.GetType())
                        {
                            var manyToOne = constraint as ManyToOne;
                            var toType = manyToOne.ToPocoType;
                            var toEntity = entities.First(e => e.PocoType == toType);
                            manyToOne.ToEntity = toEntity;
                            manyToOne.To = toEntity.PkColumn;
                        }
                        else if (typeof(ManyToMany) == constraint.GetType())
                        {
                            var manyToMany = constraint as ManyToMany;
                            manyToMany.FromEntity = entity;
                            var toType = manyToMany.ToPocoType;
                            var toEntity = entities.First(e => e.PocoType == toType);
                            manyToMany.ToEntity = toEntity;
                        }
                    }
                }
            }
            return new Schema(entities);
        }

        private static void initializeSchema()
        {
            
        }


     

        /// <summary>
        /// filters a class of type t and returns only Properties that dont have the attribute Ignore or no attribute at all
        /// </summary>
        /// <param name="t">Type of class that should be filtered</param>
        /// <returns>IEnumerable<PropertyInfo></returns>
        private static IEnumerable<PropertyInfo> FilterProperties(Type t)
        {
            return t.GetProperties()
                .Where(propInfos =>
                    !propInfos.GetCustomAttributes().Any() ||
                    propInfos.GetCustomAttributes().Any(ca => ca.GetType() != typeof(Ignore))
                );
        }


        private static IEnumerable<PropertyInfo> FilterManyToMany(IEnumerable<PropertyInfo> propertyInfos)
        {
            return propertyInfos.Where(propertyInfo =>
                propertyInfo.GetCustomAttributes().Any(cA => typeof(ManyToMany) == cA.GetType()));
        }


        /// <summary>
        /// Checks if the property return types in the DbSets are valid as a columntype. They either have to be able to be mapped by the TypeMapper
        /// or be of type of another DbSet or be an enum. 
        /// </summary>
        /// <param name="columnTypes">Types to check</param>
        /// <param name="tableTypes">Types of other DbSets</param>
        /// <returns>true when valid, false when not</returns>
        private static bool ValidColumnTypes(IEnumerable<Type> columnTypes, IEnumerable<Type> tableTypes)
        {
            return columnTypes
                .Select(cType => cType.IsNonStringEnumerable() ? cType.GetGenericArguments()[0] : cType)
                .Where(columnType => string.IsNullOrEmpty(TypeMapper.GetDbType(columnType)) && !columnType.IsEnum)
                .All(tableTypes.Contains);
        }


       
    }
}