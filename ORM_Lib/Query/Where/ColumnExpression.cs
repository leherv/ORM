using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Lib.Query.Where
{
    class ColumnExpression : ISqlExpression
    {


        private string _tableAlias;
        private string _columnName;

        public ColumnExpression(string tableAlias, string columnName)
        {
            _tableAlias = tableAlias;
            _columnName = columnName;
        }

        public string AsSqlString()
        {
            return $"{_tableAlias}.{_columnName}";
        }
    }
}
