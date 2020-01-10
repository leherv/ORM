using ORM_Lib.DbSchema;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ORM_Lib.Cache
{
    internal class InternalLazyLoader : ILazyLoader
    {
        private DbContext _ctx;
        private Entity _entity;
        private Column _column;

        InternalLazyLoader(DbContext ctx, Entity entity, Column column)
        {
            _ctx = ctx;
            _entity = entity;
            _column = column;
        }


        public ICollection<T> Load<T>(object poco, ref ICollection<T> loadTo)
        {
            //_column.Constraints.
            return null;

        }

        public T Load<T>(object poco, ref T loadTo)
        {
            throw new NotImplementedException();
        }
    }
}
