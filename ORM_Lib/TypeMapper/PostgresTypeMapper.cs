using System;
using System.Collections.Generic;

namespace ORM_Lib.TypeMapper
{
    public class PostgresTypeMapper : ITypeMapper
    {
        private static readonly Dictionary<string, string> TypeMap = new Dictionary<string, string>
        {
            {"sbyte", "smallint"},
            // short is int16
            {"int16", "smallint"},
            {"int", "integer"},
            // int is int32
            {"int32", "integer"},
            {"long", "bigint"},
            // long is int64
            {"int64", "bigint"},
            {"byte", "smallint"},
            {"byte[]", "bytea"},
            {"uint16", "integer"},
            {"uint32", "oid"},
//            {"ulong",""}, -- no postgres type
            // float is single
            {"single", "real"},
            {"double", "double precision"},
            {"decimal", "numeric"},
            {"boolean", "boolean"},
            {"string", "text"},
            {"char", "text"},
            {"char[]", "text"},
            {"datetime", "timestamp"}
        };

        public string GetDbType(string type)
        {
            TypeMap.TryGetValue(type.ToLower(), out var a);
            return a;
        }

        public string GetDbType(Type type)
        {
            return GetDbType(type.Name);
        }

        public string GetForeignKeyType() => "integer";
    }
}