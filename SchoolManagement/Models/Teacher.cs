namespace SchoolManagement.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;

        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();


        public string Email { get; set; }
        public string Password { get; set; }
    }
}
