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
        public string Name { get; set; }

        public bool Active { get; set; }


        [ManyToOne]
        public Teacher Teacher
        {
            get => LazyLoader.Load(this, ref _teacher);
            set => _teacher = value;
        }
        private Teacher _teacher;

        [ManyToMany(TableName = "student_course", ForeignKeyNear = "fk_course_id", ForeignKeyFar = "fk_person_id")]
        public ICollection<Student> Students
        {
            get => LazyLoader.Load(this, ref _students);
            set => _students = value;
        }
        private ICollection<Student> _students;

        public Course(bool active, string name)
        {
            Active = active;
            Name = name;
        }

        public Course() { }
    }
}