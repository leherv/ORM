using ORM_Lib.DbSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORM_Lib.Query.Insert
{
    public class InsertStatementBuilder<T>
    {

        private DbContext _ctx;
        private Entity _entityExecutedOn;
        private List<Column> _insertColumns;
        private IEnumerable<T> _pocos;
        private List<Column> _sEInsertColumns;
        private Entity _superEntity;

        public InsertStatementBuilder(DbContext ctx, Type intoType, IEnumerable<T> pocos)
        {
            _ctx = ctx;
            _entityExecutedOn = _ctx.Schema.GetByType(intoType);
            _pocos = pocos;


            if (_entityExecutedOn.SuperClasses.Any())
            {
                _superEntity = _entityExecutedOn.SuperClasses.Select(superCl => _ctx.Schema.GetByType(superCl)).First();
                _sEInsertColumns = _superEntity.Columns.Where(c => c.IsDbColumn && !c.IsPkColumn).ToList();
                _insertColumns = _entityExecutedOn.Columns.Where(c => c.IsDbColumn).ToList();
            }
            else
            {
                _insertColumns = _entityExecutedOn.Columns.Where(c => c.IsDbColumn && !c.IsPkColumn).ToList();
            }
        }

        public InsertStatement<T> Build()
        {
            return new InsertStatement<T>(
                    _ctx,
                    _entityExecutedOn,
                    _superEntity,
                    _insertColumns,
                    _sEInsertColumns,
                    _pocos.ToList()
                );
        }



    }
}
