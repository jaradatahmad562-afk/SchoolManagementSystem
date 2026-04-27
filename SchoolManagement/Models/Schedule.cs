namespace SchoolManagement.Models
{
    public class Schedule
    {
        public int Id { get; set; }

        // اليوم (مثلاً: الأحد، الاثنين)
        public DayOfWeek Day { get; set; }

        // وقت الحصة (البداية والنهاية)
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        // المادة اللي رح تندرس
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }

        // القاعة أو الصف اللي رح تكون فيه الحصة
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
    }
}
