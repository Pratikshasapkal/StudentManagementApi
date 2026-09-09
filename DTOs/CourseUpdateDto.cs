namespace StudentManagementApi.DTOs;

using System.ComponentModel.DataAnnotations;

public class CourseUpdateDto
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 60)]
    public int DurationInMonths { get; set; }
}
