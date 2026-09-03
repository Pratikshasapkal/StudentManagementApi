namespace StudentManagementApi.DTOs;

public class StudentResponseDto
{
    public int Id { get; set;}
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }
    public int? CourseId { get; set; }
    public string? CourseName { get; set; } = string.Empty;
}