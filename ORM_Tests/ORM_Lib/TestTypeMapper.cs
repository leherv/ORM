using System;
using System.Linq;
using NUnit.Framework;
using ORM_Lib;
using ORM_Lib.TypeMapper;

namespace ORM_Tests.ORM_Lib
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
        public void TestSByte()
        {
            Assert.AreEqual("smallint", _typeMapper.GetDbType(typeof(sbyte)));
        }
        
        [Test]
        public void TestShort()
        {
            Assert.AreEqual("smallint", _typeMapper.GetDbType(typeof(short)));
        }
        
        [Test]
        public void TestInt()
        {
            Assert.AreEqual("integer", _typeMapper.GetDbType(typeof(int)));            
        }

        
        [Test]
        public void TestLong()
        {
            Assert.AreEqual("bigint", _typeMapper.GetDbType(typeof(long)));            
        }

        
        [Test]
        public void TestByte()
        {
            Assert.AreEqual("smallint", _typeMapper.GetDbType(typeof(byte)));
        }
        
        [Test]
        public void TestByteArray()
        {
            Assert.AreEqual("bytea", _typeMapper.GetDbType(typeof(byte[])));
        }
        
        [Test]
        public void TestUShort()
        {
            Assert.AreEqual("integer", _typeMapper.GetDbType(typeof(ushort)));
        }
        
        [Test]
        public void TestUInt()
        {
            Assert.AreEqual("oid", _typeMapper.GetDbType(typeof(uint)));
        }
        
        [Test]
        public void TestFloat()
        {
            Assert.AreEqual("real", _typeMapper.GetDbType(typeof(float)));
        }
        
        [Test]
        public void TestDouble()
        {
            Assert.AreEqual("double precision", _typeMapper.GetDbType(typeof(double)));
        }
        
        [Test]
        public void TestDecimal()
        {
            Assert.AreEqual("numeric", _typeMapper.GetDbType(typeof(decimal)));
        }
        
        [Test]
        public void TestBool()
        {
            Assert.AreEqual("boolean", _typeMapper.GetDbType(typeof(bool)));
        }
        
        [Test]
        public void TestString()
        {
            Assert.AreEqual("text", _typeMapper.GetDbType(typeof(string)));
        }
        
        [Test]
        public void TestChar()
        {
            Assert.AreEqual("text", _typeMapper.GetDbType(typeof(char)));
        }
        
        [Test]
        public void TestCharA()
        {
            Assert.AreEqual("text", _typeMapper.GetDbType(typeof(char[])));
        }
        
        [Test]
        public void TestDatetime()
        {
            Assert.AreEqual("timestamp", _typeMapper.GetDbType(typeof(DateTime)));
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