using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using ORM_Lib;

namespace ORM_Tests.ORM_Lib
{
    public class TestQueryTranslator
    {

        
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Test1()
        {
            var queryTranslator = new QueryTranslator();
            var entity = new Entity("John", "Wick", Expression.Default(typeof(Entity)));
            var s = queryTranslator.Translate(entity.Expression);
            Console.WriteLine(s);
            Assert.Pass();
        }
        
        
    }
}