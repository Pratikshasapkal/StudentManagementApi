namespace StudentManagementApi.Data;

using Microsoft.EntityFrameworkCore;
using StudentManagementApi.Models;


public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    // AppDbContext
    // │
    // ├── DbSet<Student> → Students table
    // │
    // └── DbSet<Course>  → Courses table
    // │
    // └── DbSet<User>  → Users table
    public DbSet<Student> Students { get; set; }

    public DbSet<Course> Courses { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.Email)
            .IsUnique();
    }
}