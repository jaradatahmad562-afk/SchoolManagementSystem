using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentPortalController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentPortalController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("my-profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var student = await _context.Students
                .Include(s => s.Classroom)
                .FirstOrDefaultAsync(s => s.UserId == userId);

            if (student == null) return NotFound("Student profile not found.");

            var profileData = new
            {
                fullName = student.Name,
                birthDate = student.BirthDate,
                classroom = student.Classroom != null ? student.Classroom.Name : "Not Assigned",
                status = "Active"
            };

            return Ok(profileData);
        }

        [HttpGet("my-grades")]
        public async Task<IActionResult> GetMyGrades()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null) return NotFound("Student not found.");

            var studentGrades = await _context.Grades
                .Include(g => g.Subject)
                .Where(g => g.StudentId == student.Id)
                .Select(g => new
                {
                    SubjectName = g.Subject.Title,
                    FirstExam = g.FirstExam,
                    SecondExam = g.SecondExam,
                    FinalExam = g.FinalExam,
                    Total = g.FirstExam + g.SecondExam + g.FinalExam
                })
                .ToListAsync();

            return Ok(studentGrades);
        }

        [HttpGet("my-schedule")]
        public async Task<IActionResult> GetMySchedule()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim)) return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == userId);
            if (student == null) return NotFound("Student not found.");

            var schedule = await _context.Schedules
                .Where(s => s.ClassroomId == student.ClassroomId)
                .Select(s => new
                {
                    DayOfWeek = s.Day.ToString(),
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    SubjectName = s.Subject != null ? s.Subject.Title : string.Empty,
                    TeacherName = (s.Subject != null && s.Subject.Teacher != null) ? s.Subject.Teacher.Name : "Not Assigned"
                })
                .ToListAsync();

            return Ok(schedule);
        }
    }
}