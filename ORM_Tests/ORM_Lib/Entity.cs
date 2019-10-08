using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace ORM_Tests.ORM_Lib
{
    public class Entity : IQueryable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Expression Expression { get; }

        public Entity(string firstName, string lastName, Expression expression)
        {
            FirstName = firstName;
            LastName = lastName;
            Expression = expression;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Type ElementType => GetType();
  
        public IQueryProvider Provider { get; }
    }
}