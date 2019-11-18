using System;
using System.IO;
using System.Linq;
using ORM_Lib;

namespace ORM_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new ExampleDbContext();
            var schema = dbContext.Schema;

            var ddl = SchemaSqlBuilder.BuildDdl(schema);
            using (var file = new StreamWriter("test.sql"))
            {
                file.WriteLine(ddl);
            }
           
        }
    }
}