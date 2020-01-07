using ORM_Lib.DbSchema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ORM_Lib.Query
{
    public class SelectQueryBuilder<T>
    {
        private Type _fromType;
        private DbContext _ctx;
        private string[] _queriedColumns;
        private List<(Entity, List<(Column, string)>)> _entityColumnAlias;
        private Entity _entityExecutedOn;

        internal SelectQueryBuilder(DbContext ctx, string[] queriedColumns, Type fromType)
        {
            _ctx = ctx;
            _fromType = fromType;
            _entityExecutedOn = _ctx.Schema.GetByType(fromType);
            _queriedColumns = queriedColumns;
            BuildColumnAliasMap();
        }

        private void BuildColumnAliasMap()
        {
            //TODO: doc if columns is empty all columns will be used!
            var columnsToQuery = _queriedColumns.Length > 0 ? _queriedColumns.Select(c => _entityExecutedOn.GetColumnByName(c)).ToList() : _entityExecutedOn.Columns;

            _entityColumnAlias = new List<(Entity, List<(Column, string)>)>();
            var entity = _ctx.Schema.GetByType(_fromType);
            if (entity.SuperClasses.Any())
            {
                var superEntity = entity.SuperClasses.Select(superCl => _ctx.Schema.GetByType(superCl)).First();
                if (_queriedColumns.Length <= 0)
                {
                    columnsToQuery.AddRange(superEntity.Columns);
                }
            }
        }

        public SelectQuery<T> Build()
        {
            return new SelectQuery<T>(
                _ctx,
                // TODO: doc either a string [] of columns to select or empty array to select all
                _entityExecutedOn,
                _entityColumnAlias
            ); 


        }

        private string Alias(Entity entity, int i)
        {
            return $"{entity.Name.First()}{i}";
        }
    }
}