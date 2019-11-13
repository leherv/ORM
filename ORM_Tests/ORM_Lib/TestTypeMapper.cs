using System;
using System.Linq;
using NUnit.Framework;
using ORM_Lib;

namespace ORM_Tests.ORM_Lib
{
    public class TestTypeMapper
    {
        
        [Test]
        public void TestSByte()
        {
            Assert.AreEqual("smallint", TypeMapper.GetDbType(typeof(sbyte)));
        }
        
        [Test]
        public void TestShort()
        {
            Assert.AreEqual("smallint", TypeMapper.GetDbType(typeof(short)));
        }
        
        [Test]
        public void TestInt()
        {
            Assert.AreEqual("integer", TypeMapper.GetDbType(typeof(int)));            
        }

        
        [Test]
        public void TestLong()
        {
            Assert.AreEqual("bigint", TypeMapper.GetDbType(typeof(long)));            
        }

        
        [Test]
        public void TestByte()
        {
            Assert.AreEqual("smallint", TypeMapper.GetDbType(typeof(byte)));
        }
        
        [Test]
        public void TestByteArray()
        {
            Assert.AreEqual("bytea", TypeMapper.GetDbType(typeof(byte[])));
        }
        
        [Test]
        public void TestUShort()
        {
            Assert.AreEqual("integer", TypeMapper.GetDbType(typeof(ushort)));
        }
        
        [Test]
        public void TestUInt()
        {
            Assert.AreEqual("oid", TypeMapper.GetDbType(typeof(uint)));
        }
        
        [Test]
        public void TestFloat()
        {
            Assert.AreEqual("real", TypeMapper.GetDbType(typeof(float)));
        }
        
        [Test]
        public void TestDouble()
        {
            Assert.AreEqual("double precision", TypeMapper.GetDbType(typeof(double)));
        }
        
        [Test]
        public void TestDecimal()
        {
            Assert.AreEqual("numeric", TypeMapper.GetDbType(typeof(decimal)));
        }
        
        [Test]
        public void TestBool()
        {
            Assert.AreEqual("boolean", TypeMapper.GetDbType(typeof(bool)));
        }
        
        [Test]
        public void TestString()
        {
            Assert.AreEqual("text", TypeMapper.GetDbType(typeof(string)));
        }
        
        [Test]
        public void TestChar()
        {
            Assert.AreEqual("text", TypeMapper.GetDbType(typeof(char)));
        }
        
        [Test]
        public void TestCharA()
        {
            Assert.AreEqual("text", TypeMapper.GetDbType(typeof(char[])));
        }
        
        [Test]
        public void TestDatetime()
        {
            Assert.AreEqual("timestamp", TypeMapper.GetDbType(typeof(DateTime)));
        }

        [Test]
        public void TestNonExistent()
        {
            var e = new Entity();
            Assert.AreEqual(null, TypeMapper.GetDbType(e.GetType()));
        }

        [Test]
        public void TestVoid()
        {
            Assert.AreEqual(null, TypeMapper.GetDbType(typeof(void)));            
        }

    }
}