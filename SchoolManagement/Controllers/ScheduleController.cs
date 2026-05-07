using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ScheduleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetSchedules()
        {
            return await _context.Schedules
                .Include(s => s.Subject)
                .Include(s => s.Classroom)
                .Select(s => new
                {
                    s.Id,
                    s.Day,
                    Time = $"{s.StartTime:hh\\:mm} - {s.EndTime:hh\\:mm}",
                    SubjectName = s.Subject.Title,
                    RoomName = s.Classroom.Name
                })
                .ToListAsync();
        }
        [HttpPost]
        public async Task<IActionResult> PostSchedule([FromBody] CreateScheduleDto dto)
        {
            var subject = await _context.Subjects.FindAsync(dto.SubjectId);
            if (subject == null) return BadRequest("Subject not found");

            var schedule = new Schedule
            {
                Day = (DayOfWeek)dto.Day,
                StartTime = TimeSpan.Parse(dto.StartTime),
                EndTime = TimeSpan.Parse(dto.EndTime),
                SubjectId = dto.SubjectId,
                ClassroomId = dto.ClassroomId
            };

            var conflict = await _context.Schedules.AnyAsync(s =>
                s.ClassroomId == schedule.ClassroomId &&
                s.Day == schedule.Day &&
                (schedule.StartTime < s.EndTime &&
                 schedule.EndTime > s.StartTime)
            );

            if (conflict)
                return BadRequest("Conflict");

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            return Ok(schedule);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);

            if (schedule == null) return NotFound();

            _context.Schedules.Remove(schedule);

            await _context.SaveChangesAsync();

            return NoContent(); 
        }
    }
}