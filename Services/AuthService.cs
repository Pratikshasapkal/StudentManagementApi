using Microsoft.AspNetCore.Identity;
using StudentManagementApi.Data;
using StudentManagementApi.DTOs;
using StudentManagementApi.Models;

namespace StudentManagementApi.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _passwordHasher = new PasswordHasher<User>();
        _jwtService = jwtService;
    }

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        var existingUser = _context.Users
            .FirstOrDefault(u => u.Email == dto.Email);

        if (existingUser != null)
        {
            return false;
        }

        var user = new User
        {
            UserName = dto.Username,
            Email = dto.Email,
            Role = "User"
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            dto.Password);

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<string?> LoginAsync(LoginDto dto)
    {
        var user = _context.Users.FirstOrDefault(
        u => u.Email == dto.Email);

        if (user == null)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            dto.Password
        );

        if (result != PasswordVerificationResult.Success)
        {
            return null;
        }

        return _jwtService.GenerateToken(
            user.Id,
            user.Email,
            user.Role);
    }
}