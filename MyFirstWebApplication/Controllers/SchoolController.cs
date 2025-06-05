using Microsoft.AspNetCore.Mvc;
using MyFirstWebApplication.Class;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private static readonly School _school = new School(1, "MySchool");

        [HttpPost("students")]
        public IActionResult AddStudent([FromBody] StudentDto studentDto)
        {
            try
            {
                if (!Enum.TryParse<Gender>(studentDto.Gender, true, out var gender))
                {
                    return BadRequest(new { message = "Invalid gender value." });
                }

                var student = new Student(GenerateUniqueStudentId(), gender, studentDto.DateOfBirth, studentDto.Name, studentDto.ClassName);
                _school.AddStudent(student);
                return Ok(new { message = "Student added successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("students/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                var success = _school.RemoveStudent(id);
                if (!success)
                {
                    return NotFound(new { message = "Student not found." });
                }
                return Ok(new { message = "Student deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("classrooms")]
        public IActionResult AddClassroom([FromBody] ClassroomDto classroomDto)
        {
            try
            {
                var classroom = new Classroom(GenerateUniqueClassroomId(), classroomDto.RoomName, classroomDto.Size, classroomDto.Capacity, classroomDto.HasCynapSystem);
                _school.AddClassroom(classroom);
                return Ok(new { message = "Classroom added successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("classrooms/{id}")]
        public IActionResult DeleteClassroom(int id)
        {
            try
            {
                var success = _school.RemoveClassroom(id);
                if (!success)
                {
                    return NotFound(new { message = "Classroom not found." });
                }
                return Ok(new { message = "Classroom deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("students")]
        public IActionResult GetAllStudents()
        {
            return Ok(_school.Students);
        }

        [HttpGet("students/name/{name}")]
        public IActionResult GetStudentByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new { message = "Student name cannot be empty." });
            }

            var student = _school.Students
                .FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (student == null)
            {
                return NotFound(new { message = "Student not found." });
            }

            return Ok(student);
        }

        [HttpGet("classrooms")]
        public IActionResult GetAllClassrooms()
        {
            return Ok(_school.Classrooms);
        }

        [HttpGet("classrooms/{roomName}")]
        public IActionResult GetClassroomByName(string roomName)
        {
            if (string.IsNullOrWhiteSpace(roomName))
            {
                return BadRequest(new { message = "Room name cannot be empty." });
            }

            var classroom = _school.Classrooms
                .FirstOrDefault(c => c.RoomName.Equals(roomName, StringComparison.OrdinalIgnoreCase));
            if (classroom == null)
            {
                return NotFound(new { message = "Classroom not found." });
            }

            return Ok(classroom);
        }

        [HttpGet("students/class/{className}")]
        public IActionResult GetStudentsByClass(string className)
        {
            if (string.IsNullOrWhiteSpace(className))
            {
                return BadRequest(new { message = "Class name cannot be empty." });
            }

            var students = _school.Students
                .Where(s => s.ClassName.Equals(className, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Ok(students);
        }

        [HttpGet("classroom/fit")]
        public IActionResult CanClassFitInRoom([FromQuery] string className, [FromQuery] string roomName)
        {
            try
            {
                var canFit = _school.CanClassFitInRoom(className, roomName);
                return Ok(new { className, roomName, canFit });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private int GenerateUniqueStudentId()
        {
            var random = new Random();
            int id;
            do
            {
                id = random.Next(1, int.MaxValue);
            } while (_school.Students.Any(s => s.Id == id));
            return id;
        }

        private int GenerateUniqueClassroomId()
        {
            var random = new Random();
            int id;
            do
            {
                id = random.Next(1, int.MaxValue);
            } while (_school.Classrooms.Any(c => c.Id == id));
            return id;
        }
    }

    public class StudentDto
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ClassName { get; set; }
    }

    public class ClassroomDto
    {
        public string RoomName { get; set; }
        public double Size { get; set; }
        public int Capacity { get; set; }
        public bool HasCynapSystem { get; set; }
    }
}