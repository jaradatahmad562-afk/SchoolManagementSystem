namespace SchoolManagement.Models
{
    public class Enrollment
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public decimal? Grade { get; set; }

        public DateTime ExamDate { get; set; }
    }
}
