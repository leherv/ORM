using System.Collections.Generic;
using System.Linq;
using System.Text;
using ORM_Lib.Constraints_Attributes;
using ORM_Lib.DbSchema;

namespace ORM_Lib.Ddl
{
    public static class SchemaSqlBuilder
    {
        public static string BuildDdl(Schema schema)
        {
            var ddlBuilder = new StringBuilder();
            foreach (var entity in schema.Entities)
            {
                ddlBuilder.Append(EntitySqlBuilder.BuildDdl(entity) + ";");
                ddlBuilder.AppendLine();
            }

            ddlBuilder.AppendLine();
            BuildManyToMany(schema, ddlBuilder);
            BuildConstraints(schema, ddlBuilder);
            return ddlBuilder.ToString();
        }
        
        private static void BuildManyToMany(Schema schema, StringBuilder ddlBuilder)
        {
            var manyToMany = schema.Entities
                .SelectMany(e => e.Columns)
                .SelectMany(cols => cols.Constraints)
                .Where(constraint => constraint.GetType() == typeof(ManyToMany))
                .Select(m => m as ManyToMany)
                .Distinct(new ManyToManyComparer())
                .ToList();
                
            manyToMany    
                .Select(EntitySqlBuilder.BuildManyToManyDdl)
                .Aggregate(ddlBuilder, (builder, s) => builder.AppendLine(s + ";"));
            ddlBuilder.AppendLine();
            //TODO: maybe leave here - can not handle in BuildConstraints or it will be done twice
            manyToMany
                .Select(m => BuildPrimaryKey(m.TableName, new [] {m.ForeignKeyFar, m.ForeignKeyNear}))
                .Aggregate(ddlBuilder, (builder, s) => builder.AppendLine(s + ";"));
        }

        private static void BuildConstraints(Schema schema, StringBuilder ddlBuilder)
        {
            foreach (var entity in schema.Entities)
            {
                foreach (var column in entity.Columns.Where(c => c.IsDbColumn))
                {
                    foreach (var constraint in column.Constraints)
                    {
                        if (constraint.GetType() == typeof(Pk))
                        {
                            ddlBuilder.AppendLine(BuildPrimaryKey(entity.Name, new[] {column.Name}) + ";");
                        }
                        else if (constraint.GetType() == typeof(Fk))
                        {
                            var fk = constraint as Fk;
                            ddlBuilder.AppendLine(BuildForeignKey(entity.Name,
                                                      $"fk_{fk.ToEntity.Name}_{fk.To.Name}",
                                                      column.Name,
                                                      fk.ToEntity.Name,
                                                      fk.To.Name) + ";");
                        }
                        else if (constraint.GetType() == typeof(ManyToMany))
                        {
                            // ManyToMany has to occur twice so we always add constraint for foreign key near 
                            var manyToMany = constraint as ManyToMany;
                            ddlBuilder.AppendLine(BuildForeignKey(manyToMany.TableName,
                                                      $"fk_{entity.Name}_{entity.PkColumn.Name}",
                                                      manyToMany.ForeignKeyNear,
                                                      entity.Name,
                                                      entity.PkColumn.Name) + ";");
                        }
                        else if (constraint.GetType() == typeof(ManyToOne))
                        {
                            var manyToOne = constraint as ManyToOne;
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
            return $"ALTER TABLE {tableName} ADD PRIMARY KEY ({columnName.Aggregate("", (s1, s2) => s1 == "" ? $"{s2}" : $"{s1}, {s2}")})";
        }
    }
}