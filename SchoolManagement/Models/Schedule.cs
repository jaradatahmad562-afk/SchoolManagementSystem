namespace SchoolManagement.Models
{
    public class Schedule
    {
        public int Id { get; set; }

        public DayOfWeek Day { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }

        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
    }
}
