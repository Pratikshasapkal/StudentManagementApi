using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Data;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.DTOs;
using StudentManagementApi.Models;
using StudentManagementApi.Services;

namespace StudentManagemntApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    //Get Courses
    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _courseService.GetCoursesAsync();
        return Ok(courses);
    }

    //Get Course By Id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseById(int id)
    {
        var courses = await _courseService.GetCourseByIdAsync(id);

        if (courses == null)
        {
            return NotFound("Course Not Found");
        }

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
    public async Task<IActionResult> DeleteCourseById(int id)
    {
        var deleted = await _courseService.DeleteCourseAsync(id);

        if (!deleted)
        {
            return NotFound("Course not found");
        }

        return NoContent();
    }
}