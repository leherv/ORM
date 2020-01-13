using System;
using System.Collections.Generic;
using ORM_Lib.Attributes;
using ORM_Lib.Cache;

namespace ORM_Example
{
    [TableName("Custom_name")]
    public class Teacher : Person
    {
        private ILazyLoader LazyLoader { get; set; } = new LazyLoader();

        [ColumnName("Custom_name")]
        public int Salary { get; set; }

        [OneToMany(mappedByColumnName ="teacher")]
        public ICollection<Course> Courses
        {
            get => LazyLoader.Load(this, ref _courses);
            set => _courses = value;
        }
        private ICollection<Course> _courses;

        [OneToMany(mappedByColumnName ="teacher")]
        public ICollection<Class> Classes
        {
            get => LazyLoader.Load(this, ref _classes);
            set => _classes = value;
        }
        private ICollection<Class> _classes;

        public Teacher(string name, string firstName, Gender gender, DateTime bDay, int salary) : base(name, firstName, gender, bDay)
        {
            Salary = salary;
        }

        public Teacher() { }
    }
}