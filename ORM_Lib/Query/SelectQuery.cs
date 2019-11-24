using System.Collections.Generic;
using System.Linq;
using ORM_Lib.DbSchema;

namespace ORM_Lib.Query
{
    // class is public because user has to be able to see it and execute - Execute | but he is not allowed to create an instance himself constructor internal
    public class SelectQuery<T>
    {
        private List<Column> _columns;
        private Entity _entity;
        private DbContext _ctx;

        internal SelectQuery(DbContext ctx, List<Column> columns, Entity entity)
        {
            _columns = columns;
            _entity = entity;
            _ctx = ctx;
        }

        public string BuildQuery()
        {
            return $"SELECT {CommaSeparatedColumns()} FROM {_entity.Name};";
        }

        public IEnumerable<T> Execute()
        {
            var dbConnection = _ctx.DbConnection;
            var cmd = dbConnection.CreateCommand();
            cmd.CommandText = BuildQuery();
            var reader = cmd.ExecuteReader();
            return new List<T>();
        }

        private string CommaSeparatedColumns()
        {
            return _columns
                .Select(c => c.Name)
                .Aggregate("", (c1, c2) => c1 == "" ? $"{c2}" : $"{c1}, {c2}");
        }
    }
}