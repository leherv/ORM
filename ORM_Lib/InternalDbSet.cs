using System.Collections.Generic;
using ORM_Lib.Query.Insert;
using ORM_Lib.Query.Select;

namespace ORM_Lib
{
    internal class InternalDbSet<T> : DbSet<T>
    {
        internal InternalDbSet(DbContext ctx) : base(ctx)
        {
        }

        public override InsertStatementBuilder<T> Add(IEnumerable<T> poco)
        {
            return new InsertStatementBuilder<T>(_ctx, typeof(T), poco);
        }

        public override InsertStatementBuilder<T> Add(T poco)
        {
            return Add(new[] { poco });
        }

        public override SelectQueryBuilder<T> Select()
        {
            return new SelectQueryBuilder<T>(_ctx, typeof(T));
        }
    }
}