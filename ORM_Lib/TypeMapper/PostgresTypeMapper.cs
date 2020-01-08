using System;
using System.Collections.Generic;

namespace ORM_Lib.TypeMapper
{
    public class PostgresTypeMapper : ITypeMapper
    {
        private static readonly Dictionary<Type, string> TypeMap = new Dictionary<Type, string>
        {
            {typeof(byte), "smallint"},
            {typeof(sbyte), "smallint"},
            {typeof(short), "smallint"},
            {typeof(ushort), "integer"},
            {typeof(int), "integer"},
            {typeof(uint), "oid"},
            {typeof(long), "bigint"},
            {typeof(ulong), null},
            {typeof(float), "real"},
            {typeof(double), "double precision"},
            {typeof(decimal), "numeric"},
            {typeof(bool), "boolean"},
            {typeof(string), "text"},
            {typeof(char), "text"},
            {typeof(Guid), null},
            {typeof(DateTime), "timestamp"},
            {typeof(DateTimeOffset), null},
            {typeof(byte[]), "bytea"},
            {typeof(byte?), "smallint"},
            {typeof(sbyte?), "smallint"},
            {typeof(short?), "smallint"},
            {typeof(ushort?), "integer"},
            {typeof(int?), "integer"},
            {typeof(uint?), "oid"},
            {typeof(long?), "bigint"},
            {typeof(ulong?), null},
            {typeof(float?), "real"},
            {typeof(double?), "double precision"},
            {typeof(decimal?), "numeric"},
            {typeof(bool?), "boolean"},
            {typeof(char?), "text"},
            {typeof(char[]), "text"},
        };

        public string GetDbType(Type type)
        {
            TypeMap.TryGetValue(type, out var a);
            return a;
        }

        public string GetForeignKeyType() => "integer";
    }
}