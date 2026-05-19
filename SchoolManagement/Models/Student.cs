using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public int ClassroomId { get; set; }
        public Classroom? Classroom { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

        public string Status { get; set; } = "Active";


        [NotMapped] 
        public string Email { get; set; } = string.Empty;

        [NotMapped] 
        public string Password { get; set; } = string.Empty;

        public int? UserId { get; set; } 
    }
}