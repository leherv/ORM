using System.Collections.Generic;
using System.Linq;
using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;

namespace ORM_Lib.Query.Select
{
    // class is public because user has to be able to see it and execute - Execute | but he is not allowed to create an instance himself constructor internal
    public class SelectQuery<T> : ISqlExpression
    {
        internal DbContext _ctx;
        internal List<Column> _combinedQueryColumns;
        internal Entity _entityExecutedOn;
        internal List<IWhereFilter> _whereFilter;
        internal List<Join> _joins;

        internal SelectQuery(DbContext ctx, Entity entityExecutedOn, List<Column> combinedQueryColumns, List<IWhereFilter> whereFilter, List<Join> joins)
        {
            _ctx = ctx;
            _entityExecutedOn = entityExecutedOn;
            _combinedQueryColumns = combinedQueryColumns;
            _whereFilter = whereFilter;
            foreach (var w in _whereFilter)
            {
                w.SetContextInformation(_entityExecutedOn);
            }
            _joins = joins;
        }

        public IEnumerable<T> Execute()
        {
            Database db = _ctx.Database;
            // we send all the queriedColumns here to decide which to set on the object later
            var result = db.ExecuteQuery(this);
            return result;
        }

        //
        private string BuildFrom()
        {
            var from = $"{_entityExecutedOn.Name} {_entityExecutedOn.Alias}";
            if (_joins.Count > 0)
                from += $" {BuildJoins()}";
            return from;
        }

        private string BuildJoins()
        {
            return _joins
                .Select(j => $"{j.AsSqlString()}")
                .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1} {c2}");
        }

        private string BuildWhere()
        {
            return _whereFilter
                .Select(w => $"{w.AsSqlString()}")
                .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1} AND {c2}");

        }

        public string AsSqlString()
        {
            var query =
                   $"SELECT {CommaSeparatedColumns()} " +
                   $"FROM {BuildFrom()}";
            if (_whereFilter.Count > 0)
            {
                query += $" WHERE {BuildWhere()}";
            }
            return query += ";";
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            return _whereFilter.SelectMany(wF => wF.GetNamedParams());
        }

        private string CommaSeparatedColumns()
        {
            // we have to only query those columns present in the DB
            return _combinedQueryColumns
                // isindb
                .Where(c => c.IsDbColumn)
                .Select(c => $"{c.Entity.Alias}.{c.Name}")
                .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1}, {c2}");
        }


        void ISqlExpression.SetContextInformation(Entity entity)
        {

        }
    }
}