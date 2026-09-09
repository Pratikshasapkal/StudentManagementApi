using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Data;
using StudentManagementApi.DTOs;
using StudentManagementApi.Models;

namespace StudentManagementApi.Services;

public class StudentService
{
    private readonly AppDbContext _context;

    public StudentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<StudentResponseDto>> GetStudentsAsync()
    {
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
        var student = new Student
        {
            Name = dto.Name,
            Email = dto.Email,
            Age = dto.Age
        };

        _context.Students.Add(student);

        await _context.SaveChangesAsync();

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
        var existingStudent = await _context.Students.FindAsync(id);

        if (existingStudent == null)
        {
            return null;
        }

        if (dto.CourseId.HasValue)
        {
            var course = await _context.Courses.FindAsync(dto.CourseId.Value);

            if (course == null)
            {
                return null;
            }
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

        if(student == null)
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