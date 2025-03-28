using Microsoft.AspNetCore.Mvc;
using MyFirstWebApplication.Class;
using System;
using System.Collections.Generic;

namespace MyFirstWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private static School _school = new School("MySchool");

        [HttpPost("students")]
        public IActionResult AddStudent([FromBody] StudentDto studentDto)
        {
            var student = new Student(studentDto.Gender, studentDto.DateOfBirth, studentDto.Class);
            _school.AddStudent(student);
            return Ok(new { message = "Student added successfully" });
        }

        [HttpGet("students")]
        public IActionResult GetAllStudents()
        {
            return Ok(_school.GetAllStudents());
        }

        [HttpGet("students/class/{className}")]
        public IActionResult GetStudentsByClass(string className)
        {
            var students = _school.GetAllStudents()
                .Where(s => s.Class.Equals(className, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Ok(students);
        }

        [HttpGet("classroom/fit")]
        public IActionResult CanClassFitInRoom([FromQuery] string className, [FromQuery] string roomName)
        {
            var canFit = _school.CanClassFitInRoom(className, roomName);
            return Ok(new { className, roomName, canFit });
        }
    }

    public class StudentDto
    {
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Class { get; set; }
    }
}