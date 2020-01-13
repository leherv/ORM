using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using System.Collections.Generic;
using System.Linq;

namespace ORM_Lib.Query.Select
{
    class Join : ISqlExpression
    {
        private string _entity1Alias;
        private string _entity1PkColumnName;

        private string _entity2Name;
        private string _entity2Alias;
        private string _entity2PkColumnName;

        public Join(string entity1Alias, string entity1PkColumnName, string entity2Name, string entity2Alias, string entity2PkColumnName)
        {
            _entity1Alias = entity1Alias;
            _entity1PkColumnName = entity1PkColumnName;
            _entity2Name = entity2Name;
            _entity2Alias = entity2Alias;
            _entity2PkColumnName = entity2PkColumnName;
        }

        public string AsSqlString()
        {
            return $"JOIN {_entity2Name} {_entity2Alias} ON {_entity1Alias}.{_entity1PkColumnName} = {_entity2Alias}.{_entity2PkColumnName}";
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            return Enumerable.Empty<NamedParameter>();
        }

        void ISqlExpression.SetContextInformation(Entity entity)
        {

        }
    }
}
