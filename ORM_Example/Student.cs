using System;
using System.Collections.Generic;
using ORM_Lib.Constraints_Attributes;

namespace ORM_Example
{
    public class Student : Person
    {
        [ManyToMany(TableName = "student_course", ForeignKeyNear = "fk_course_id", ForeignKeyFar = "fk_person_id")]
        public List<Course> Courses { get; set; }
        
        [ManyToOne]
        public Class Class { get; set; }

        public Student(string name, string firstName, Gender gender, DateTime bDay, List<Course> courses) : base(name,
            firstName, gender,
            bDay)
        {
            Courses = courses;
        }
    }
}