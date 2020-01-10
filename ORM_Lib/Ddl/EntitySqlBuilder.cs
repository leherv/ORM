using System.Linq;
using System.Text;
using ORM_Lib.Attributes;
using ORM_Lib.DbSchema;
using ORM_Lib.TypeMapper;

namespace ORM_Lib.Ddl
{
    internal static class EntitySqlBuilder
    {
        public static string BuildDdl(Entity entity)
        {
            var ddlBuilder = new StringBuilder();
            ddlBuilder.AppendLine($"CREATE TABLE IF NOT EXISTS {entity.Name}(");

            var colS = entity.Columns
                .Where(col => col.IsDbColumn)
                .Select(ColumnSqlBuilder.BuildDdl)
                .ToList();
            for (var i = 0; i < colS.Count; i++)
            {
                ddlBuilder.Append(colS[i]);
                if (i < colS.Count - 1) ddlBuilder.AppendLine(",");
            }
            ddlBuilder.AppendLine();
            ddlBuilder.Append(")");
            return ddlBuilder.ToString();
        }

        public static string BuildManyToManyDdl(ManyToMany m, ITypeMapper typeMapper)
        {
            var ddlBuilder = new StringBuilder();
            ddlBuilder.AppendLine($"CREATE TABLE IF NOT EXISTS {m.TableName}(");
            ddlBuilder.AppendLine($"{ColumnSqlBuilder.BuildManyToManyDdl(m.ForeignKeyNear, typeMapper)},");
            ddlBuilder.AppendLine($"{ColumnSqlBuilder.BuildManyToManyDdl(m.ForeignKeyFar, typeMapper)}");
            ddlBuilder.Append(")");
            return ddlBuilder.ToString();
        }
    }
}