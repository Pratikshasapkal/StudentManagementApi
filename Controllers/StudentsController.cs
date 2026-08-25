using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Models;
using StudentManagementApi.Data;
using StudentManagementApi.DTOs;

namespace StudentManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var students = await _context.Students.ToListAsync();

        return Ok(students);
    }

    // Get student by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    // Post request to add a new student
    [HttpPost]
    public async Task<IActionResult> Createstudent(StudentCreateDto dto)
    {
        var student = new Student
        {
            Name = dto.Name,
            Email = dto.Email,
            Age = dto.Age
        };

        _context.Students.Add(student);

        await _context.SaveChangesAsync();

        return Ok(student);
    }


    //Delete student By Id
    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteStudentById(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if(student == null)
        {
            return NotFound();
        }

        _context.Students.Remove(student);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    //Put/Update Student
    [HttpPut("{id}")]

    public async Task<IActionResult> UpdateStudentById(int id, Student student)
    {
        var existingStudent = await _context.Students.FindAsync(id);

        if(existingStudent == null)
        {
            return NotFound();
        }

        existingStudent.Name = student.Name;
        existingStudent.Email = student.Email;
        existingStudent.Age = student.Age;

        await _context.SaveChangesAsync();

        return Ok(existingStudent);
    }
}