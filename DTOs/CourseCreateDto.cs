using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi.DTOs;

public class CourseCreateDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public int DurationInMonths { get; set; }
}