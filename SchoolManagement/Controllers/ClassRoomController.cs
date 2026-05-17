using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [Authorize] // 👈 1. شلنا التعليق عشان السيستم يصير محمي تماماً ومفتوح لأي حد معه توكن
    [Route("api/[controller]")]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClassroomController(AppDbContext context)
        {
            _context = context;
        }

        // 🟢 ميثود القراءة بتضل مفتوحة لأي حد مسجل دخول عشان المدرس يقرأ الغرف ويعبي الـ Dropdown بجدوله
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Classroom>>> GetClassrooms()
        {
            return await _context.Classrooms.ToListAsync();
        }

        // 🔴 2. قفلنا إضافة غرفة صفية جديدة للأدمن فقط 🔒
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Classroom>> CreateClassroom(Classroom classroom)
        {
            _context.Classrooms.Add(classroom);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClassrooms), new { id = classroom.Id }, classroom);
        }

        // 🔴 3. قفلنا تعديل الغرف الصفية للأدمن فقط 🔒
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClassroom(int id, Classroom classroom)
        {
            if (id != classroom.Id)
            {
                return BadRequest("ID mismatch");
            }

            _context.Entry(classroom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassroomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // 🔴 4. قفلنا حذف الغرف الصفية للأدمن فقط 🔒
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            var classroom = await _context.Classrooms.FindAsync(id);
            if (classroom == null) return NotFound("not available");

            _context.Remove(classroom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassroomExists(int id)
        {
            return _context.Classrooms.Any(e => e.Id == id);
        }
    }
}