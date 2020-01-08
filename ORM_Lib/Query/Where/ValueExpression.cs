using ORM_Lib.Query.Where;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ORM_Lib.Query
{
    class ValueExpression : ISqlExpression
    {

        private DbType _dbType;
        private string _alias;
        private object _value; 

        public ValueExpression(string alias, object value, DbType dbType)
        {
            _alias = alias;
            _dbType = dbType;
            _value = value;
        }
        
        public string AsSqlString()
        {
            return $"@{_alias}";
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            yield return new NamedParameter(_alias, _value, _dbType);
        }
    }
}
