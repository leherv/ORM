using ORM_Lib.DbSchema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace ORM_Lib.Deserialization
{
    class ObjectReader<T>
    {

        private DbDataReader DataReader { get; }
        private Entity Entity { get; }

        private List<Column> QueriedColumns { get; }

        public ObjectReader(DbDataReader dataReader, Entity entity, List<Column> queriedColumns)
        {
            DataReader = dataReader;
            Entity = entity;
            QueriedColumns = queriedColumns;
        }


        public IEnumerable<T> Serialize()
        {
            var result = new List<T>();
            while (DataReader.Read())
            {
                result.Add(CreateObject(DataReader));
            }
            return result;
        }


        //TODO: dont forget that inheritance needs to fetch columns from superclass too and leave Pk out
        private T CreateObject(IDataReader reader)
        {
            var poco = ObjectFactory.Create(Entity.PocoType);
            //TODO: why cant i use LINQ here
            foreach (var col in QueriedColumns)
            {
                var value = reader[col.Name];
                if (!col.IsShadowAttribute)
                {
                    // specially handle enums
                    // TODO: doc enums will always be stored as strings
                    if (col.PropInfo.PropertyType.IsEnum)
                    {
                        value = Enum.Parse(col.PropInfo.PropertyType, value as string);
                    }
                    col.PropInfo.SetMethod.Invoke(poco, new[] { value });
                };
            }
            return (T)poco;
        }
    }
}
