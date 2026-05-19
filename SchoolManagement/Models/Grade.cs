using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models
{
    public class Grade
    {
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        [Required]
        public int SubjectId { get; set; }

        [ForeignKey("SubjectId")]
        public Subject? Subject { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 20)]
        public decimal FirstExam { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 20)]
        public decimal SecondExam { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 60)]
        public decimal FinalExam { get; set; } = 0;
    }
}