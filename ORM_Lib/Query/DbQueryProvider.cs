using System;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ORM_Lib.Query;

namespace ORM_Lib
{
    internal class DbQueryProvider: IQueryProvider
    {
        private DbConnection _connection;

        public DbQueryProvider(DbConnection connection)
        {
            _connection = connection;
        }

        public string GetQueryText(Expression expression)
        {
            return Translate(expression);
        }
        
        private static string Translate(Expression expression)
        {
            return new QueryTranslator().Translate(expression);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            throw new System.NotImplementedException();
        }

        public object Execute(Expression expression)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = Translate(expression);
            var reader = cmd.ExecuteReader();
            // caution here Type elementType = TypeSystem.GetElementType(expression.Type);
            var elementType = expression.GetType();
            return Activator.CreateInstance(
                typeof(ObjectReader<>).MakeGenericType(elementType),
                BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] {reader},
                null);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = Translate(expression);
            var reader = cmd.ExecuteReader();
            // caution here Type elementType = TypeSystem.GetElementType(expression.Type);
            var elementType = expression.GetType();
            return (TResult) Activator.CreateInstance(
                typeof(ObjectReader<>).MakeGenericType(elementType),
                BindingFlags.Instance | BindingFlags.NonPublic, null,
                new object[] {reader},
                null);
        }
    }
}