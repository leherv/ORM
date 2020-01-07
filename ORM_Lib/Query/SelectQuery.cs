using System.Collections.Generic;
using System.Linq;
using ORM_Lib.DbSchema;
using ORM_Lib.Deserialization;

namespace ORM_Lib.Query
{
    // class is public because user has to be able to see it and execute - Execute | but he is not allowed to create an instance himself constructor internal
    public class SelectQuery<T>
    {
        private DbContext _ctx;
        private List<(Entity, List<(Column, string)>)> _entityColumnAlias;
        private Entity _entityExecutedOn;

        internal SelectQuery(DbContext ctx, Entity entityExecutedOn, List<(Entity, List<(Column, string)>)> entityColumnAlias)
        {
            _ctx = ctx;
            _entityExecutedOn = entityExecutedOn;
            _entityColumnAlias = entityColumnAlias;
            _ctx = ctx;
        }


        public string BuildQuery()
        {

            return $"SELECT {CommaSeparatedColumns()} FROM {BuildFrom()};";
        }

        public IEnumerable<T> Execute()
        {
            Database db = _ctx.Database;
            // we send all the queriedColumns here to decide which to set on the object later
            var result = db.ExecuteQuery<T>(BuildQuery(), _entityExecutedOn, QueriedColumns());
            return result;
        }

        //
        public string BuildFrom()
        {
            if (_entityColumnAlias.Count > 1)
            {
                var superEntity = _entityColumnAlias.ElementAt(1).Item1;
                var superEntityAlias = _entityColumnAlias.ElementAt(1).Item2.First().Item2;
                var entityAlias = _entityColumnAlias.First().Item2.First().Item2;
                return $"{superEntity.Name} {superEntityAlias} JOIN {_entityExecutedOn.Name} {entityAlias} ON {superEntityAlias}.{superEntity.PkColumn.Name} = {entityAlias}.{_entityExecutedOn.PkColumn.Name}";
            }
            else
            {
                return $"{_entityExecutedOn.Name}";
            }
        }

        private string CommaSeparatedColumns()
        {

            var entityColumns = _entityColumnAlias.First().Item2;
            if (_entityColumnAlias.Count > 1)
            {
                entityColumns.AddRange(_entityColumnAlias.ElementAt(2).Item2);
            }
            return entityColumns
                .Where(colAlias => !colAlias.Item1.IsShadowAttribute)
                // we have to only query columns present in the DB
                // we have the reference to the column VIK just use it here and leave out all the fancy stuff
                .Select(colAlias => $"{colAlias.Item2}.{colAlias.Item1.Name}")
                .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1}, {c2}");
        }

        private List<Column> QueriedColumns()
        {
            var entityColumns = _entityColumnAlias.First().Item2.Select(columAlias => columAlias.Item1).ToList();
            if (_entityColumnAlias.Count > 1)
            {
                entityColumns.AddRange(_entityColumnAlias.ElementAt(1).Item2.Select(columAlias => columAlias.Item1));
            }
            return entityColumns;
        }
    }
}