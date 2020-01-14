using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using ORM_Lib.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORM_Lib.Query.Update
{
    class UpdateStatementBuilder<T>
    {
        private PocoChange _changes;
        private Entity _entity;
        private List<IWhereFilter> _whereFilter;

        public UpdateStatementBuilder(PocoChange changes, Entity entity, List<IWhereFilter> whereFilter)
        {
            _changes = changes;
            _entity = entity;
            _whereFilter = whereFilter;
        }

        public UpdateStatementBuilder<T> Where(IWhereFilter where)
        {
            _whereFilter.Add(where);
            return this;
        }

        private List<UpdateColumnStatement> BuildUpdateColumns()
        {
            //TODO: maybe use constructor with dbtype defined if problems occur
            return _changes.NewValues.Select(c => new UpdateColumnStatement(c.Key, new ValueExpression(c.Value))).ToList();
        }

        public UpdateStatement<T> Build()
        {
            return new UpdateStatement<T>(
                _entity,
                _whereFilter,
                BuildUpdateColumns()
            );
        }

    }
}
