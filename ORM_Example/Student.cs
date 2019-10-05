using System;
using System.Collections.Generic;
using ORM_Example;

namespace ORM
{
    public class Student : Person
    {
        public List<Course> Courses { get; }


        public Student(string name, string firstName, Gender gender, DateTime bDay, List<Course> courses) : base(name, firstName, gender,
            bDay)
        {
            Courses = courses;
        }
    }
}