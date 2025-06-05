using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstWebApplication.Class
{
    public class School
    {
        public int Id { get; set; } 
        public string Name { get; set; } = null!; 
        public List<Student> Students { get; set; } = new List<Student>(); // Navigation property for EF
        public List<Classroom> Classrooms { get; set; } = new List<Classroom>(); // Navigation property for EF

        public School() { } // Parameterless constructor for EF

        public School(int id, string name)
        {
            if (id <= 0) throw new ArgumentException("ID must be positive.", nameof(id));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));

            Id = id;
            Name = name;
        }

        public void AddStudent(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));
            if (Students.Any(s => s.Id == student.Id)) throw new ArgumentException("Student already exists.", nameof(student));

            Students.Add(student);
        }

        public void AddClassroom(Classroom classroom)
        {
            if (classroom == null) throw new ArgumentNullException(nameof(classroom));
            if (Classrooms.Any(c => c.Id == classroom.Id)) throw new ArgumentException("Classroom already exists.", nameof(classroom));

            Classrooms.Add(classroom);
        }

        public bool RemoveStudent(int studentId)
        {
            var student = Students.FirstOrDefault(s => s.Id == studentId);
            if (student == null) return false;

            return Students.Remove(student);
        }

        public bool RemoveClassroom(int classroomId)
        {
            var classroom = Classrooms.FirstOrDefault(c => c.Id == classroomId);
            if (classroom == null) return false;

            return Classrooms.Remove(classroom);
        }

        public int GetTotalStudents()
        {
            return Students.Count;
        }

        public int GetStudentsByGender(Gender gender)
        {
            return Students.Count(s => s.Gender == gender);
        }

        public int GetTotalClassrooms()
        {
            return Classrooms.Count;
        }

        public double GetAverageAge()
        {
            if (!Students.Any()) return 0;

            var today = DateTime.Today;
            return Students.Average(s => (today - s.DateOfBirth).TotalDays / 365);
        }

        public IReadOnlyList<Classroom> GetClassroomsWithCynapSystem()
        {
            return Classrooms.Where(c => c.HasCynapSystem).ToList().AsReadOnly();
        }

        public int GetTotalDistinctClasses()
        {
            return Students.Select(s => s.ClassName).Distinct().Count();
        }

        public IReadOnlyDictionary<string, int> GetClassesWithStudentCount()
        {
            return Students.GroupBy(s => s.ClassName)
                           .ToDictionary(g => g.Key, g => g.Count())
                           .AsReadOnly();
        }

        public double gotFemalePercentageInClass(string className)
        {
            if (string.IsNullOrWhiteSpace(className)) throw new ArgumentException("Class name cannot be empty.", nameof(className));

            var studentsInClass = Students.Where(s => s.ClassName == className).ToList();
            if (!studentsInClass.Any()) return 0;

            var femaleCount = studentsInClass.Count(s => s.Gender == Gender.Female);
            return (double)femaleCount / studentsInClass.Count * 100;
        }

        public bool CanClassFitInRoom(string className, string roomName)
        {
            if (string.IsNullOrWhiteSpace(className)) throw new ArgumentException("Class name cannot be empty.", nameof(className));
            if (string.IsNullOrWhiteSpace(roomName)) throw new ArgumentException("Room name cannot be empty.", nameof(roomName));

            var studentsInClass = Students.Count(s => s.ClassName == className);
            var room = Classrooms.FirstOrDefault(r => r.RoomName == roomName);
            return room != null && room.Capacity >= studentsInClass;
        }
    }
}