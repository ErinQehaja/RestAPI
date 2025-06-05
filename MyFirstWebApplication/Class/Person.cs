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
        private readonly int _id;
        private readonly Gender _gender;
        private readonly DateTime _dateOfBirth;

        protected Person(int id, Gender gender, DateTime dateOfBirth)
        {
            if (id <= 0) throw new ArgumentException("ID must be positive.", nameof(id));
            if (!Enum.IsDefined(typeof(Gender), gender)) throw new ArgumentException("Invalid gender.", nameof(gender));
            if (dateOfBirth > DateTime.Today) throw new ArgumentException("Date of birth cannot be in the future.", nameof(dateOfBirth));

            _id = id;
            _gender = gender;
            _dateOfBirth = dateOfBirth;
        }

        public int Id => _id;
        public Gender Gender => _gender;
        public DateTime DateOfBirth => _dateOfBirth;
    }
}