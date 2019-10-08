using System;
using System.Linq;
using ORM;
using ORM_Lib;
using ORM_Tests;

namespace ORM_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new ExampleDbContext();
            var dbSet = dbContext.Persons;
            var iq = dbSet.Where(p => p.Name.Equals("Frank"));
            var iqs = iq.ToList();
        }
    }
}