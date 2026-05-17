using System.Collections.Generic;

namespace SchoolManagement.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();

        public int? UserId { get; set; }

        public User? User { get; set; }
    }
}
public class TeacherCreationDto
{
    public string Name { get; set; }
    public string Subject { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}