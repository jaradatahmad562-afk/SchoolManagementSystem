using Microsoft.AspNetCore.Mvc;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentPortalController : ControllerBase
    {
        [HttpGet("my-profile")]
        public IActionResult GetMyProfile()
        {
            var profile = new
            {
                name = "Ahmad",
                birthDate = "2004-01-03",
                status = "ACTIVE",
                classroomName = "Room1"
            };
            return Ok(profile);
        }

        [HttpGet("my-grades")]
        public IActionResult GetMyGrades()
        {
            var grades = new[] {
                new { subjectName = "Artificial Intelligence", grade = 95 },
                new { subjectName = "Web Development", grade = 92 },
                new { subjectName = "Network Engineering", grade = 88 }
            };
            return Ok(grades);
        }

        [HttpGet("my-schedule")]
        public IActionResult GetMySchedule()
        {
            var schedule = new[] {
                new { subjectName = "Artificial Intelligence", teacherName = "Dr. Mohammad" },
                new { subjectName = "Web Development", teacherName = "Eng. Fares" }
            };
            return Ok(schedule);
        }
    }
}