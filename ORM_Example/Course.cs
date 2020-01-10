using System.Collections.Generic;
using ORM_Lib.Attributes;
using ORM_Lib.Cache;

namespace ORM_Example
{
    public class Course
    {

        private ILazyLoader LazyLoader { get; set; } = new LazyLoader();

        [Pk]
        public long OtherNameThanId { get; set; }
        public bool Active { get; }
        public string Name { get; }

        [ManyToOne]
        public Teacher Teacher
        {
            get => LazyLoader.Load(this, ref _teacher);
            set => _teacher = value;
        }
        private Teacher _teacher;

        [ManyToMany(TableName = "student_course", ForeignKeyNear = "fk_person_id", ForeignKeyFar = "fk_course_id")]
        public List<Student> Students
        {
            get => LazyLoader.Load(this, ref _students);
            set => _students = value;
        }
        private List<Student> _students;

        public Course(bool active, string name, Teacher teacher, List<Student> students)
        {
            Active = active;
            Name = name;
            Teacher = teacher;
            Students = students;
        }

        public Course() { }
    }
}