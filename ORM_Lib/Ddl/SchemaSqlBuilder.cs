using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORM_Lib.Attributes;
using ORM_Lib.DbSchema;
using ORM_Lib.TypeMapper;

namespace ORM_Lib.Ddl
{
    internal static class SchemaSqlBuilder
    {
        public static string BuildDdl(Schema schema, ITypeMapper typeMapper)
        {
            var ddlBuilder = new StringBuilder();
            foreach (var entity in schema.Entities)
            {
                ddlBuilder.Append(EntitySqlBuilder.BuildDdl(entity) + ";");
                ddlBuilder.AppendLine();
            }

            ddlBuilder.AppendLine();
            BuildManyToMany(schema, ddlBuilder, typeMapper);
            BuildConstraintsAndRelation(schema, ddlBuilder);
            return ddlBuilder.ToString();
        }

        private static void BuildManyToMany(Schema schema, StringBuilder ddlBuilder, ITypeMapper typeMapper)
        {
            var manyToMany = schema.Entities
                .SelectMany(e => e.Columns)
                .Select(cols => cols.Relation)
                .Where(relation => relation?.GetType() == typeof(ManyToMany))
                .Select(m => m as ManyToMany)
                .Distinct(new ManyToManyComparer())
                .ToList();

            manyToMany
                .Select(m => EntitySqlBuilder.BuildManyToManyDdl(m, typeMapper))
                .Aggregate(ddlBuilder, (builder, s) => builder.AppendLine(s + ";"));
            ddlBuilder.AppendLine();
            manyToMany
                .Select(m => BuildPrimaryKey(m.TableName, new[] { m.ForeignKeyFar, m.ForeignKeyNear }))
                .Aggregate(ddlBuilder, (builder, s) => builder.AppendLine(s + ";"));
        }

        private static void BuildConstraintsAndRelation(Schema schema, StringBuilder ddlBuilder)
        {
            foreach (var entity in schema.Entities)
            {
                foreach (var column in entity.Columns.Where(c => c.IsDbColumn))
                {

                    // constraints

                    foreach (var constraint in column.Constraints)
                    {
                        if (constraint.GetType() == typeof(Pk))
                        {
                            ddlBuilder.AppendLine(BuildPrimaryKey(entity.Name, new[] { column.Name }) + ";");
                        }
                    }

                    // now relation
                    var relation = column.Relation;
                    if (relation != null)
                    {
                        if (relation.GetType() == typeof(Fk))
                        {
                            var fk = relation as Fk;
                            ddlBuilder.AppendLine(BuildForeignKey(entity.Name,
                                                      $"fk_{fk.ToEntity.Name}_{fk.To.Name}",
                                                      column.Name,
                                                      fk.ToEntity.Name,
                                                      fk.To.Name) + ";");
                        }
                        else if (relation.GetType() == typeof(ManyToMany))
                        {
                            // ManyToMany has to occur twice so we always add constraint for foreign key near 
                            var manyToMany = relation as ManyToMany;
                            ddlBuilder.AppendLine(BuildForeignKey(manyToMany.TableName,
                                                      $"fk_{entity.Name}_{entity.PkColumn.Name}",
                                                      manyToMany.ForeignKeyNear,
                                                      entity.Name,
                                                      entity.PkColumn.Name) + ";");
                        }
                        else if (relation.GetType() == typeof(ManyToOne))
                        {
                            var manyToOne = relation as ManyToOne;
                            ddlBuilder.AppendLine(BuildForeignKey(entity.Name,
                                                      $"fk_{manyToOne.ToEntity.Name}_{manyToOne.To.Name}",
                                                      column.Name,
                                                      manyToOne.ToEntity.Name,
                                                      manyToOne.To.Name) + ";");
                        }
                    }
                }
            }
        }

        private static string BuildForeignKey(string tableName, string constraintName, string fkColumnName,
            string tableNameTo, string columnNameTo)
        {
            return
                $"ALTER TABLE {tableName} ADD CONSTRAINT {constraintName} FOREIGN KEY ({fkColumnName}) REFERENCES {tableNameTo} ({columnNameTo})";
        }

        private static string BuildPrimaryKey(string tableName, IEnumerable<string> columnName)
        {
            return
                $"ALTER TABLE {tableName} ADD PRIMARY KEY ({columnName.Aggregate("", (s1, s2) => s1 == "" ? $"{s2}" : $"{s1}, {s2}")})";
        }
    }
}