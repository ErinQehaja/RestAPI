using MyFirstWebApplication.Class;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstWebApplication.Class
{
    public class School
    {
        private readonly int _id;
        private string _name;
        private readonly List<Student> _students;
        private readonly List<Classroom> _classrooms;

        public School(int id, string name)
        {
            if (id <= 0) throw new ArgumentException("ID must be positive.", nameof(id));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));

            _id = id;
            _name = name;
            _students = new List<Student>();
            _classrooms = new List<Classroom>();
        }

        public int Id => _id;
        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Name cannot be empty.", nameof(value)) : value;
        }
        public IReadOnlyList<Student> Students => _students.AsReadOnly();
        public IReadOnlyList<Classroom> Classrooms => _classrooms.AsReadOnly();

        public void AddStudent(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));
            if (_students.Any(s => s.Id == student.Id)) throw new ArgumentException("Student already exists.", nameof(student));

            _students.Add(student);
        }

        public void AddClassroom(Classroom classroom)
        {
            if (classroom == null) throw new ArgumentNullException(nameof(classroom));
            if (_classrooms.Any(c => c.Id == classroom.Id)) throw new ArgumentException("Classroom already exists.", nameof(classroom));

            _classrooms.Add(classroom);
        }

        public bool RemoveStudent(int studentId)
        {
            var student = _students.FirstOrDefault(s => s.Id == studentId);
            if (student == null) return false;

            return _students.Remove(student);
        }

        public bool RemoveClassroom(int classroomId)
        {
            var classroom = _classrooms.FirstOrDefault(c => c.Id == classroomId);
            if (classroom == null) return false;

            return _classrooms.Remove(classroom);
        }

        public int GetTotalStudents()
        {
            return _students.Count;
        }

        public int GetStudentsByGender(Gender gender)
        {
            return _students.Count(s => s.Gender == gender);
        }

        public int GetTotalClassrooms()
        {
            return _classrooms.Count;
        }

        public double GetAverageAge()
        {
            if (!_students.Any()) return 0;

            var today = DateTime.Today;
            return _students.Average(s => (today - s.DateOfBirth).TotalDays / 365);
        }

        public IReadOnlyList<Classroom> GetClassroomsWithCynapSystem()
        {
            return _classrooms.Where(c => c.HasCynapSystem).ToList().AsReadOnly();
        }

        public int GetTotalDistinctClasses()
        {
            return _students.Select(s => s.ClassName).Distinct().Count();
        }

        public IReadOnlyDictionary<string, int> GetClassesWithStudentCount()
        {
            return _students.GroupBy(s => s.ClassName)
                           .ToDictionary(g => g.Key, g => g.Count())
                           .AsReadOnly();
        }

        public double GetFemalePercentageInClass(string className)
        {
            if (string.IsNullOrWhiteSpace(className)) throw new ArgumentException("Class name cannot be empty.", nameof(className));

            var studentsInClass = _students.Where(s => s.ClassName == className).ToList();
            if (!studentsInClass.Any()) return 0;

            var femaleCount = studentsInClass.Count(s => s.Gender == Gender.Female);
            return (double)femaleCount / studentsInClass.Count * 100;
        }

        public bool CanClassFitInRoom(string className, string roomName)
        {
            if (string.IsNullOrWhiteSpace(className)) throw new ArgumentException("Class name cannot be empty.", nameof(className));
            if (string.IsNullOrWhiteSpace(roomName)) throw new ArgumentException("Room name cannot be empty.", nameof(roomName));

            var studentsInClass = _students.Count(s => s.ClassName == className);
            var room = _classrooms.FirstOrDefault(r => r.RoomName == roomName);
            return room != null && room.Capacity >= studentsInClass;
        }
    }
}