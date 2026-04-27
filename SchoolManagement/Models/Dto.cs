namespace SchoolManagement.Models
{
    public class Dto
    {
        public class CreateEnrollmentDto
        {
            public int StudentId { get; set; }
            public int SubjectId { get; set; }
        }
        public class UpdateGradeDto
        {
            public int StudentId { get; set; }
            public int SubjectId { get; set; }
            public decimal Grade { get; set; }
        }
    }
}
