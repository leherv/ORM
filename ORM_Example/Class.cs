using System.Collections.Generic;
using ORM_Lib.Attributes;
using ORM_Lib.Cache;

namespace ORM_Example
{
    public class Class
    {
        private ILazyLoader LazyLoader { get; set; } = new LazyLoader();

        public long Id { get; set; }
        public string Name { get; }

        [OneToMany]
        public List<Student> Students
        {
            get => LazyLoader.Load(this, ref _students);
            set => _students = value;
        }
        private List<Student> _students;

        [ManyToOne]
        public Teacher Teacher
        {
            get => LazyLoader.Load(this, ref _teacher);
            set => _teacher = value;
        }
        private Teacher _teacher;

        public Class(string name, List<Student> students, Teacher teacher)
        {
            Name = name;
            Students = students;
            Teacher = teacher;
        }

        public Class() { }
    }
}