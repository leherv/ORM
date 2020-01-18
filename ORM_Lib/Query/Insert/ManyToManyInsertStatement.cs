using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using ORM_Lib.TypeMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORM_Lib.Query.Insert
{
    class ManyToManyInsertStatement : ISqlExpression
    {

        private string _tableName;
        private (string, string) _columnNames;
        private HashSet<(object, object)> _values;
        private List<List<NamedParameter>> _namedParameters;

        public ManyToManyInsertStatement(string tableName, (string, string) columnNames, HashSet<(object, object)> values)
        {
            _tableName = tableName;
            _columnNames = columnNames;
            _values = values;
            _namedParameters = BuildNamedParams();
        }

        public string AsSqlString()
        {
            return $"INSERT INTO {_tableName} ({_columnNames.Item1}, {_columnNames.Item2}) " +
                   $"VALUES " +
                   $"{BuildValues()};";
        }


        private string BuildValues()
        {
            return _namedParameters
               .Select(singleInsert => BuildValue(singleInsert))
               .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1}, {c2}");
        }


        public string BuildValue(List<NamedParameter> singleInsert)
        {
            return "(" +
                singleInsert
                    .Select(namedParam => namedParam.Alias)
                    .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1}, {c2}")
                + ")";
        }

        private List<List<NamedParameter>> BuildNamedParams()
        {
            var namedParams = new List<List<NamedParameter>>();
            var i = 0;
            foreach((object fk_col1, object fk_col2) in _values)
            {
                var singleLine = new List<NamedParameter>();
                singleLine.Add(new NamedParameter($"@{_columnNames.Item1}{i}", fk_col1, PreparedStatementTypeMapper.GetForeignKeyType()));
                singleLine.Add(new NamedParameter($"@{_columnNames.Item2}{i++}", fk_col2, PreparedStatementTypeMapper.GetForeignKeyType()));
                namedParams.Add(singleLine);
            }
            return namedParams;
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            return _namedParameters.SelectMany(singleInsert => singleInsert);
        }

        void ISqlExpression.SetContextInformation(Entity entity)
        {
            
        }
    }
}
