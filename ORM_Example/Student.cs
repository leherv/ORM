using System;
using System.Collections.Generic;
using ORM_Lib.Attributes;
using ORM_Lib.Cache;

namespace ORM_Example
{
    public class Student : Person
    {
        private ILazyLoader LazyLoader { get; set; } = new LazyLoader();

        [ManyToMany(TableName = "student_course", ForeignKeyNear = "fk_course_id", ForeignKeyFar = "fk_person_id")]
        public ICollection<Course> Courses
        {
            get => LazyLoader.Load(this, ref _courses);
            set => _courses = value;
        }
        private ICollection<Course> _courses;

        [ManyToOne]
        public Class Class
        {
            get => LazyLoader.Load(this, ref _class);
            set => _class = value;
        }
        private Class _class;

        public Student(string name, string firstName, Gender gender, DateTime bDay, List<Course> courses) : base(name,
            firstName, gender,
            bDay)
        {
            Courses = courses;
        }

        public Student() { }
    }
}