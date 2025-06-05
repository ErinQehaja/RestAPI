using MyFirstWebApplication.Class;
using System;

namespace MyFirstWebApplication.Class
{
    public class Student : Person
    {
        private string _className;
        private string _name;

        public Student(int id, Gender gender, DateTime dateOfBirth, string name, string className)
            : base(id, gender, dateOfBirth)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ClassName = className ?? throw new ArgumentNullException(nameof(className));
        }

        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Name cannot be empty.", nameof(value)) : value;
        }

        public string ClassName
        {
            get => _className;
            set => _className = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Class name cannot be empty.", nameof(value)) : value;
        }
    }
}