using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.Data;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.DTOs;
using StudentManagementApi.Models;

namespace StudentManagemntApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CoursesController(AppDbContext context)
    {
        _context = context;
    }

    //Get Courses
    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var courses = await _context.Courses.ToListAsync();

        return Ok(courses);
    }

    //Get Course By Id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseById(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    //Post new Course
    [HttpPost]
    public async Task<IActionResult> CreateCourse(CourseCreateDto dto, int studentId)
    {
        var course = new Course
        {
            Name = dto.Name,
            DurationInMonths = dto.DurationInMonths
        };

        var student = await _context.Students.FindAsync(studentId);

        if (student == null)
        {
            return NotFound("Student not found");
        }


        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCourseById),
            new {id = course.Id},
            course
        );
    }

    //Get students for a specific course
    [HttpGet("{courseId}/students")]
    public async Task<IActionResult> GetStudentsByCourseId(int courseId)
    {
        var course = await _context.Courses.FindAsync(courseId);

        if (course == null)
        {
            return NotFound();
        }

        var students = await _context.Students
            .Where(s => s.CourseId == courseId)
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.Email,
                s.Age,
                s.CourseId
            })
            .ToListAsync();

        return Ok(students);
    }
}