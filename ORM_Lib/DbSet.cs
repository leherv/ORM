using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ORM_Lib
{
    public abstract class DbSet<T> : IQueryable<T> where T : class
    {
        Type IQueryable.ElementType => throw new NotImplementedException();

        Expression IQueryable.Expression => throw new NotImplementedException();

        IQueryProvider IQueryable.Provider => throw new NotImplementedException();


        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}