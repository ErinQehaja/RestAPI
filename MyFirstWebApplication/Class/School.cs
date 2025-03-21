using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstWebApplication.Class
{
    public class School
    {
        public string Name { get; set; }
        public List<Student> Students { get; private set; }
        public List<Classroom> Classrooms { get; private set; }

        public School(string name)
        {
            Name = name;
            Students = new List<Student>();
            Classrooms = new List<Classroom>();
        }

        public void AddStudent(Student student)
        {
            Students.Add(student);
        }

        public void AddClassroom(Classroom classroom)
        {
            Classrooms.Add(classroom);
        }

        public int GetTotalStudents()
        {
            return Students.Count;
        }

        public int GetStudentsByGender(string gender)
        {
            return Students.Count(s => s.GenderPerson.Equals(gender, StringComparison.OrdinalIgnoreCase));
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

        public List<Classroom> GetClassroomsWithCynap()
        {
            return Classrooms.Where(c => c.HasCynap).ToList();
        }

        public int GetTotalClasses()
        {
            return Students.Select(s => s.Class).Distinct().Count();
        }

        public Dictionary<string, int> GetClassesWithStudentCount()
        {
            return Students.GroupBy(s => s.Class)
                           .ToDictionary(g => g.Key, g => g.Count());
        }

        public double GetFemalePercentageInClass(string className)
        {
            var studentsInClass = Students.Where(s => s.Class == className).ToList();
            if (!studentsInClass.Any()) return 0;

            var femaleCount = studentsInClass.Count(s => s.GenderPerson.Equals("Female", StringComparison.OrdinalIgnoreCase));
            return (double)femaleCount / studentsInClass.Count * 100;
        }

        public bool CanClassFitInRoom(string className, string roomName)
        {
            var studentsInClass = Students.Count(s => s.Class == className);
            var room = Classrooms.FirstOrDefault(r => r.RoomName == roomName);
            return room != null && room.Capacity >= studentsInClass;
        }

        public List<Student> GetAllStudents()
        {
            return Students;
        }
    }
}
