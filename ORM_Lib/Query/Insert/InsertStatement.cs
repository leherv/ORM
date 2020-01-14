using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORM_Lib.Query.Insert
{
    public class InsertStatement<T> : ISqlExpression
    {
        internal Entity _entityExecutedOn;
        internal Entity _superEntity;
        internal List<Column> _insertColumns;
        internal List<Column> _sEInsertColumns;
        internal List<T> _pocos;
        internal List<List<NamedParameter>> _namedParameters;
        internal List<List<NamedParameter>> _sENamedParameters;
        internal DbContext _ctx;

        internal InsertStatement(DbContext ctx, Entity entityExecutedOn, Entity superEntity, List<Column> insertColumns, List<Column> sEInsertColumns, List<T> pocos)
        {
            _ctx = ctx;
            _entityExecutedOn = entityExecutedOn;
            _superEntity = superEntity;
            _insertColumns = insertColumns;
            _sEInsertColumns = sEInsertColumns;
            _pocos = pocos;
            _namedParameters = BuildNamedParams(_insertColumns.Where(c => !c.IsPkColumn).ToList());
            // so we dont include the id in the VALUES part (we inject the result of evaluating the WITH there)
            _sENamedParameters = BuildNamedParams(_sEInsertColumns);
        }

        public void Execute()
        {
            Database db = _ctx.Database;
            db.ExecuteInsert(this);
        }


        public string AsSqlString()
        {
            if (_superEntity != null)
            {
                var insert = "";
                var alias = RandomStringGenerator.RandomString(8);
                var with = $"WITH {alias} AS (" +
                            $"INSERT INTO {_superEntity.Name} ({BuildInsertColumns(_sEInsertColumns)}) " +
                            $"VALUES " +
                            $"{BuildValues(_sENamedParameters)} " +
                            $"RETURNING { _entityExecutedOn.PkColumn.Name}) ";
                insert += with;
            

                insert += $"INSERT INTO {_entityExecutedOn.Name} ({BuildInsertColumns(_insertColumns)}) " +
                          $"VALUES " +
                          $"{BuildValues(_namedParameters, alias)} " +
                          $"RETURNING {_entityExecutedOn.PkColumn.Name}";
                return insert;
            }
            else
            {
                return $"INSERT INTO {_entityExecutedOn.Name} ({BuildInsertColumns(_insertColumns)}) " +
                   $"VALUES " +
                   $"{BuildValues(_namedParameters)} " +
                   $"RETURNING {_entityExecutedOn.PkColumn.Name}";
            }
        }

        private string SelectAlias(string alias, int offset)
        {
            return $"SELECT * FROM {alias} LIMIT 1 OFFSET {offset}";
        }

        private string BuildInsertColumns(List<Column> cols)
        {
            return cols
                .Select(c => c.Name)
                .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1}, {c2}");
        }

        private string BuildValues(List<List<NamedParameter>> namedParameters)
        {
            return namedParameters
                .Select(singleInsert => BuildValue(singleInsert))
                .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1}, {c2}");
        }

        private string BuildValues(List<List<NamedParameter>> namedParameters, string alias)
        {
            var i = 0;
            return namedParameters
               .Select(singleInsert => BuildValue(singleInsert, alias, i++))
               .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1}, {c2}");
        }

        private string BuildValue(List<NamedParameter> singleInsert)
        {
            return "(" +
                singleInsert
                    .Select(namedParam => namedParam.Alias)
                    .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1}, {c2}")
                + ")";
        }

        private string BuildValue(List<NamedParameter> singleInsert, string alias, int offset)
        {
            var pkIndex = _insertColumns.FindIndex(c => c.IsPkColumn);
            var j = 0;
            var result = "(";
            for(var i = 0; i < _insertColumns.Count; i++)
            {
                if (i == pkIndex)
                {
                    result += $"({SelectAlias(alias, offset)})";
                } else
                {
                    result += singleInsert[j++].Alias;
                }
                if (i < _insertColumns.Count - 1) result += ",";
            }
            return result += ")";
        }

        private List<List<NamedParameter>> BuildNamedParams(List<Column> cols)
        {
            var namedParams = new List<List<NamedParameter>>();
            for (var i = 0; i < _pocos.Count; i++)
            {
                var singleInsert = new List<NamedParameter>();
                foreach (var col in cols)
                {
                    // get the value of poco and handle if its enum
                    var value = col.PropInfo.GetMethod.Invoke(_pocos[i], new object[0]);
                    if (col.PropInfo.PropertyType.IsEnum) value = value.ToString();
                    singleInsert.Add(
                        new NamedParameter(
                            $"@{col.Name}{i}",
                            value,
                            col.DbType.PStmtDbType
                        )
                    );
                }
                namedParams.Add(singleInsert);
            }
            return namedParams;
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            return _namedParameters.SelectMany(singleInsert => singleInsert).Concat(_sENamedParameters.SelectMany(singleInsert => singleInsert));
        }

        void ISqlExpression.SetContextInformation(Entity entity)
        {

        }
    }
}
