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

        // Test AddStudent method
        [Test]
        public void AddStudent_ValidStudent_ReturnsSuccessMessage()
        {
            var studentDto = new StudentDto
            {
                Name = "John Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(2007, 4, 14),
                ClassName = "1A"
            };

            var result = _controller.AddStudent(studentDto) as OkObjectResult;

            Assert.IsNotNull(result);
            var response = result.Value;
            Assert.IsTrue(response.ToString().Contains("Student added successfully"));
        }

        [Test]
        public void AddStudent_InvalidGender_ReturnsBadRequest()
        {
            var studentDto = new StudentDto
            {
                Name = "John Doe",
                Gender = "InvalidGender",
                DateOfBirth = new DateTime(2007, 4, 14),
                ClassName = "1A"
            };

            var result = _controller.AddStudent(studentDto) as BadRequestObjectResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void AddStudent_FutureDateOfBirth_ReturnsBadRequest()
        {
            var studentDto = new StudentDto
            {
                Name = "John Doe",
                Gender = "Male",
                DateOfBirth = DateTime.Today.AddDays(1),
                ClassName = "1A"
            };

            var result = _controller.AddStudent(studentDto) as BadRequestObjectResult;

            Assert.IsNotNull(result);
        }

        // Test DeleteStudent method
        [Test]
        public void DeleteStudent_ExistingStudent_ReturnsSuccessMessage()
        {
            var studentDto = new StudentDto
            {
                Name = "John Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(2007, 4, 14),
                ClassName = "1A"
            };
            _controller.AddStudent(studentDto);

            // Get the student ID
            var studentsResult = _controller.GetAllStudents() as OkObjectResult;
            var students = studentsResult.Value as List<Student>;
            var studentId = students.First().Id;

            // Delete the student
            var result = _controller.DeleteStudent(studentId) as OkObjectResult;

            Assert.IsNotNull(result);
            var response = result.Value;
            Assert.IsTrue(response.ToString().Contains("Student deleted successfully"));
        }

        [Test]
        public void DeleteStudent_NonExistentStudent_ReturnsNotFound()
        {
            var result = _controller.DeleteStudent(999) as NotFoundObjectResult;

            Assert.IsNotNull(result);
        }

        // Test GetAllStudents method
        [Test]
        public void GetAllStudents_NoStudents_ReturnsEmptyList()
        {
            var result = _controller.GetAllStudents() as OkObjectResult;

            Assert.IsNotNull(result);
            var students = result.Value as List<Student>;
            Assert.AreEqual(0, students.Count);
        }

        [Test]
        public void GetAllStudents_WithStudents_ReturnsAllStudents()
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
                Name = "Jane Smith",
                Gender = "Female",
                DateOfBirth = new DateTime(2006, 6, 23),
                ClassName = "2B"
            };

            _controller.AddStudent(student1);
            _controller.AddStudent(student2);

            var result = _controller.GetAllStudents() as OkObjectResult;

            Assert.IsNotNull(result);
            var students = result.Value as List<Student>;
            Assert.AreEqual(2, students.Count);
        }

        // Test GetStudentByName method
        [Test]
        public void GetStudentByName_ExistingStudent_ReturnsStudent()
        {
            var studentDto = new StudentDto
            {
                Name = "John Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(2007, 4, 14),
                ClassName = "1A"
            };
            _controller.AddStudent(studentDto);

            var result = _controller.GetStudentByName("John Doe") as OkObjectResult;

            Assert.IsNotNull(result);
            var student = result.Value as Student;
            Assert.AreEqual("John Doe", student.Name);
        }

        [Test]
        public void GetStudentByName_NonExistentStudent_ReturnsNotFound()
        {
            var result = _controller.GetStudentByName("Non Existent") as NotFoundObjectResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetStudentByName_EmptyName_ReturnsBadRequest()
        {
            var result = _controller.GetStudentByName("") as BadRequestObjectResult;

            Assert.IsNotNull(result);
        }

        // Test GetStudentsByClass method
        [Test]
        public void GetStudentsByClass_ExistingClass_ReturnsStudentsInClass()
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
                Name = "Jane Smith",
                Gender = "Female",
                DateOfBirth = new DateTime(2006, 6, 23),
                ClassName = "1A"
            };
            var student3 = new StudentDto
            {
                Name = "Bob Johnson",
                Gender = "Male",
                DateOfBirth = new DateTime(2005, 8, 10),
                ClassName = "2B"
            };

            _controller.AddStudent(student1);
            _controller.AddStudent(student2);
            _controller.AddStudent(student3);

            var result = _controller.GetStudentsByClass("1A") as OkObjectResult;

            Assert.IsNotNull(result);
            var students = result.Value as List<Student>;
            Assert.AreEqual(2, students.Count);
            Assert.IsTrue(students.All(s => s.ClassName == "1A"));
        }

        [Test]
        public void GetStudentsByClass_NonExistentClass_ReturnsEmptyList()
        {
            var result = _controller.GetStudentsByClass("NonExistentClass") as OkObjectResult;

            Assert.IsNotNull(result);
            var students = result.Value as List<Student>;
            Assert.AreEqual(0, students.Count);
        }

        // Test AddClassroom method
        [Test]
        public void AddClassroom_ValidClassroom_ReturnsSuccessMessage()
        {
            var classroomDto = new ClassroomDto
            {
                RoomName = "Room 101",
                Size = 50.5,
                Capacity = 30,
                HasCynapSystem = true
            };

            var result = _controller.AddClassroom(classroomDto) as OkObjectResult;

            Assert.IsNotNull(result);
            var response = result.Value;
            Assert.IsTrue(response.ToString().Contains("Classroom added successfully"));
        }

        // Test DeleteClassroom method
        [Test]
        public void DeleteClassroom_ExistingClassroom_ReturnsSuccessMessage()
        {
            var classroomDto = new ClassroomDto
            {
                RoomName = "Room 101",
                Size = 50.5,
                Capacity = 30,
                HasCynapSystem = true
            };
            _controller.AddClassroom(classroomDto);

            // Get the classroom ID
            var classroomsResult = _controller.GetAllClassrooms() as OkObjectResult;
            var classrooms = classroomsResult.Value as List<Classroom>;
            var classroomId = classrooms.First().Id;

            // Delete the classroom
            var result = _controller.DeleteClassroom(classroomId) as OkObjectResult;

            Assert.IsNotNull(result);
            var response = result.Value;
            Assert.IsTrue(response.ToString().Contains("Classroom deleted successfully"));
        }

        [Test]
        public void DeleteClassroom_NonExistentClassroom_ReturnsNotFound()
        {
            var result = _controller.DeleteClassroom(999) as NotFoundObjectResult;

            Assert.IsNotNull(result);
        }

        // Test GetAllClassrooms method
        [Test]
        public void GetAllClassrooms_NoClassrooms_ReturnsEmptyList()
        {
            var result = _controller.GetAllClassrooms() as OkObjectResult;

            Assert.IsNotNull(result);
            var classrooms = result.Value as List<Classroom>;
            Assert.AreEqual(0, classrooms.Count);
        }

        [Test]
        public void GetAllClassrooms_WithClassrooms_ReturnsAllClassrooms()
        {
            var classroom1 = new ClassroomDto
            {
                RoomName = "Room 101",
                Size = 50.5,
                Capacity = 30,
                HasCynapSystem = true
            };
            var classroom2 = new ClassroomDto
            {
                RoomName = "Room 102",
                Size = 45.0,
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

        // Test GetClassroomByName method
        [Test]
        public void GetClassroomByName_ExistingClassroom_ReturnsClassroom()
        {
            var classroomDto = new ClassroomDto
            {
                RoomName = "Room 101",
                Size = 50.5,
                Capacity = 30,
                HasCynapSystem = true
            };
            _controller.AddClassroom(classroomDto);

            var result = _controller.GetClassroomByName("Room 101") as OkObjectResult;

            Assert.IsNotNull(result);
            var classroom = result.Value as Classroom;
            Assert.AreEqual("Room 101", classroom.RoomName);
        }

        [Test]
        public void GetClassroomByName_NonExistentClassroom_ReturnsNotFound()
        {
            var result = _controller.GetClassroomByName("Non Existent Room") as NotFoundObjectResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void GetClassroomByName_EmptyRoomName_ReturnsBadRequest()
        {
            var result = _controller.GetClassroomByName("") as BadRequestObjectResult;

            Assert.IsNotNull(result);
        }

        // Test CanClassFitInRoom method
        [Test]
        public void CanClassFitInRoom_ClassFitsInRoom_ReturnsTrue()
        {
            var classroomDto = new ClassroomDto
            {
                RoomName = "Room 101",
                Size = 50.5,
                Capacity = 30,
                HasCynapSystem = true
            };
            _controller.AddClassroom(classroomDto);

            var student1 = new StudentDto
            {
                Name = "John Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(2007, 4, 14),
                ClassName = "1A"
            };
            var student2 = new StudentDto
            {
                Name = "Jane Smith",
                Gender = "Female",
                DateOfBirth = new DateTime(2006, 6, 23),
                ClassName = "1A"
            };
            _controller.AddStudent(student1);
            _controller.AddStudent(student2);

            var result = _controller.CanClassFitInRoom("1A", "Room 101") as OkObjectResult;

            Assert.IsNotNull(result);
            dynamic response = result.Value;
            Assert.AreEqual("1A", response.GetType().GetProperty("className").GetValue(response));
            Assert.AreEqual("Room 101", response.GetType().GetProperty("roomName").GetValue(response));
            Assert.AreEqual(true, response.GetType().GetProperty("canFit").GetValue(response));
        }

        [Test]
        public void CanClassFitInRoom_ClassDoesNotFitInRoom_ReturnsFalse()
        {
            var classroomDto = new ClassroomDto
            {
                RoomName = "Small Room",
                Size = 20.0,
                Capacity = 1,
                HasCynapSystem = false
            };
            _controller.AddClassroom(classroomDto);

            var student1 = new StudentDto
            {
                Name = "John Doe",
                Gender = "Male",
                DateOfBirth = new DateTime(2007, 4, 14),
                ClassName = "1A"
            };
            var student2 = new StudentDto
            {
                Name = "Jane Smith",
                Gender = "Female",
                DateOfBirth = new DateTime(2006, 6, 23),
                ClassName = "1A"
            };
            _controller.AddStudent(student1);
            _controller.AddStudent(student2);

            var result = _controller.CanClassFitInRoom("1A", "Small Room") as OkObjectResult;

            Assert.IsNotNull(result);
            dynamic response = result.Value;
            Assert.AreEqual(false, response.GetType().GetProperty("canFit").GetValue(response));
        }

        [Test]
        public void CanClassFitInRoom_NonExistentRoom_ReturnsNotFound()
        {
            var result = _controller.CanClassFitInRoom("1A", "Non Existent Room") as NotFoundObjectResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void CanClassFitInRoom_EmptyClassName_ReturnsBadRequest()
        {
            var result = _controller.CanClassFitInRoom("", "Room 101") as BadRequestObjectResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void CanClassFitInRoom_EmptyRoomName_ReturnsBadRequest()
        {
            var result = _controller.CanClassFitInRoom("1A", "") as BadRequestObjectResult;

            Assert.IsNotNull(result);
        }
    }
}