using NUnit.Framework;
using ORM_Lib.DbSchema;
using ORM_Lib.Query;
using ORM_Lib.Query.Insert;
using ORM_Lib.Query.Where;
using ORM_Lib.TypeMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Tests.ORM_Lib.Query.Insert
{
    class TestInsertStatement
    {
        private Entity TestEntity { get; set; }
        private List<Column> TestColumns { get; set; }
        private List<WithStatement> WithStatements { get; set; }

        [OneTimeSetUp]
        public void Setup()
        {
            TestEntity = new Entity("table_name", new List<Column>(), typeof(TestEntity), null, new List<Type>(), 1);
            var col1 = new Column("column1", null, null, null, new OrmDbType("text", PreparedStatementTypeMapper.Map(typeof(string))), false);
            col1.Entity = TestEntity;
            var col2 = new Column("column2", null, null, null, new OrmDbType("text", PreparedStatementTypeMapper.Map(typeof(string))), false);
            col2.Entity = TestEntity;
            TestColumns = new List<Column>() { col1, col2 };

            WithStatements = new List<WithStatement>() {
                    new WithStatement(
                        "column1",
                        new ValueExpression("othervalue", PreparedStatementTypeMapper.Map(typeof(string)), "a")
                    ),
                };
        }
    }
}
