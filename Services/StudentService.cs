using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Data;
using StudentManagementApi.DTOs;
using StudentManagementApi.Models;
using Microsoft.Extensions.Logging;

namespace StudentManagementApi.Services;

public class StudentService : IStudentService
{
    private readonly AppDbContext _context;
    private readonly ILogger<StudentService> _logger;

    public StudentService(
        AppDbContext context,
        ILogger<StudentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<StudentResponseDto>> GetStudentsAsync()
    {
        _logger.LogInformation("Fetching All Students.");

        return await _context.Students
            .Include(s => s.Course)
            .Select(s => new StudentResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Age = s.Age,
                CourseId = s.CourseId,
                CourseName = s.Course != null ? s.Course.Name : null
            }).ToListAsync();
    }

    public async Task<StudentResponseDto?> GetStudentByIdAsync(int id)
    {
        _logger.LogInformation("Fetching Student with id {StudentId}", id);

        return await _context.Students
              .Include(s => s.Course)
              .Where(s => s.Id == id)
              .Select(s => new StudentResponseDto
              {
                  Id = s.Id,
                  Name = s.Name,
                  Email = s.Email,
                  Age = s.Age,
                  CourseId = s.CourseId,
                  CourseName = s.Course != null ? s.Course.Name : null
              }).FirstOrDefaultAsync();
    }

    public async Task<StudentResponseDto> CreateStudentAsync(StudentCreateDto dto)
    {

        _logger.LogInformation($"Create student with Email {dto.Email})");

        var student = new Student
        {
            Name = dto.Name,
            Email = dto.Email,
            Age = dto.Age
        };

        _context.Students.Add(student);

        await _context.SaveChangesAsync();

        _logger.LogInformation(
        "Student created successfully with ID {StudentId}.",
        student.Id);

        return new StudentResponseDto
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            Age = student.Age,
            CourseId = student.CourseId,
            CourseName = null
        };
    }

    //UpdateStudentById
    public async Task<StudentResponseDto?> UpdateStudent(int id, StudentUpdateDto dto)
    {

        _logger.LogInformation("Updating student with id : {StudentId}.", id);

        var existingStudent = await _context.Students.FindAsync(id);

        if (existingStudent == null)
        {
            _logger.LogInformation("Student not found with id {StudentId}.", id);
            return null;
        }

        if (dto.CourseId.HasValue)
        {
            var course = await _context.Courses.FindAsync(dto.CourseId.Value);

            if (course == null)
            {
                return null;
            }

            _logger.LogInformation(
            "Student with ID {StudentId} updated successfully.",
            id);
            
        }

        existingStudent.Name = dto.Name;
        existingStudent.Email = dto.Email;
        existingStudent.Age = dto.Age;
        existingStudent.CourseId = dto.CourseId;

        await _context.SaveChangesAsync();

        var updatedStudent = await _context.Students
            .Include(s => s.Course)
            .Where(s => s.Id == id)
            .Select(s => new StudentResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                Age = s.Age,
                CourseId = s.CourseId,
                CourseName = s.Course != null ? s.Course.Name : null
            }).FirstOrDefaultAsync();

        return updatedStudent;
    }

    //Delete Student
    public async Task<bool> DeleteStudentById(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return false;
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return true;
    }

    //Asign course by Id 
    public async Task<StudentResponseDto?> AssignCourseAsync(
    int studentId,
    int courseId)
    {
        var student = await _context.Students.FindAsync(studentId);

        if (student == null)
        {
            return null;
        }

        var course = await _context.Courses.FindAsync(courseId);

        if (course == null)
        {
            return null;
        }

        student.CourseId = courseId;

        await _context.SaveChangesAsync();

        return new StudentResponseDto
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            Age = student.Age,
            CourseId = student.CourseId,
            CourseName = course.Name
        };
    }

    //
}