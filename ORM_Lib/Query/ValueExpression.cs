using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using ORM_Lib.TypeMapper;
using System.Collections.Generic;
using System.Data;

namespace ORM_Lib.Query
{
    public class ValueExpression : ISqlExpression
    {

        private DbType _dbType;
        private object _value;
        private Entity _entity;
        private string _alias;


        public ValueExpression(object value)
            : this(value, PreparedStatementTypeMapper.Map(value.GetType()))
        {
        }

        internal ValueExpression(object value, DbType dbType)
            : this(value, dbType, RandomStringGenerator.RandomString(4))
        {
        }

        internal ValueExpression(object value, DbType dbType, string alias)
        {
            _dbType = dbType;
            _value = value;
            _alias = alias;
        }

        public string AsSqlString()
        {
            return $"@{_alias}";
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            yield return new NamedParameter(_alias, _value, _dbType);
        }

        void ISqlExpression.SetContextInformation(Entity entity)
        {
            _entity = entity;
        }
    }
}
