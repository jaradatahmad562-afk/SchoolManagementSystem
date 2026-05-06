using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Data;
using SchoolManagement.Models;
using static SchoolManagement.Models.Dto;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class EnrollmentController : ControllerBase
{
    private readonly AppDbContext _context;

    public EnrollmentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> EnrollStudent(CreateEnrollmentDto dto)
    {
        var exists = await _context.Enrollments
            .AnyAsync(e => e.StudentId == dto.StudentId && e.SubjectId == dto.SubjectId);

        if (exists) return BadRequest("");

        var enrollment = new Enrollment
        {
            StudentId = dto.StudentId,
            SubjectId = dto.SubjectId,
            ExamDate = DateTime.Now 
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        return Ok("Done");
    }

    [HttpPatch("update-grade")]
    public async Task<IActionResult> UpdateGrade(UpdateGradeDto dto)
    {
        var record = await _context.Enrollments.FindAsync(dto.StudentId, dto.SubjectId);

        if (record == null) return NotFound("سجل التسجيل غير موجود.");

        record.Grade = dto.Grade;
        record.ExamDate = DateTime.Now; 

        await _context.SaveChangesAsync();
        return Ok("تم رصد العلامة بنجاح.");
    }

    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetStudentGrades(int studentId)
    {
        var grades = await _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .Include(e => e.Subject) 
            .Select(e => new {
                SubjectId = e.SubjectId,
                SubjectName = e.Subject.Title,
                Grade = e.Grade,
                Date = e.ExamDate
            })
            .ToListAsync();

        return Ok(grades);
    }

    [HttpDelete("{studentId}/{subjectId}")]
    public async Task<IActionResult> DeleteEnrollment(int studentId, int subjectId)
    {
        var enrollment = await _context.Enrollments.FindAsync(studentId, subjectId);

        if (enrollment == null) return NotFound();

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}