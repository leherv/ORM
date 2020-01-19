using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using System.Collections.Generic;

namespace ORM_Lib.Query
{
    class WithStatement : ISqlExpression
    {

        public string Alias { get; }
        private ISqlExpression _inner;

        public WithStatement(string alias, ISqlExpression inner)
        {
            Alias = alias;
            _inner = inner;
        }

        public WithStatement(ISqlExpression inner)
        : this(RandomStringGenerator.RandomString(4), inner)
        {

        }

        public string AsSqlString()
        {
            return $"WITH {Alias} AS ({_inner.AsSqlString()})";
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            return _inner.GetNamedParams();
        }


        void ISqlExpression.SetContextInformation(Entity entity)
        {

        }
    }
}
