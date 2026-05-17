using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [Authorize] // 👈 1. شلنا التعليق وخليناها عامة (أدمن + مدرس) عشان الطرفين يقدروا يقرأوا بيانات الطلاب
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        // 🟢 مسموح للكل (أدمن ومدرس) عشان المدرس يشوف طلابه
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

        // 🟢 مسموح للكل لقراءة تفاصيل طالب معين
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();
            return student;
        }

        // 🔴 2. قفلنا إضافة طالب جديد للأدمن فقط 🔒
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return Ok(student);
        }

        // 🔴 3. قفلنا تعديل بيانات الطلاب للأدمن فقط 🔒
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

        // 🔴 4. قفلنا حذف الطلاب للأدمن فقط 🔒
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

        // 🔴 5. قفلنا ميثود الـ list الاحتياطية للأدمن فقط 🔒
        [Authorize(Roles = "Admin")]
        [HttpPost("list")]
        public IActionResult GetStudentsPost()
        {
            var students = _context.Students.ToList();
            return Ok(students);
        }
    }
}