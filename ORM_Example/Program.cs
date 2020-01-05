using System;
using System.IO;
using System.Linq;
using ORM_Lib;
using ORM_Lib.Ddl;
using ORM_Lib.Query;

namespace ORM_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new ExampleDbContext();

            var queryBuilder = dbContext.Persons.Select(new[] { "Name", "FirstName" }).Build().Execute();
        }
    }
}