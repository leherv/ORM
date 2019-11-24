using System;
using System.Linq;

namespace ORM_Lib.Query
{
    public class SelectQueryBuilder<T>
    {
        private Type _fromType;
        private DbContext _ctx;
        private string[] _columns;

        internal SelectQueryBuilder(DbContext ctx, string[] columns, Type fromType)
        {
            _ctx = ctx;
            _columns = columns;
            _fromType = fromType;
        }

        public SelectQuery<T> Build()
        {
            var entity = _ctx.Schema.GetByType(_fromType);
            return new SelectQuery<T>(
                _ctx,
                _columns.Select(c => entity.GetColumnByName(c)).ToList(),
                entity
            );
        }
    }
}