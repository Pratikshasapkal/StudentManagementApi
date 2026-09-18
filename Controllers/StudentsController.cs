using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Models;
using StudentManagementApi.Data;
using StudentManagementApi.DTOs;
using StudentManagementApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace StudentManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    //Get All students
    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        _logger.LogInformation("Fetching Students");
        var students = await _studentService.GetStudentsAsync();

        return Ok(students);
    }

    // Get student by ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        _logger.LogInformation("Fetching Students with Id {Id}", id);
        var student = await _studentService.GetStudentByIdAsync(id);

        if (student == null)
        {
            _logger.LogWarning("Student with ID {Id} not found", id);
            return NotFound();
        }

        _logger.LogInformation("Student with ID {StudentId} retrieved successfully", id);
        return Ok(student);
    }

    // Post request to add a new student
    [HttpPost]
    public async Task<IActionResult> CreateStudent(StudentCreateDto dto)
    {

        var student = await _studentService.CreateStudentAsync(dto);

        if (student is null)
        {
            return Conflict("A student with this email already exists.");
        }

        _logger.LogInformation("Student with ID {StudentId} created successfully", student.Id);

        return CreatedAtAction(
            nameof(GetStudentById),
            new { id = student.Id },
            student
        );

    }


    //Delete student By Id
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteStudentById(int id)
    {
        var delete = await _studentService.DeleteStudentById(id);

        if (!delete)
        {
            _logger.LogError("Something went wrong");
            return NotFound("Student Not Found");
        }

        _logger.LogInformation(
            "Cannot delete student with ID {StudentId}: student not found",
            id);
        return NoContent();
    }

    //Put/Update Student
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudentById(int id, StudentUpdateDto dto)
    {
        var student = await _studentService.UpdateStudent(id, dto);

        if (student == null)
        {
            return NotFound("Student Not Found");
        }

        return Ok(student);
    }

    //Asigning course to student
    [HttpPut("{studentId}/course/{courseId}")]
    public async Task<IActionResult> AssignCourse(
        int studentId,
        int courseId)
    {
        var student = await _studentService.AssignCourseAsync(
            studentId,
            courseId);

        if (student == null)
        {
            return NotFound("Student or Course not found");
        }

        return Ok(student);
    }

}