using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Data;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.DTOs;
using StudentManagementApi.Models;
using StudentManagementApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace StudentManagemntApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(ICourseService courseService, ILogger<CoursesController> logger)
    {
        _courseService = courseService;
        _logger = logger;
    }

    //Get Courses
    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        _logger.LogInformation("Fetching Courses...");
        var courses = await _courseService.GetCoursesAsync();
        return Ok(courses);
    }

    //Get Course By Id
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCourseById(int id)
    {
        _logger.LogInformation("Fetching Courses with {Id} ", id);
        var courses = await _courseService.GetCourseByIdAsync(id);

        if (courses == null)
        {
            _logger.LogWarning("Course with id {Id} not found", id);
            return NotFound("Course Not Found");
        }

        _logger.LogInformation("Fetched student withh id {Id}", id);
        return Ok(courses);
    }

    //Post new Course
    [HttpPost]
    public async Task<IActionResult> CreateCourse(CourseCreateDto dto)
    {
        var course = await _courseService.CreateCourseAsync(dto);

        return CreatedAtAction(
            nameof(GetCourseById),
            new { id = course.Id },
            course
        );
    }

    //Get students for a specific course
    [HttpGet("{courseId}/students")]
    public async Task<IActionResult> GetStudentsByCourseId(int courseId)
    {

        var students = await _courseService
            .GetStudentsByCourseIdAsync(courseId);

        if (students == null)
        {
            return NotFound("Course Not Found");
        }

        return Ok(students);
    }

    //Put/Update Course
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourseById(
        int id,
        CourseUpdateDto dto)
    {
        var course = await _courseService.UpdateCourseAsync(id, dto);

        if (course == null)
        {
            return NotFound("Course Not Found");
        }

        return Ok(course);
    }

    //Delete courseConfirmDelete(int id)
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCourseById(int id)
    {
        var deleted = await _courseService.DeleteCourseAsync(id);

        if (!deleted)
        {
            _logger.LogError("Something went wrong");
            return NotFound("Course not found");
        }

        return NoContent();
    }
}