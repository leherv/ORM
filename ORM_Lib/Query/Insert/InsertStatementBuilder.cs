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
        private WithStatement _with;

        public InsertStatementBuilder(DbContext ctx, Type intoType, IEnumerable<T> pocos)
        {
            _ctx = ctx;
            _entityExecutedOn = _ctx.Schema.GetByType(intoType);
            _pocos = pocos;
            if (_entityExecutedOn.SuperClasses.Any())
            {
                var superEntity = _entityExecutedOn.SuperClasses.Select(superCl => _ctx.Schema.GetByType(superCl)).First();
                var sEInsertColumns = superEntity.Columns.Where(c => c.IsDbColumn && !c.IsPkColumn).ToList();
                _insertColumns = _entityExecutedOn.Columns.Where(c => c.IsDbColumn).ToList();
                _with = new WithStatement(new InsertStatement<T>(_ctx, superEntity, sEInsertColumns, null, _pocos.ToList()));
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
                    _insertColumns,
                    _with,
                    _pocos.ToList()
                );
        }



    }
}
