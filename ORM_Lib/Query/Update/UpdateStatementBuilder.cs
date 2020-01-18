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
        private PocoUpdateChange _changes;
        private Entity _entity;

        //TODO: wherefilter could be a problem passing to superclass
        public UpdateStatementBuilder(DbContext ctx, PocoUpdateChange changes)
        {
            _ctx = ctx;
            _changes = changes;
            _entity = changes.EntityChanged;
        }



        public List<UpdateStatement> Build()
        {
            var result = new List<UpdateStatement>();

            if (_entity.SuperClasses.Any())
            {
                var superEntity = _entity.SuperClasses.Select(superCl => _ctx.Schema.GetByType(superCl)).First();
                var updateColumnStatementsS = BuildUpdateColumns(superEntity);
                if (updateColumnStatementsS != null && updateColumnStatementsS.Count > 0)
                    result.Add(new UpdateStatement(
                        superEntity,
                        new List<IWhereFilter>() { BuildWhere(superEntity.PkColumn.Name, _changes.Pk) },
                        updateColumnStatementsS
                    ));
            }

            var updateColumnStatements = BuildUpdateColumns(_entity);
            if (updateColumnStatements != null && updateColumnStatements.Count > 0)
            {
                result.Add(new UpdateStatement(
                    _entity,
                    new List<IWhereFilter>() { BuildWhere(_entity.PkColumn.Name, _changes.Pk) },
                    updateColumnStatements
                ));
            }
            return result;
        }


        private IWhereFilter BuildWhere(string columnName, object value)
        {
            return BinaryExpression.Eq(
                    new ColumnExpression(columnName),
                    new ValueExpression(value));
        }

        private List<UpdateColumnStatement> BuildUpdateColumns(Entity entity)
        {
            var entityChanges = _changes.NewValues
                   .Where(pair => entity.GetColumnByName(pair.Key) != null);
            return entityChanges
                .Select(c => new UpdateColumnStatement(
                    c.Key,
                    new ValueExpression(
                        c.Value.GetType().IsEnum ? c.Value.ToString() : c.Value
                    )))
                .ToList();
        }
    }
}
