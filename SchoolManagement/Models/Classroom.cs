using SchoolManagement.Models;

public class Classroom
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int Capacity { get; set; }
    public string Status { get; set; } = "Available";

    public ICollection<Student> Students { get; set; } = new List<Student>();
}