using ORM_Lib.DbSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORM_Lib.Query.Where
{
    public class ColumnExpression : ISqlExpression
    {

        private string _columnName;
        private Entity _entity;

        public ColumnExpression(string columnName)
        {
            _columnName = columnName;
        }

        public string AsSqlString()
        {
            return $"{_entity.Alias}.{_columnName}";
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            return Enumerable.Empty<NamedParameter>();
        }

        void ISqlExpression.SetContextInformation(Entity entity)
        {
            _entity = entity;
        }
    }
}
