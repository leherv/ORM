using System.Collections.Generic;
using ORM_Lib.Constraints_Attributes;

namespace ORM_Example
{
    public class Class
    {
        public long Id { get; set; }
        public string Name { get; }
        
        [OneToMany]
        public List<Student> Students { get; }
        
        [ManyToOne]
        public Teacher Teacher { get; }

        public Class(string name, List<Student> students, Teacher teacher)
        {
            Name = name;
            Students = students;
            Teacher = teacher;
        }

        public Class() { }
    }
}