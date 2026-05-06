using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        

        public DateTime BirthDate { get; set; }

        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public string Status { get; set; } = "Active"; 
    }
}
