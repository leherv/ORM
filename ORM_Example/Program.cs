using ORM_Lib.Query;
using ORM_Lib.Query.Where;
using System;
using System.Linq;

namespace ORM_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new ExampleDbContext();
            ShowCase_DDL();
            ShowCase_Insert(dbContext);
            ShowCase_Query_Simple(dbContext);
            ShowCase_Query_Lazy(dbContext);
            ShowCase_Change_Multiple(dbContext);
        }

        private static void ShowCase_DDL()
        {
            // ddl creation is controlled via flag in ORMConfiguration 
            // just a reminder here
        }


        private static void ShowCase_Insert(ExampleDbContext dbContext)
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
                .Execute()
                .First();

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

            //teacher.Classes.Add(cl);
            cl.Teacher = teacher; // equivalent result 

            //course.Teacher = teacher;
            teacher.Courses.Add(course); // equivalent result 

            //student.Class = cl;
            cl.Students.Add(student); // equivalent result 

            //student.Courses.Add(course);
            course.Students.Add(student); // equivalent result

            dbContext.SaveChanges();
        }

        private static void ShowCase_Query_Simple(ExampleDbContext dbContext)
        {

            // Simple Select //
            var persons = dbContext.Persons.Select().Build().Execute();

            var students = dbContext.Students
                .Select()
                .Build()
                .Execute();

            var classes = dbContext.Classes
                .Select()
                .Build()
                .Execute();

            var teachers = dbContext.Teachers
                .Select()
                .Build()
                .Execute();

            // Select with Where //
            var filteredPerson = dbContext.Persons
                .Select()
                .Where(BinaryExpression.Eq(new ColumnExpression("firstname"), new ValueExpression("test_firstname")))
                .Build()
                .Execute();

            var allPersons = dbContext.Persons
                .Select()
                .Where(BinaryExpression.Eq(new ValueExpression(1), new ValueExpression(1)))
                .Build()
                .Execute();
        }

        private static void ShowCase_Query_Lazy(ExampleDbContext dbContext)
        {
            // OneToMany + Inheritance //
            var teachers = dbContext.Teachers.Select().Build().Execute();
            var teacher = teachers.First();
            //// now classes are lazy loaded
            var classes = teacher.Classes;
            //// now lazy loaded students of the lazy loaded classes
            var studentsInClass = classes.First().Students;

            // ManyToOne
            var classes2 = dbContext.Classes.Select().Build().Execute();
            var c = classes2.FirstOrDefault();
            ////lazy load ManyToOne
            var teacher2 = c.Teacher;

            // ManyToMany + Inheritance
            var courses = dbContext.Courses.Select().Build().Execute();
            var course = courses.First();
            var students = course.Students;
        }


        private static void ShowCase_Change_Multiple(ExampleDbContext dbContext)
        {
            var ps = dbContext.Persons
                .Select()
                .Build()
                .Execute()
                .ToList();

            ps.ForEach(p => p.FirstName = "changed");

            dbContext.SaveChanges();

            var ps2 = dbContext.Persons
                .Select()
                .Build()
                .Execute()
                .ToList();
        }
    }
}