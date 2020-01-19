using ORM_Lib.DbSchema;
using ORM_Lib.Query.Where;
using System.Collections.Generic;
using System.Linq;

namespace ORM_Lib.Query
{
    public class BinaryExpression : IWhereFilter
    {

        private ISqlExpression _left;
        private ISqlExpression _right;
        private string _operatorSymbol;
        private Entity _entity;

        public BinaryExpression(ISqlExpression left, ISqlExpression right, string operatorSymbol)
        {
            _left = left;
            _right = right;
            _operatorSymbol = operatorSymbol;
        }

        public string AsSqlString()
        {
            return $"{_left.AsSqlString()} {_operatorSymbol} {_right.AsSqlString()}";
        }

        public IEnumerable<NamedParameter> GetNamedParams()
        {
            return _left.GetNamedParams().Concat(_right.GetNamedParams());
        }

        public static BinaryExpression Eq(ISqlExpression left, ISqlExpression right)
        {
            return new BinaryExpression(left, right, "=");
        }

        public static BinaryExpression NotEq(ISqlExpression left, ISqlExpression right)
        {
            return new BinaryExpression(left, right, "<>");
        }

        public static BinaryExpression LT(ISqlExpression left, ISqlExpression right)
        {
            return new BinaryExpression(left, right, "<");
        }

        public static BinaryExpression GT(ISqlExpression left, ISqlExpression right)
        {
            return new BinaryExpression(left, right, ">");
        }

        public static BinaryExpression LTEq(ISqlExpression left, ISqlExpression right)
        {
            return new BinaryExpression(left, right, "<=");
        }

        public static BinaryExpression GTEq(ISqlExpression left, ISqlExpression right)
        {
            return new BinaryExpression(left, right, ">=");
        }

        public static BinaryExpression And(ISqlExpression left, ISqlExpression right)
        {
            return new BinaryExpression(left, right, "AND");
        }
        public static BinaryExpression Or(ISqlExpression left, ISqlExpression right)
        {
            return new BinaryExpression(left, right, "OR");
        }

        void ISqlExpression.SetContextInformation(Entity entity)
        {
            _entity = entity;
            _left.SetContextInformation(entity);
            _right.SetContextInformation(entity);
        }
    }
}
