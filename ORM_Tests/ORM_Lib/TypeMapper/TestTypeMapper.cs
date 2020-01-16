using System;
using NUnit.Framework;
using ORM_Lib.TypeMapper;

namespace ORM_Tests.ORM_Lib.TypeMapper
{
    public class TestPostgresTypeMapper
    {

        private ITypeMapper _typeMapper;

        [SetUp]
        public void Setup()
        {
            _typeMapper = new PostgresTypeMapper();
        }

        [Test]
        public void TestNonExistent()
        {
            var e = new TestEntity();
            Assert.AreEqual(null, _typeMapper.GetDbType(e.GetType()));
        }

        [Test]
        public void TestVoid()
        {
            Assert.AreEqual(null, _typeMapper.GetDbType(typeof(void)));
        }

        [Test]
        public void TestEnum()
        {
            // enums have to be handled specifically in the application (they are always text but it is handled in the ColumnBuilder)
            Assert.AreEqual(null, _typeMapper.GetDbType(typeof(Gender)));
        }
    }
}