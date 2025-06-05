using System;
using System.ComponentModel.DataAnnotations;

namespace MyFirstWebApplication.Class
{
    public class Student : Person
    {
        public string Name { get; set; } = null!; // EF requires non-nullable for required fields
        public string ClassName { get; set; } = null!; // EF requires non-nullable for required fields

        public int SchoolId { get; set; } // Foreign key to School
        public School School { get; set; } // Navigation property for EF

        public Student() : base() { } // Parameterless constructor for EF

        public Student(int id, Gender gender, DateTime dateOfBirth, string name, string className)
            : base(id, gender, dateOfBirth)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ClassName = className ?? throw new ArgumentNullException(nameof(className));
        }
    }
}