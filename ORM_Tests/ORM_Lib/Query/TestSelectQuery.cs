using System.Collections.Generic;
using NUnit.Framework;
using ORM_Lib;
using ORM_Lib.DbSchema;
using ORM_Lib.Query;

namespace ORM_Tests.ORM_Lib.Query
{
    public class TestSelectQuery
    {
        [Test]
        public void TestSelect()
        {
            var entity = new Entity("table_name", new List<Column>(), typeof(TestEntity), null);
            var selectQuery = new SelectQuery<TestEntity>(
                null,
                new List<Column>() 
                {
                    new Column("column1", null, null, null, true, false),
                    new Column("column2", null, null, null, true, false)
                },
                entity);
            Assert.AreEqual("SELECT column1, column2 FROM table_name;", selectQuery.BuildQuery());
        }
    }
}