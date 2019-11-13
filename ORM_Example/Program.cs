using System;
using System.Linq;
using ORM_Lib;
using ORM_Tests;

namespace ORM_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new ExampleDbContext();
            var schema = dbContext.Schema;
        }
    }
}