using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using System.Collections.Generic;
using System.Linq;

namespace ORM_Lib.Query.Insert
{
    public class InsertStatement<T> : ISqlExpression
    {
        internal Entity _entityExecutedOn;
        internal List<Column> _insertColumns;
        internal List<T> _pocos;
        internal List<List<NamedParameter>> _namedParameters;
        internal DbContext _ctx;
        internal WithStatement _with;

        internal InsertStatement(DbContext ctx, Entity entityExecutedOn, List<Column> insertColumns, WithStatement withStatement, List<T> pocos)
        {
            _ctx = ctx;
            _entityExecutedOn = entityExecutedOn;
            _insertColumns = insertColumns;
            _pocos = pocos;
            _namedParameters = BuildNamedParams(_insertColumns);
            _with = withStatement;
        }

        public List<T> Execute()
        {
            Database db = _ctx.Database;
            return db.ExecuteInsert(this);
        }

        public string AsSqlString()
        {
            var insert = "";
            if (_with != null) insert = _with.AsSqlString();
            insert += $"INSERT INTO {_entityExecutedOn.Name} ({BuildInsertColumns(_insertColumns)}) " +
                      $"VALUES ";
            insert += _with != null ? BuildValues(_namedParameters, _with.Alias) : BuildValues(_namedParameters);
            insert += $" RETURNING {_entityExecutedOn.PkColumn.Name}";
            return insert;
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
            var para = _namedParameters.SelectMany(singleInsert => singleInsert);
            return _with != null ? para.Concat(_with.GetNamedParams()) : para;
        }

        void ISqlExpression.SetContextInformation(Entity entity)
        {

        }
    }
}
