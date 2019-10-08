using System.Reflection;
using NUnit.Framework;

namespace ORM_Tests.ORM_Lib
{
    public class Test
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            var type = typeof(TestObject);
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var properties = type.GetProperties();
        }
    }
}