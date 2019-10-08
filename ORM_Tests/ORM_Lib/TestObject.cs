using System;
using System.Dynamic;

namespace ORM_Tests.ORM_Lib
{
    public class TestObject
    {
        private string _test = "private";
        public string test2 = "public";
        public string Test3 { get; set; }


        public void PrintTest()
        {
            Console.WriteLine(_test);
        }
    }
}