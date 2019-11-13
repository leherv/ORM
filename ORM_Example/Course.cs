using System.Collections.Generic;
using ORM_Lib.Constraints_Attributes;

namespace ORM_Example
{
    public class Course
    {
        [Pk]
        public long OtherNameThanId { get; set; }
        public bool Active { get; }
        public string Name { get; }
        public Teacher Teacher { get; }
        
        [ManyToMany(TableName = "student_course", ForeignKeyNear = "fk_person_id", ForeignKeyFar = "fk_course_id")]
        public List<Student> Students { get; }

        public Course(bool active, string name, Teacher teacher, List<Student> students)
        {
            Active = active;
            Name = name;
            Teacher = teacher;
            Students = students;
        }
    }
}