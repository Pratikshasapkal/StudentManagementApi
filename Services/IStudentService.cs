using StudentManagementApi.DTOs;

namespace StudentManagementApi.Services;

public interface IStudentService
{
    Task<List<StudentResponseDto>> GetStudentsAsync();

    Task<StudentResponseDto?> GetStudentByIdAsync(int id);

    Task<StudentResponseDto> CreateStudentAsync(StudentCreateDto dto);

    Task<StudentResponseDto?> UpdateStudent(int id, StudentUpdateDto dto);

    Task<bool> DeleteStudentById(int id);

    Task<StudentResponseDto?> AssignCourseAsync(
         int studentId,
         int courseId);
}


