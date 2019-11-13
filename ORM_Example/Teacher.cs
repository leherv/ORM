using System;
using System.Collections.Generic;
using ORM_Lib.Constraints_Attributes;

namespace ORM_Example
{
    [TableName("Custom_name")]
    public class Teacher : Person
    {
        [ColumnName("Custom_name")]
        public int Salary { get; set; }
        public List<Course> Courses { get; set; }
        
        public Teacher(string name, string firstName, Gender gender, DateTime bDay, int salary, List<Course> courses) : base(name, firstName, gender, bDay)
        {
            Salary = salary;
            Courses = courses;
        }
    }
}