using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Lib.Query.Update
{
    internal class UpdateColumnStatement : ISqlExpression
    {

        private string _columnName;
        private ValueExpression _valueExpression;

        public UpdateColumnStatement(string columnName, ValueExpression valueExpression)
        {
            _columnName = columnName;
            _valueExpression = valueExpression;
        }

        public string AsSqlString()
        {
            return $"{_columnName} = {_valueExpression.AsSqlString()}";
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            return _valueExpression.GetNamedParams();
        }

        void ISqlExpression.SetContextInformation(Entity entity)
        {
        }
    }
}
