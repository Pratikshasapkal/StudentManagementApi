namespace StudentManagementApi.Data;
using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Models;



public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Student> Students { get; set; }
}