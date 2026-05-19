using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;
using System.Security.Claims; 

namespace SchoolManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _context.Students.ToListAsync();

            foreach (var student in students)
            {
                if (string.IsNullOrEmpty(student.Status))
                {
                    student.Status = "Active";
                }
            }
            await _context.SaveChangesAsync();

            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();
            return student;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            var newUser = new User
            {
                Email = student.Email,
                Password = student.Password,
                Role = "Student"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            student.UserId = newUser.Id;

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return Ok(student);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id) return BadRequest();

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(e => e.Id == id)) return NotFound();
                else throw;
            }
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("list")]
        public IActionResult GetStudentsPost()
        {
            var students = _context.Students.ToList();
            return Ok(students);
        }

        [HttpGet("my-profile")]
        public async Task<ActionResult<object>> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized(new { message = "Invalid token claims." });

            int userId = int.Parse(userIdClaim.Value);

            var student = await _context.Students
                .Include(s => s.Classroom)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null) return NotFound(new { message = "Student profile not found." });

            return Ok(new
            {
                Id = student.Id,
                Name = student.Name,
                BirthDate = student.BirthDate,
                ClassroomName = student.Classroom != null ? student.Classroom.Name : "Not Assigned",
                Status = student.Status
            });
        }
    }
}