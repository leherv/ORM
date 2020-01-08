using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Lib.Query.Where
{
    public class WhereFilter : ISqlExpression
    {
        private ISqlExpression _sqlExpression;

        public WhereFilter(ISqlExpression sqlExpression)
        {
            _sqlExpression = sqlExpression;
        }

        public string AsSqlString()
        {
            return _sqlExpression.AsSqlString();
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            return _sqlExpression.GetNamedParams();
        }
    }
}
