using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Models;
using StudentManagementApi.Data;
using StudentManagementApi.DTOs;
using StudentManagementApi.Services;

namespace StudentManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var students = await _studentService.GetStudentsAsync();

        return Ok(students);
    }

    // Get student by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);

        if (student == null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    // Post request to add a new student
    [HttpPost]
    public async Task<IActionResult> CreateStudent(StudentCreateDto dto)
    {
        var student = await _studentService.CreateStudentAsync(dto);

        return CreatedAtAction(
            nameof(GetStudentById),
            new { id = student.Id },
            student
        );

    }


    //Delete student By Id
    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteStudentById(int id)
    {
        var delete = await _studentService.DeleteStudentById(id);

        if (!delete)
        {
            return NotFound("Student Not Found");
        }

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