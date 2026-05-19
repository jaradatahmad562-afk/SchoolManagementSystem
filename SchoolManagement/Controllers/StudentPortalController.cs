using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Data;

namespace SchoolManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentPortalController : ControllerBase
    {
        public StudentPortalController()
        {
        }

        [HttpGet("my-profile")]
        public IActionResult GetMyProfile()
        {
            var dummyProfile = new
            {
                fullName = "Mohammed",
                birthDate = "2004-01-03",
                classroom = "Room",
                status = "Active"
            };

            return Ok(dummyProfile);
        }

        [HttpGet("my-grades")]
        public IActionResult GetMyGrades()
        {
            var dummyGrades = new[]
            {
                new { SubjectName = "Advanced Algorithms", FirstExam = 22, SecondExam = 23, FinalExam = 45, Total = 90 },
                new { SubjectName = "Machine Learning", FirstExam = 24, SecondExam = 25, FinalExam = 48, Total = 97 },
                new { SubjectName = "Network Engineering", FirstExam = 21, SecondExam = 22, FinalExam = 42, Total = 85 }
            };

            return Ok(dummyGrades);
        }

        [HttpGet("my-schedule")]
        public IActionResult GetMySchedule()
        {
            var dummySchedule = new[]
            {
                new { DayOfWeek = "Sunday", StartTime = "08:00", EndTime = "09:30", SubjectName = "Machine Learning", TeacherName = "Dr. Husam" },
                new { DayOfWeek = "Monday", StartTime = "10:00", EndTime = "11:30", SubjectName = "Algorithms", TeacherName = "Dr. Zaid" },
                new { DayOfWeek = "Tuesday", StartTime = "12:00", EndTime = "13:30", SubjectName = "Network Security", TeacherName = "Dr. Fares" }
            };

            return Ok(dummySchedule);
        }
    }
}