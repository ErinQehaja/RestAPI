using System;

namespace MyFirstWebApplication.Class
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public abstract class Person
    {
        public int Id { get; set; } 
        public Gender Gender { get; set; } 
        public DateTime DateOfBirth { get; set; } 

        protected Person() { } // Parameterless constructor for EF

        public Person(int id, Gender gender, DateTime dateOfBirth)
        {
            if (id <= 0) throw new ArgumentException("ID must be positive.", nameof(id));
            if (!Enum.IsDefined(typeof(Gender), gender)) throw new ArgumentException("Invalid gender.", nameof(gender));
            if (dateOfBirth > DateTime.Today) throw new ArgumentException("Date of birth cannot be in the future.", nameof(dateOfBirth));

            Id = id;
            Gender = gender;
            DateOfBirth = dateOfBirth;
        }
    }
}