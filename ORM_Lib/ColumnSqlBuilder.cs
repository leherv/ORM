namespace ORM_Lib
{
    public static class ColumnSqlBuilder
    {
        public static string BuildDdl(Column column)
        {
            return $"{column.Name} {column.DbType}";
        }

        public static string BuildManyToManyDdl(string fkName)
        {
            return $"{fkName} {TypeMapper.GetForeignKeyType()}";
        }
    }
}