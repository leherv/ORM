using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ORM_Lib.Query;

namespace ORM_Lib
{
    public abstract class DbSet<T>
    {
        protected DbContext _ctx;

        protected DbSet(DbContext ctx)
        {
            _ctx = ctx;
        }

        public abstract SelectQueryBuilder<T> Select(string[] columns);
        public abstract void Add(T poco);

    }
}