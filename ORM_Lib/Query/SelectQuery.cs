using System.Collections.Generic;
using System.Linq;
using ORM_Lib.DbSchema;
using ORM_Lib.Deserialization;

namespace ORM_Lib.Query
{
    // class is public because user has to be able to see it and execute - Execute | but he is not allowed to create an instance himself constructor internal
    class SelectQuery<T>
    {
        private DbContext _ctx;
        private List<Column> _combinedQueryColumns;
        private Entity _entityExecutedOn;


        internal SelectQuery(DbContext ctx, Entity entityExecutedOn, List<Column> combinedQueryColumns)
        {
            _ctx = ctx;
            _entityExecutedOn = entityExecutedOn;
            _combinedQueryColumns = combinedQueryColumns;
        }


        public string BuildQuery()
        {

            return $"SELECT {CommaSeparatedColumns()} FROM {BuildFrom()};";
        }

        public IEnumerable<T> Execute()
        {
            Database db = _ctx.Database;
            // we send all the queriedColumns here to decide which to set on the object later
            var result = db.ExecuteQuery<T>(BuildQuery(), _entityExecutedOn, _combinedQueryColumns);
            return result;
        }

        //
        public string BuildFrom()
        {
            var eAlias = _entityExecutedOn.Alias;
            if (_entityExecutedOn.SuperClasses.Count >= 1)
            {
                var superEntity = _ctx.Schema.GetByType(_entityExecutedOn.SuperClasses.First());
                var sEAlias = superEntity.Alias;
                return $"{superEntity.Name} {sEAlias} JOIN {_entityExecutedOn.Name} {eAlias} ON {sEAlias}.{superEntity.PkColumn.Name} = {eAlias}.{_entityExecutedOn.PkColumn.Name}";
            }
            else
            {
                return $"{_entityExecutedOn.Name} {eAlias}";
            }
        }

        private string CommaSeparatedColumns()
        {
            // we have to only query those columns present in the DB
            return _combinedQueryColumns
                // isindb
                .Where(c => c.IsDbColumn)
                .Select(c => $"{(c.Entity.Alias)}.{c.Name}")
                .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1}, {c2}");
        }

    }
}