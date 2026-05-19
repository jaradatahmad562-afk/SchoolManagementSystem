using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GradesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Grades
        [HttpGet]
        public async Task<IActionResult> GetGrades()
        {
            var grades = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .Select(g => new
                {
                    id = g.Id,
                    studentId = g.StudentId,
                    studentName = g.Student.Name,
                    subjectId = g.SubjectId,
                    subjectName = g.Subject.Title,
                    firstExam = g.FirstExam,
                    secondExam = g.SecondExam,
                    finalExam = g.FinalExam,
                    total = g.FirstExam + g.SecondExam + g.FinalExam
                })
                .ToListAsync();

            return Ok(grades);
        }

        // GET: api/Grades/students
        // Exclusively added to fill the Students Dropdown in Angular
        [HttpGet("students")]
        public async Task<IActionResult> GetStudentsForDropdown()
        {
            var students = await _context.Students
                .Select(s => new { id = s.Id, name = s.Name })
                .ToListAsync();

            return Ok(students);
        }

        // GET: api/Grades/subjects
        // Exclusively added to fill the Subjects Dropdown in Angular
        [HttpGet("subjects")]
        public async Task<IActionResult> GetSubjectsForDropdown()
        {
            var subjects = await _context.Subjects
                .Select(s => new { id = s.Id, title = s.Title })
                .ToListAsync();

            return Ok(subjects);
        }

        // POST: api/Grades
        [HttpPost]
        public async Task<IActionResult> AddGrade(Grade grade)
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();

            // Return a simple success message to prevent JSON Object Cycle (Infinite Loop)
            return Ok(new { message = "Grade added successfully!" });
        }

        // PUT: api/Grades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGrade(int id, Grade grade)
        {
            if (id != grade.Id) return BadRequest();

            _context.Entry(grade).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Return a simple success message
            return Ok(new { message = "Grade updated successfully!" });
        }

        // DELETE: api/Grades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null) return NotFound();

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}