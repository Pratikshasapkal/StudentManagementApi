using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Data;
using StudentManagementApi.DTOs;
using StudentManagementApi.Models;

namespace StudentManagementApi.Services;



public class CourseService
{
    private readonly AppDbContext _context;
    public CourseService(AppDbContext context)
    {
        _context = context;
    }

    //Get Courses
    public async Task<List<CourseResponseDto>> GetCoursesAsync()
    {
        return await _context.Courses
            .Include(c => c.Students)
            .Select(c => new CourseResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                DurationInMonths = c.DurationInMonths,
                Students = c.Students.Select(s => new CourseStudentDto
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList()
            })
            .ToListAsync();
    }

    //Get Courses by Id
    public async Task<CourseResponseDto?> GetCourseByIdAsync(int id)
    {
        return await _context.Courses
            .Include(c => c.Students)
            .Where(c => c.Id == id)
            .Select(c => new CourseResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                DurationInMonths = c.DurationInMonths,
                Students = c.Students.Select(s => new CourseStudentDto
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList()
            }).FirstOrDefaultAsync();
    }

    //Create Course
    public async Task<CourseResponseDto> CreateCourseAsync(CourseCreateDto dto)
    {
        var course = new Course
        {
            Name = dto.Name,
            DurationInMonths = dto.DurationInMonths
        };

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();

        return new CourseResponseDto
        {
            Id = course.Id,
            Name = course.Name,
            DurationInMonths = course.DurationInMonths,
            Students = new List<CourseStudentDto>()
        };

    }

    //Update Course
    public async Task<CourseResponseDto?> UpdateCourseAsync(
    int id,
    CourseUpdateDto dto)
    {
        var existingCourse = await _context.Courses.FindAsync(id);

        if (existingCourse == null)
        {
            return null;
        }

        existingCourse.Name = dto.Name;
        existingCourse.DurationInMonths = dto.DurationInMonths;

        await _context.SaveChangesAsync();

        var updatedCourse = await _context.Courses
            .Include(c => c.Students)
            .Where(c => c.Id == id)
            .Select(c => new CourseResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                DurationInMonths = c.DurationInMonths,
                Students = c.Students.Select(s => new CourseStudentDto
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList()
            })
            .FirstOrDefaultAsync();

        return updatedCourse;
    }

    //Get Student by Course Id
    public async Task<List<CourseStudentDto>?> GetStudentsByCourseIdAsync(int courseId)
    {
        var courseExists = await _context.Courses
            .AnyAsync(c => c.Id == courseId);

        if (!courseExists)
        {
            return null;
        }

        return await _context.Students
            .Where(s => s.CourseId == courseId)
            .Select(s => new CourseStudentDto
            {
                Id = s.Id,
                Name = s.Name
            })
            .ToListAsync();
    }

    //Delete Course
    public async Task<bool> DeleteCourseAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course == null)
        {
            return false;
        }

        _context.Courses.Remove(course);

        await _context.SaveChangesAsync();

        return true;
    }
}