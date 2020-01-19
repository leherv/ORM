using System.Collections.Generic;
using ORM_Lib.Query.Insert;
using ORM_Lib.Query.Select;

namespace ORM_Lib
{
    public abstract class DbSet<T>
    {
        protected DbContext _ctx;

        protected DbSet(DbContext ctx)
        {
            _ctx = ctx;
        }

        public abstract SelectQueryBuilder<T> Select();
        public abstract InsertStatementBuilder<T> Add(IEnumerable<T> poco);

        public abstract InsertStatementBuilder<T> Add(T poco);

    }
}