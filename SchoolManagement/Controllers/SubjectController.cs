using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

[Authorize] // 👈 1. شلنا التحديد من فوق عشان أي شخص عنده Token (أدمن أو مدرس) يقدر يدخل الكلاس ويقرأ
[Route("api/[controller]")]
[ApiController]
public class SubjectController : ControllerBase
{
    private readonly AppDbContext _context;

    public SubjectController(AppDbContext context)
    {
        _context = context;
    }

    // 🟢 ميثود القراءة بتضل مفتوحة للكل (طالما مسجل دخول) عشان المدرس يقدر يعبي الـ Dropdown بالجدول
    [HttpGet]
    public async Task<IActionResult> GetSubjects()
    {
        var subjects = await _context.Subjects
            .Include(s => s.Teacher)
            .Select(s => new {
                s.Id,
                s.Title,
                s.Credits,
                TeacherName = s.Teacher != null ? s.Teacher.Name : "No Teacher" // حماية اختيارية في حال الـ Teacher كان null
            })
            .ToListAsync();

        return Ok(subjects);
    }

    // 🔴 2. قفلنا الإضافة للأدمن فقط 🔒
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateSubject(Subject subject)
    {
        if (subject.TeacherId.HasValue && subject.TeacherId > 0)
        {
            var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == subject.TeacherId);
            if (!teacherExists)
            {
                return BadRequest("not available");
            }
        }
        else
        {
            subject.TeacherId = null;
        }

        _context.Subjects.Add(subject);
        await _context.SaveChangesAsync();
        return Ok(subject);
    }

    // 🔴 3. قفلنا الحذف للأدمن فقط 🔒
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null)
        {
            return NotFound();
        }

        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();

        return Ok();
    }
}