using System.Collections.Generic;

namespace ORM_Example
{
    public class Class
    {
        public long Id { get; set; }
        public string Name { get; }
        public List<Student> Students { get; }
        public Teacher Teacher { get; }

        public Class(string name, List<Student> students, Teacher teacher)
        {
            Name = name;
            Students = students;
            Teacher = teacher;
        }
    }
}