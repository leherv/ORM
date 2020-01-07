using System;

namespace ORM_Example
{
    public class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; } 
        public Gender Gender { get; set; }
        public DateTime BDay { get; set; }

        public Person(string name, string firstName, Gender gender, DateTime bDay)
        {
            Name = name;
            FirstName = firstName;
            Gender = gender;
            BDay = bDay;
        }

        //TODO: doc - every poco needs to have an empty constructor and only get sets! 
        public Person() { }
    }
}