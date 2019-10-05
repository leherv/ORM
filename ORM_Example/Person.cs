using System;
using ORM_Example;

namespace ORM
{
    public class Person
    {
        public string Name { get; }
        public string FirstName { get; } 
        public Gender Gender { get; }
        public DateTime BDay { get; }

        public Person(string name, string firstName, Gender gender, DateTime bDay)
        {
            Name = name;
            FirstName = firstName;
            Gender = gender;
            BDay = bDay;
        }
    }
}