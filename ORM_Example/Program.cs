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
//            var schema = dbContext.Schema;
//
//            var ddl = SchemaSqlBuilder.BuildDdl(schema);
//            using (var file = new StreamWriter("test.sql"))
//            {
//                file.WriteLine(ddl);
//            }
            
            var queryBuilder = dbContext.Persons.Select(new [] {"Name", "FirstName"});
        }
    }
}