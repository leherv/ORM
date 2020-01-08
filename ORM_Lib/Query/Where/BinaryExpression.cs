using ORM_Lib.Query.Where;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Lib.Query
{
    class BinaryExpression : ISqlExpression
    {

        private ISqlExpression _left;
        private ISqlExpression _right;
        private string _operatorSymbol;

        public BinaryExpression(ISqlExpression left, ISqlExpression right, string operatorSymbol)
        {
            _left = left;
            _right = right;
            _operatorSymbol = operatorSymbol;
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

        public string AsSqlString()
        {
            return $"{_left.AsSqlString()} {_operatorSymbol} {_right.AsSqlString()}";
        }
    }
}
