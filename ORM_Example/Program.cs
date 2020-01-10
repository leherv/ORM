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

            //var persons = dbContext.Persons.Select(new string[] { }).Build().Execute();
            //var persons = dbContext.Persons.Select(null).Build().Execute();
            //var partialPersons = dbContext.Persons.Select(new[] { "name", "firstname" }).Build().Execute();


            //var teachers = dbContext.Teachers.Select(new string[] { }).Build().Execute();


        }
    }
}