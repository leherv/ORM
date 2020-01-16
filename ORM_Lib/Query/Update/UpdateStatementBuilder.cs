using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using ORM_Lib.Saving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORM_Lib.Query.Update
{
    internal class UpdateStatementBuilder
    {
        private DbContext _ctx;
        private PocoChange _changes;
        private Entity _entity;

        //TODO: wherefilter could be a problem passing to superclass
        public UpdateStatementBuilder(DbContext ctx, PocoChange changes)
        {
            _ctx = ctx;
            _changes = changes;
            _entity = changes.EntityChanged;
        }

        private List<UpdateColumnStatement> BuildUpdateColumns(IEnumerable<KeyValuePair<string, object>> changes)
        {
            //TODO: maybe use constructor with dbtype defined if problems occur
            return changes.Select(c => new UpdateColumnStatement(c.Key, new ValueExpression(c.Value))).ToList();
        }

        public List<UpdateStatement> Build()
        {
            var result = new List<UpdateStatement>();

            if (_entity.SuperClasses.Any())
            {
                var superEntity = _entity.SuperClasses.Select(superCl => _ctx.Schema.GetByType(superCl)).First();
                var superEntityChanges = _changes.NewValues
                    .Where(pair => superEntity.GetColumnByName(pair.Key) != null);
                result.Add(new UpdateStatement(
                    superEntity,
                    new List<IWhereFilter>() { BuildWhere(superEntity.PkColumn.Name, _changes.Pk) },
                    BuildUpdateColumns(superEntityChanges)
                ));
            }

            var entityChanges = _changes.NewValues
                  .Where(pair => _entity.GetColumnByName(pair.Key) != null);
            var where = BinaryExpression.Eq(
                   new ColumnExpression(_entity.PkColumn.Name),
                   new ValueExpression(_changes.Pk)
               );
            result.Add(new UpdateStatement(
                    _entity,
                    new List<IWhereFilter>() { BuildWhere(_entity.PkColumn.Name, _changes.Pk) },
                    BuildUpdateColumns(entityChanges)
            ));
            return result;
        }


        private IWhereFilter BuildWhere(string columnName, object value)
        {
            return BinaryExpression.Eq(
                    new ColumnExpression(columnName),
                    new ValueExpression(value));
        }
    }
}
