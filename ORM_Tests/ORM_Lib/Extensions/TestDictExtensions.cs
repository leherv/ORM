using NUnit.Framework;
using ORM_Lib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_Tests.ORM_Lib.Extensions
{
    class TestDictExtensions
    {
        [Test]
        public void TestSimple()
        {
            var dict = new Dictionary<string, string>();
            var value = dict.GetOrInsert("test", "value");
            Assert.AreEqual("value", value);
            Assert.IsTrue(dict.TryGetValue("test", out var res));
            Assert.AreEqual(res, "value");
        }

        [Test]
        public void TestNoOverride()
        {
            var dict = new Dictionary<string, string>();
            dict["test"] = "value";
            var value = dict.GetOrInsert("test", "othervalue");
            Assert.AreEqual("value", value);
            Assert.AreEqual(1, dict.Count);
        }
    }
}
