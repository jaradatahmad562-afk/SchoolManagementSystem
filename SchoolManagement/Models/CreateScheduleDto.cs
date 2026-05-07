namespace SchoolManagement.Models
{
    public class CreateScheduleDto
    {
        public int Day { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public int SubjectId { get; set; }
        public int ClassroomId { get; set; }
    }
}
