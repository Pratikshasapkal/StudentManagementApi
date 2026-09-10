using StudentManagementApi.DTOs;

namespace StudentManagementApi.Services;

public interface ICourseService
{
    Task<List<CourseResponseDto>> GetCoursesAsync();

    Task<CourseResponseDto?> GetCourseByIdAsync(int id);

    Task<CourseResponseDto> CreateCourseAsync(CourseCreateDto dto);

    Task<CourseResponseDto?> UpdateCourseAsync(
    int id,
    CourseUpdateDto dto);

    Task<List<CourseStudentDto>?> GetStudentsByCourseIdAsync(int courseId);

    Task<bool> DeleteCourseAsync(int id);
}