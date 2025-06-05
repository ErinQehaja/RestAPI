using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyFirstWebApplication.Class;
using MyFirstWebApplication.Controllers;
using MyFirstWebApplication.Data;
using NUnit.Framework;

namespace MyFirstWebApplication.Tests
{
    public class SchoolTests
    {
        private SchoolDbContext _context;
        private SchoolController _controller;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<SchoolDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new SchoolDbContext(options);

            var school = new School(1, "Test School");
            _context.Schools.Add(school);
            _context.SaveChanges();

            _controller = new SchoolController(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void GetTotalStudents_AddStudent_IncreasesTotalStudents()
        {
            var studentDto = new StudentDto
            {
                Name = "John Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(2007, 4, 14),
                ClassName = "1A"
            };

            _controller.AddStudent(studentDto);

            var result = _controller.GetAllStudents() as OkObjectResult;
            Assert.IsNotNull(result);
            var students = result.Value as List<Student>;
            Assert.AreEqual(1, students.Count);
        }

        [Test]
        public void GetStudentsByGender_TwoDifferentGenders_ReturnsCorrectCount()
        {
            var student1 = new StudentDto
            {
                Name = "John Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(2007, 4, 14),
                ClassName = "1A"
            };
            var student2 = new StudentDto
            {
                Name = "Jane Doe",
                Gender = "Female",
                DateOfBirth = new DateTime(2006, 6, 23),
                ClassName = "1A"
            };

            _controller.AddStudent(student1);
            _controller.AddStudent(student2);

            var result = _controller.GetAllStudents() as OkObjectResult;
            Assert.IsNotNull(result);
            var students = result.Value as List<Student>;
            Assert.AreEqual(1, students.Count(s => s.Gender == Gender.Male));
            Assert.AreEqual(1, students.Count(s => s.Gender == Gender.Female));
        }

        [Test]
        public void GetTotalClassrooms_AddTwoClassrooms_ReturnsCorrectCount()
        {
            var classroom1 = new ClassroomDto
            {
                RoomName = "1A",
                Size = 50.0,
                Capacity = 30,
                HasCynapSystem = true
            };
            var classroom2 = new ClassroomDto
            {
                RoomName = "2B",
                Size = 60.0,
                Capacity = 25,
                HasCynapSystem = false
            };

            _controller.AddClassroom(classroom1);
            _controller.AddClassroom(classroom2);

            var result = _controller.GetAllClassrooms() as OkObjectResult;
            Assert.IsNotNull(result);
            var classrooms = result.Value as List<Classroom>;
            Assert.AreEqual(2, classrooms.Count);
        }

        [Test]
        public void GetAverageAge_ReturnsCorrectValue()
        {
            var student1 = new StudentDto
            {
                Name = "John Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(2007, 4, 14),
                ClassName = "1A"
            };
            var student2 = new StudentDto
            {
                Name = "Jane Doe",
                Gender = "Female",
                DateOfBirth = new DateTime(2006, 6, 23),
                ClassName = "1A"
            };

            _controller.AddStudent(student1);
            _controller.AddStudent(student2);

            var studentsResult = _controller.GetAllStudents() as OkObjectResult;
            Assert.IsNotNull(studentsResult);
            var students = studentsResult.Value as List<Student>;

            var today = DateTime.Today;
            double expectedAverageAge = (today - new DateTime(2007, 4, 14)).TotalDays / 365 +
                                       (today - new DateTime(2006, 6, 23)).TotalDays / 365;
            expectedAverageAge /= 2.0;

            double actualAverageAge = students.Average(s => (today - s.DateOfBirth).TotalDays / 365);

            Assert.AreEqual(expectedAverageAge, actualAverageAge, 0.01);
        }

        [Test]
        public void GetClassroomsWithCynap_AddTwoRoomsWithAndWithoutCynap_ReturnsCorrectRooms()
        {
            var roomWithCynap = new ClassroomDto
            {
                RoomName = "1A",
                Size = 50.0,
                Capacity = 30,
                HasCynapSystem = true
            };
            var roomWithoutCynap = new ClassroomDto
            {
                RoomName = "2B",
                Size = 60.0,
                Capacity = 25,
                HasCynapSystem = false
            };

            _controller.AddClassroom(roomWithCynap);
            _controller.AddClassroom(roomWithoutCynap);

            var result = _controller.GetAllClassrooms() as OkObjectResult;
            Assert.IsNotNull(result);
            var classrooms = result.Value as List<Classroom>;
            var cynapClassrooms = classrooms.Where(c => c.HasCynapSystem).ToList();

            Assert.AreEqual(1, cynapClassrooms.Count);
            Assert.AreEqual("1A", cynapClassrooms.First().RoomName);
        }

        [Test]
        public void GetTotalClasses_AddTwoClasses_ReturnsCorrectCount()
        {
            var student1 = new StudentDto
            {
                Name = "John Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(2007, 4, 14),
                ClassName = "1A"
            };
            var student2 = new StudentDto
            {
                Name = "Jane Doe",
                Gender = "Female",
                DateOfBirth = new DateTime(2006, 6, 23),
                ClassName = "2B"
            };

            _controller.AddStudent(student1);
            _controller.AddStudent(student2);

            var result = _controller.GetAllStudents() as OkObjectResult;
            Assert.IsNotNull(result);
            var students = result.Value as List<Student>;
            var distinctClasses = students.Select(s => s.ClassName).Distinct().Count();

            Assert.AreEqual(2, distinctClasses);
        }

        [Test]
        public void GetClassesWithStudentCount_AddThreeStudents_ReturnsCorrectData()
        {
            var student1 = new StudentDto
            {
                Name = "John Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(2007, 4, 14),
                ClassName = "1A"
            };
            var student2 = new StudentDto
            {
                Name = "Jane Doe",
                Gender = "Female",
                DateOfBirth = new DateTime(2006, 6, 23),
                ClassName = "1A"
            };
            var student3 = new StudentDto
            {
                Name = "Bob Smith",
                Gender = "Male",
                DateOfBirth = new DateTime(2005, 5, 15),
                ClassName = "2B"
            };

            _controller.AddStudent(student1);
            _controller.AddStudent(student2);
            _controller.AddStudent(student3);

            var result = _controller.GetAllStudents() as OkObjectResult;
            Assert.IsNotNull(result);
            var students = result.Value as List<Student>;
            var classCounts = students.GroupBy(s => s.ClassName)
                                     .ToDictionary(g => g.Key, g => g.Count());

            Assert.AreEqual(2, classCounts["1A"]);
            Assert.AreEqual(1, classCounts["2B"]);
        }

        [Test]
        public void GetFemalePercentageInClass_AddOneFemale_ReturnsCorrectValue()
        {
            var student1 = new StudentDto
            {
                Name = "John Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(2007, 4, 14),
                ClassName = "1A"
            };
            var student2 = new StudentDto
            {
                Name = "Jane Doe",

                Gender = "Female",
                DateOfBirth = new DateTime(2006, 6, 23),
                ClassName = "1A"
            };

            _controller.AddStudent(student1);
            _controller.AddStudent(student2);

            var result = _controller.GetStudentsByClass("1A") as OkObjectResult;
            Assert.IsNotNull(result);
            var students = result.Value as List<Student>;
            var femaleCount = students.Count(s => s.Gender == Gender.Female);
            var totalCount = students.Count;
            var femalePercentage = (double)femaleCount / totalCount * 100;

            Assert.AreEqual(50.0, femalePercentage);
        }
    }
}