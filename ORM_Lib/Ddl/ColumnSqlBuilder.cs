using ORM_Lib.DbSchema;
using ORM_Lib.TypeMapper;

namespace ORM_Lib.Ddl
{
    internal static class ColumnSqlBuilder
    {
        public static string BuildDdl(Column column)
        {
            var sql = $"{column.Name} {column.DbType.DdlDbType}";
            if (column.IsPkColumn)
                sql += " PRIMARY KEY";
            return sql;
        }

        public static string BuildManyToManyDdl(string fkName, ITypeMapper typeMapper)
        {
            return $"{fkName} {typeMapper.GetForeignKeyType()} ";
        }
    }
}