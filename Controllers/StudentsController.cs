using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StudentManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetStudents()
    {
        return Ok(new[]
        {
        new
        {
            Id = 1,
            Name = "Pratiksha",
            Age = 22
        },
        new
        {
            Id = 2,
            Name = "Rahul",
            Age = 23
        }
    });
    }
}