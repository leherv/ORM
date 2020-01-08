using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ORM_Lib.Query
{
    public class SelectQueryBuilder<T>
    {
        private string[] _queriedColumns;
        private Type _fromType;
        private DbContext _ctx;
        private List<Column> _combinedQueryColumns;
        private Entity _entityExecutedOn;
        private List<WhereFilter> _whereFilter = new List<WhereFilter>();

        internal SelectQueryBuilder(DbContext ctx, string[] queriedColumns, Type fromType)
        {
            _ctx = ctx;
            _fromType = fromType;
            _entityExecutedOn = _ctx.Schema.GetByType(fromType);
            _queriedColumns = queriedColumns;
            BuildCombinedQueryColumns();
        }

        private void BuildCombinedQueryColumns()
        {
            //TODO: doc if columns is empty all columns will be used!
            var columnsToQuery = (_queriedColumns != null && _queriedColumns.Length > 0) ? _queriedColumns.Select(c => _entityExecutedOn.GetColumnByName(c)).ToList() : _entityExecutedOn.Columns;
            var entity = _ctx.Schema.GetByType(_fromType);
            if (entity.SuperClasses.Any())
            {
                var superEntity = entity.SuperClasses.Select(superCl => _ctx.Schema.GetByType(superCl)).First();
                if (_queriedColumns != null && _queriedColumns.Length <= 0)
                {
                    columnsToQuery.AddRange(superEntity.Columns);
                }
            }
            //_combinedQueryColumns = new HashSet<Column>(columnsToQuery);
            _combinedQueryColumns = columnsToQuery;
        }

        public SelectQueryBuilder<T> Where(WhereFilter where)
        {
            _whereFilter.Add(where);
            return this;
        }
      
        public SelectQuery<T> Build()
        {
            return new SelectQuery<T>(
                _ctx,
                // TODO: doc either a string [] of columns to select or empty array to select all
                _entityExecutedOn,
                _combinedQueryColumns,
                _whereFilter
            ); 


        }
    }
}