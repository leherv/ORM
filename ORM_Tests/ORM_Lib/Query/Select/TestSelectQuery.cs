using System.Collections.Generic;
using NUnit.Framework;
using ORM_Lib;
using ORM_Lib.DbSchema;
using ORM_Lib.Query;
using ORM_Lib.Query.Select;
using ORM_Lib.Query.Where;
using ORM_Lib.TypeMapper;

namespace ORM_Tests.ORM_Lib.Query
{
    public class TestSelectQuery
    {
        private Entity TestEntity { get; set; }
        private List<Column> TestColumns { get; set; }

        [OneTimeSetUp]
        public void Setup()
        {
            TestEntity = new Entity("table_name", new List<Column>(), typeof(TestEntity), null, new List<System.Type>(), 1);
            var col1 = new Column("column1", null, null, null, new OrmDbType("text", PreparedStatementTypeMapper.Map(typeof(string))), false);
            col1.Entity = TestEntity;
            var col2 = new Column("column2", null, null, null, new OrmDbType("text", PreparedStatementTypeMapper.Map(typeof(string))), false);
            col2.Entity = TestEntity;
            TestColumns = new List<Column>() { col1, col2 };
        }


        [Test]
        public void TestSelect()
        {
            var selectQuery = new SelectQuery<TestEntity>(
                null,
                TestEntity,
                TestColumns,
                new List<IWhereFilter>(),
                new List<Join>());
            Assert.AreEqual("SELECT t1.column1, t1.column2 FROM table_name t1;", selectQuery.AsSqlString());
        }


        [Test]
        public void TestSelectWhere()
        {
            var where = BinaryExpression.GT(
                new ColumnExpression("column1"),
                new ValueExpression(5, PreparedStatementTypeMapper.Map(typeof(int)), "test")
            );
         
            var selectQuery = new SelectQuery<TestEntity>(
                null,
                TestEntity,
                TestColumns,
                new List<IWhereFilter>() { where },
                new List<Join>());
            Assert.AreEqual("SELECT t1.column1, t1.column2 FROM table_name t1 WHERE t1.column1 > @test;", selectQuery.AsSqlString());
        }
        


        [Test]
        public void TestSelectMultipleWhere()
        {
            var where = BinaryExpression.GT(
                new ColumnExpression("column1"),
                new ValueExpression(5, PreparedStatementTypeMapper.Map(typeof(int)), "test")
            );
            var where2 = BinaryExpression.Eq(
                new ColumnExpression("column2"),
                new ValueExpression("hallo", PreparedStatementTypeMapper.Map(typeof(string)), "test2")
            );
            var selectQuery = new SelectQuery<TestEntity>(
                null,
                TestEntity,
                TestColumns,
                new List<IWhereFilter>() { where, where2 },
                new List<Join>());
            Assert.AreEqual("SELECT t1.column1, t1.column2 FROM table_name t1 WHERE t1.column1 > @test AND t1.column2 = @test2;", selectQuery.AsSqlString());
        }
    }
}