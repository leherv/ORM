using ORM_Lib.DbSchema;
using ORM_Lib.TypeMapper;

namespace ORM_Lib.Ddl
{
    internal static class ColumnSqlBuilder
    {
        public static string BuildDdl(Column column)
        {
            return $"{column.Name} {column.DbType.DdlDbType}";
        }

        public static string BuildManyToManyDdl(string fkName, ITypeMapper typeMapper)
        {
            return $"{fkName} {typeMapper.GetForeignKeyType()}";
        }
    }
}