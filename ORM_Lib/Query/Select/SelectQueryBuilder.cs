using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ORM_Lib.Query.Select
{
    public class SelectQueryBuilder<T>
    {
        private DbContext _ctx;
        private Entity _entityExecutedOn;
        private List<IWhereFilter> _whereFilter = new List<IWhereFilter>();
        private List<Join> _joins = new List<Join>(); 

        internal SelectQueryBuilder(DbContext ctx, Type fromType)
        {
            _ctx = ctx;
            _entityExecutedOn = _ctx.Schema.GetByType(fromType);
        }

        internal SelectQueryBuilder<T> Join(Join join)
        {
            _joins.Add(join);
            return this;
        }

        public SelectQueryBuilder<T> Where(IWhereFilter where)
        {
            _whereFilter.Add(where);
            return this;
        }

        public SelectQuery<T> Build()
        {
            if (_entityExecutedOn.SuperClasses.Any())
            {
                var superEntity = _entityExecutedOn.SuperClasses.Select(superCl => _ctx.Schema.GetByType(superCl)).First();
                Join(new Join(_entityExecutedOn.Alias, _entityExecutedOn.PkColumn.Name, superEntity.Name, superEntity.Alias, superEntity.PkColumn.Name));
            }
            return new SelectQuery<T>(
                _ctx,
                _entityExecutedOn,
                _entityExecutedOn.CombinedColumns().ToList(),
                _whereFilter,
                _joins
            );
        }
    }
}