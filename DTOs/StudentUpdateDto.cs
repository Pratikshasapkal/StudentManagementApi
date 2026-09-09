namespace StudentManagementApi.DTOs;

using System.ComponentModel.DataAnnotations;


public class StudentUpdateDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Range(18, 60)]
    public int Age { get; set; }
    public int? CourseId { get; set; }
}