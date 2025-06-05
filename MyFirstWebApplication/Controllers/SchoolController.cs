using Microsoft.AspNetCore.Mvc;
using MyFirstWebApplication.Class;
using MyFirstWebApplication.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MyFirstWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly SchoolDbContext _context;

        public SchoolController(SchoolDbContext context)
        {
            _context = context;
        }

        [HttpPost("students")]
        public IActionResult AddStudent([FromBody] StudentDto studentDto)
        {
            try
            {
                Console.WriteLine($"Received StudentDto: Name={studentDto.Name}, Gender={studentDto.Gender}, DateOfBirth={studentDto.DateOfBirth}, ClassName={studentDto.ClassName}");

                if (!Enum.TryParse<Gender>(studentDto.Gender, true, out var gender))
                {
                    return BadRequest(new { message = "Invalid gender value." });
                }

                if (studentDto.DateOfBirth > DateTime.Today)
                {
                    return BadRequest(new { message = "Date of birth cannot be in the future." });
                }

                var school = _context.Schools.FirstOrDefault(s => s.Id == 1);
                if (school == null)
                {
                    return NotFound(new { message = "School not found." });
                }

                var student = new Student(GenerateUniqueStudentId(), gender, studentDto.DateOfBirth, studentDto.Name, studentDto.ClassName)
                {
                    SchoolId = school.Id
                };
                _context.Students.Add(student);
                _context.SaveChanges();
                return Ok(new { message = "Student added successfully" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Concurrency error: The data may have been modified or deleted. Please try again." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error adding student: {ex.Message}" });
            }
        }

        [HttpDelete("students/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            try
            {
                var student = _context.Students.FirstOrDefault(s => s.Id == id && s.SchoolId == 1);
                if (student == null)
                {
                    return NotFound(new { message = "Student not found." });
                }

                _context.Students.Remove(student);
                _context.SaveChanges();
                return Ok(new { message = "Student deleted successfully" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Concurrency error: The data may have been modified or deleted. Please try again." });
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
                var school = _context.Schools.FirstOrDefault(s => s.Id == 1);
                if (school == null)
                {
                    return NotFound(new { message = "School not found." });
                }

                var classroom = new Classroom(GenerateUniqueClassroomId(), classroomDto.RoomName, classroomDto.Size, classroomDto.Capacity, classroomDto.HasCynapSystem)
                {
                    SchoolId = school.Id
                };
                _context.Classrooms.Add(classroom);
                _context.SaveChanges();
                return Ok(new { message = "Classroom added successfully" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Concurrency error: The data may have been modified or deleted. Please try again." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error adding classroom: {ex.Message}" });
            }
        }

        [HttpDelete("classrooms/{id}")]
        public IActionResult DeleteClassroom(int id)
        {
            try
            {
                var classroom = _context.Classrooms.FirstOrDefault(c => c.Id == id && c.SchoolId == 1);
                if (classroom == null)
                {
                    return NotFound(new { message = "Classroom not found." });
                }

                _context.Classrooms.Remove(classroom);
                _context.SaveChanges();
                return Ok(new { message = "Classroom deleted successfully" });
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Concurrency error: The data may have been modified or deleted. Please try again." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("students")]
        public IActionResult GetAllStudents()
        {
            var students = _context.Students.Where(s => s.SchoolId == 1).ToList();
            return Ok(students);
        }

        [HttpGet("students/name/{name}")]
        public IActionResult GetStudentByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest(new { message = "Student name cannot be empty." });
                }

                // Use ToLower() for case-insensitive comparison, which EF Core can translate
                var student = _context.Students
                    .FirstOrDefault(s => s.SchoolId == 1 && s.Name.ToLower() == name.ToLower());
                if (student == null)
                {
                    return NotFound(new { message = "Student not found." });
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error retrieving student by name: {ex.Message}" });
            }
        }

        [HttpGet("classrooms")]
        public IActionResult GetAllClassrooms()
        {
            var classrooms = _context.Classrooms.Where(c => c.SchoolId == 1).ToList();
            return Ok(classrooms);
        }

        [HttpGet("classrooms/{roomName}")]
        public IActionResult GetClassroomByName(string roomName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roomName))
                {
                    return BadRequest(new { message = "Room name cannot be empty." });
                }

                var classroom = _context.Classrooms
                    .FirstOrDefault(c => c.SchoolId == 1 && c.RoomName.ToLower() == roomName.ToLower());
                if (classroom == null)
                {
                    return NotFound(new { message = "Classroom not found." });
                }

                return Ok(classroom);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error retrieving classroom by name: {ex.Message}" });
            }
        }

        [HttpGet("students/class/{className}")]
        public IActionResult GetStudentsByClass(string className)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(className))
                {
                    return BadRequest(new { message = "Class name cannot be empty." });
                }

                // Use ToLower() for case-insensitive comparison, which EF Core can translate
                var students = _context.Students
                    .Where(s => s.SchoolId == 1 && s.ClassName.ToLower() == className.ToLower())
                    .ToList();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Error retrieving students by class: {ex.Message}" });
            }
        }

        [HttpGet("classroom/fit")]
        public IActionResult CanClassFitInRoom([FromQuery] string className, [FromQuery] string roomName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(className))
                {
                    return BadRequest(new { message = "Class name cannot be empty." });
                }
                if (string.IsNullOrWhiteSpace(roomName))
                {
                    return BadRequest(new { message = "Room name cannot be empty." });
                }

                var studentsInClass = _context.Students
                    .Count(s => s.SchoolId == 1 && s.ClassName.ToLower() == className.ToLower());
                var room = _context.Classrooms
                    .FirstOrDefault(r => r.SchoolId == 1 && r.RoomName.ToLower() == roomName.ToLower());

                if (room == null)
                {
                    return NotFound(new { message = "Room not found." });
                }

                var canFit = room.Capacity >= studentsInClass;
                return Ok(new { className, roomName, canFit });
            }
            catch (Exception ex)
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
            } while (_context.Students.Any(s => s.Id == id));
            return id;
        }

        private int GenerateUniqueClassroomId()
        {
            var random = new Random();
            int id;
            do
            {
                id = random.Next(1, int.MaxValue);
            } while (_context.Classrooms.Any(c => c.Id == id));
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