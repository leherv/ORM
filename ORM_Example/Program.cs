using System;
using System.IO;
using System.Linq;
using ORM_Lib;
using ORM_Lib.Ddl;
using ORM_Lib.Query;
using ORM_Lib.Query.Where;

namespace ORM_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new ExampleDbContext();


            // Simple Select //
            //var persons = dbContext.Persons.Select(new string[] { }).Build().Execute();
            //var persons = dbContext.Persons.Select(null).Build().Execute();

            // Partial Select //
            //var partialPersons = dbContext.Persons.Select(new[] { "name", "firstname" }).Build().Execute();


            // Select Where //
            //var filteredPerson = dbContext.Persons
            //    .Select(null)
            //    .Where(BinaryExpression.Eq(new ColumnExpression(), new ValueExpression()));

            // OneToMany + Inheritance //
            //var teachers = dbContext.Teachers.Select(new string[] { }).Build().Execute();
            //var teacher = teachers.First();
            //// now classes are lazy loaded
            //var classes = teacher.Classes;
            //// now lazy loaded students of the lazy loaded classes
            //var classesStudents = classes.First().Students;


            // ManyToOne
            //var classes = dbContext.Classes.Select(null).Build().Execute();
            //var c = classes.FirstOrDefault();
            ////lazy load ManyToOne
            //var teacher = c.Teacher; 


            // ManyToMany + 
            //var courses = dbContext.Courses.Select(null).Build().Execute();



        }
    }
}