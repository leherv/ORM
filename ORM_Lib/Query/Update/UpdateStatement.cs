using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using ORM_Lib.Saving;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ORM_Lib.Query.Update
{
    internal class UpdateStatement : ISqlExpression
    {

        private Entity _entity;
        private List<IWhereFilter> _whereFilter;
        private List<UpdateColumnStatement> _updateColumnStatements;

        internal UpdateStatement(Entity entity, List<IWhereFilter> whereFilter, List<UpdateColumnStatement> updateColumnStatements)
        {
            _entity = entity;
            _whereFilter = whereFilter;
            foreach (var w in _whereFilter)
            {
                w.SetContextInformation(_entity);
            }
            _updateColumnStatements = updateColumnStatements;
        }

        public string AsSqlString()
        {
            var query = $"UPDATE {_entity.Name} {_entity.Alias} " +
                        $"SET {BuildSetValues()}";
            if (_whereFilter.Count > 0)
            {
                query += $" WHERE {BuildWhere()}";
            }
            return query += ";";
        }

        private string BuildSetValues()
        {
            return _updateColumnStatements
                .Select(w => $"{w.AsSqlString()}")
                .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1}, {c2}");
        }

        private string BuildWhere()
        {
            return _whereFilter
                .Select(w => $"{w.AsSqlString()}")
                .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1} AND {c2}");
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            return _whereFilter
                .SelectMany(wF => wF.GetNamedParams())
                .Concat(_updateColumnStatements.SelectMany(up => up.GetNamedParams()));
        }

        void ISqlExpression.SetContextInformation(Entity entity)
        {

        }
    }
}
