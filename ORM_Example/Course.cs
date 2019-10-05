using System.Collections.Generic;

namespace ORM
{
    public class Course
    {
        public bool Active { get; }
        public string Name { get; }
        public Teacher Teacher { get; }
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