using System;
using System.Collections.Generic;
using System.Linq;
using ORM_Lib.DbSchema;
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

        public override SelectQueryBuilder<T> Select(string[] columns)
        {
            return new SelectQueryBuilder<T>(_ctx, columns, typeof(T));
        }
    }
}



// TODO: für später ShadowObject mit Metainformationen
// Dictionary <Type, Dictionary<PK,(Entity, ShadowEntity)> 

//TODO: SELECT und WHERE , ORDERBY muss nicht sein