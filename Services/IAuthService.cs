using StudentManagementApi.DTOs;

namespace StudentManagementApi.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterDto dto);

    Task<string?> LoginAsync(LoginDto dto);
}



