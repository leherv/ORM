using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ORM_Lib.Constraints_Attributes;
using ORM_Lib.Extensions;

namespace ORM_Lib
{
    public class SchemaBuilder
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

                entities.Add(EntityBuilder.BuildEntity(type, propertyInfos));
            }

            FinishSchema();

            return new Schema(entities);
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

        
        // iterates over all entities and their columns and then their constraints 
        // resolves the constraints by building the references to the columns (that we now already created)
        private static void FinishSchema()
        {
            
        }
    }
}