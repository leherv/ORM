using NUnit.Framework;
using ORM_Lib.Query.Insert;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Tests.ORM_Lib.Query.Insert
{
    class TestManyToManyInsertStatement
    {
        [Test]
        public void TestSimple()
        {
            var statement = new ManyToManyInsertStatement(
                "test_table",
                ("col1", "col2"),
                new HashSet<(object, object)>() { (1L, 1L), (2L, 2L) }
            );

            Assert.AreEqual("INSERT INTO test_table (col1, col2) VALUES (@col10, @col20), (@col11, @col21);", statement.AsSqlString());
        }
    }
}
