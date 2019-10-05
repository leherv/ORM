using System;
using System.Collections.Generic;
using ORM_Example;

namespace ORM
{
    public class Teacher : Person
    {
        public int Salary { get; }
        public List<Course> Courses { get; }
        
        public Teacher(string name, string firstName, Gender gender, DateTime bDay, int salary, List<Course> courses) : base(name, firstName, gender, bDay)
        {
            Salary = salary;
            Courses = courses;
        }
    }
}