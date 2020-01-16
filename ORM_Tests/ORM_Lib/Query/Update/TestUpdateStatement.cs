using NUnit.Framework;
using ORM_Lib.Attributes;
using ORM_Lib.DbSchema;
using ORM_Lib.Query;
using ORM_Lib.Query.Update;
using ORM_Lib.Query.Where;
using ORM_Lib.TypeMapper;
using System.Collections.Generic;
using System.Data;

namespace ORM_Tests.ORM_Lib.Query.Update
{
    class TestUpdateStatement
    {

        private Entity TestEntity { get; set; }
        private List<Column> TestColumns { get; set; }

        private List<UpdateColumnStatement> UpdateStatements { get; set; }

        private List<IWhereFilter> WhereFilters { get; set; }

        [OneTimeSetUp]
        public void Setup()
        {
            TestEntity = new Entity("table_name", new List<Column>(), typeof(TestEntity), null, new List<System.Type>(), 1);
            var col1 = new Column("column1", null, null, null, new OrmDbType("text", PreparedStatementTypeMapper.Map(typeof(string))), false);
            col1.Entity = TestEntity;
            var col2 = new Column("column2", null, null, null, new OrmDbType("text", PreparedStatementTypeMapper.Map(typeof(string))), false);
            col2.Entity = TestEntity;
            TestColumns = new List<Column>() { col1, col2 };

            UpdateStatements = new List<UpdateColumnStatement>() {
                    new UpdateColumnStatement(
                        "column1",
                        new ValueExpression("othervalue", PreparedStatementTypeMapper.Map(typeof(string)), "a")
                    ),
                    new UpdateColumnStatement(
                        "column2",
                        new ValueExpression("newValue", PreparedStatementTypeMapper.Map(typeof(string)), "b")
                    )
                };

            WhereFilters = new List<IWhereFilter>
            {
                BinaryExpression.Eq(
                    new ColumnExpression("column2"),
                    new ValueExpression("test", PreparedStatementTypeMapper.Map(typeof(string)), "test2")
                )
            };
        }


        [Test]
        public void TestSimpleStatement()
        {
            var updateStatement = new UpdateStatement(
                TestEntity,
                new List<IWhereFilter>(),
                new List<UpdateColumnStatement>() {
                    UpdateStatements[0]
                }
            );
            Assert.AreEqual("UPDATE table_name t1 SET column1 = @a;", updateStatement.AsSqlString());
        }

        [Test]
        public void TestWithWhere()
        {
            var updateStatement = new UpdateStatement(
                TestEntity,
                new List<IWhereFilter>() { WhereFilters[0] },
                new List<UpdateColumnStatement>() { UpdateStatements[1] }
            );
            Assert.AreEqual("UPDATE table_name t1 SET column2 = @b WHERE t1.column2 = @test2;", updateStatement.AsSqlString());
        }


        [Test]
        public void TestWithMultipleUpdate()
        {
            var updateStatement = new UpdateStatement(
                TestEntity,
                new List<IWhereFilter>() { WhereFilters[0] },
                UpdateStatements
            );
            Assert.AreEqual("UPDATE table_name t1 SET column1 = @a, column2 = @b WHERE t1.column2 = @test2;", updateStatement.AsSqlString());
        }

    }
}
