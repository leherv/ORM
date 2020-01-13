using System;
using System.Collections.Generic;
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


            //Adding(leave out and use testData.sql for already inserted tests)
            //dbContext.Teachers
            //    .Add(new[] {
            //        new Teacher("test_name", "test_firstname", Gender.MALE, new DateTime(2000,1,1), 2500),
            //        new Teacher("test_name2", "test_firstname2", Gender.FEMALE, new DateTime(2002,2,2), 2600)    
            //    })
            //    .Build()
            //    .Execute();

            //dbContext.Persons
            //    .Add(new[] {
            //        new Person("test_name2", "test_firstname2", Gender.FEMALE, new DateTime(2002,2,2)),
            //        new Person("test_name3", "test_firstname3", Gender.MALE, new DateTime(2003,3,3))
            //    })
            //    .Build()
            //    .Execute();


            // Simple Select //
            //var persons = dbContext.Persons.Select(new string[] { }).Build().Execute();
            //var persons = dbContext.Persons.Select(null).Build().Execute();

            // Partial Select //
            //var partialPersons = dbContext.Persons.Select(new[] { "name", "firstname" }).Build().Execute();


            // Select Where //
            //var filteredPerson = dbContext.Persons
            //    .Select(null)
            //    .Where(BinaryExpression.Eq(new ColumnExpression("firstname"), new ValueExpression("test_firstname")))
            //    .Build()
            //    .Execute();

            //var filteredPerson = dbContext.Persons
            //    .Select(null)
            //    .Where(BinaryExpression.Eq(new ValueExpression(1), new ValueExpression(1)))
            //    .Build()
            //    .Execute();

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



            // Important Mentions //
            //var persons = dbContext.Persons.Select(null).Build().Execute();
            //// firstName from database = "john"
            //var person = persons.First();
            //person.FirstName = "Tom";

            //var persons2 = dbContext.Persons.Select(null).Build().Execute();
            //// firstName = "john" as the change was not saved!
            //var firstName = person.FirstName;

        }
    }
}