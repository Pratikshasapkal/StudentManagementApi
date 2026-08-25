using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi.DTOs;

public class StudentCreateDto
{
    [Required]
    [StringLength(100)]
    public string Name {get; set;} = string.Empty;

    [Required]
    [EmailAddress]
    public string Email {get; set;} = string.Empty;

    [Required]
    [Range(0, 150)]
    public int Age {get; set;}
}