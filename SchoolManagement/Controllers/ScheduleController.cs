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
        public async Task<ActionResult<Schedule>> PostSchedule(Schedule newSchedule)
        {
            var isRoomBusy = await _context.Schedules.AnyAsync(s =>
                s.ClassroomId == newSchedule.ClassroomId &&
                s.Day == newSchedule.Day &&
                ((newSchedule.StartTime < s.EndTime && newSchedule.EndTime > s.StartTime)));

            if (isRoomBusy)
            {
                return BadRequest("not available");
            }

            var subject = await _context.Subjects.FindAsync(newSchedule.SubjectId);
            if (subject == null) return BadRequest("not available");

            var isTeacherBusy = await _context.Schedules
                .Include(s => s.Subject)
                .AnyAsync(s =>
                    s.Subject.TeacherId == subject.TeacherId &&
                    s.Day == newSchedule.Day &&
                    ((newSchedule.StartTime < s.EndTime && newSchedule.EndTime > s.StartTime)));

            if (isTeacherBusy)
            {
                return BadRequest("not available");
            }

            _context.Schedules.Add(newSchedule);
            await _context.SaveChangesAsync();

            return Ok(new {  data = newSchedule });
        }
    }
}