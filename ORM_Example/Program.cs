﻿using System;
using System.Linq;

namespace ORM_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new ExampleDbContext();

            ShowCase1(dbContext);
          


        

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
            //var course = courses.First();
            //var students = course.Students;



            // Important Mentions //
            //var persons = dbContext.Persons.Select(null).Build().Execute();
            //// firstName from database = "john"
            //var person = persons.First();
            //person.FirstName = "Tom";

            //var persons2 = dbContext.Persons.Select(null).Build().Execute();
            //// firstName = "john" as the change was not saved!
            //var firstName = person.FirstName;

        }


        private static void ShowCase1(ExampleDbContext dbContext)
        {
            var teachers = dbContext.Teachers
                .Add(new[] {
                new Teacher("test_name", "test_firstname", Gender.MALE, new DateTime(2000,1,1), 2500),
                new Teacher("test_name2", "test_firstname2", Gender.FEMALE, new DateTime(2002,2,2), 2600)
                })
                .Build()
                .Execute();

            var cl = dbContext.Classes
                .Add(new Class("1A"))
                .Build()
                .Execute();

            var student = dbContext.Students
                .Add(new Student("test_name3", "test_firstname3", Gender.MALE, new DateTime(2003, 3, 3)))
                .Build()
                .Execute()
                .First();

            var course = dbContext.Courses
                .Add(new Course(true, "best course"))
                .Build()
                .Execute()
                .First();

            dbContext.SaveChanges();

            var teacher = teachers.First();

            course.Teacher = teacher;
            //teacher.Courses.Add(course); // equivalent result 

            student.Courses.Add(course);
            //course.Students.Add(student); // equivalent result

            dbContext.SaveChanges();
        }

    }
}