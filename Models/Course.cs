namespace StudentManagementApi.Models;

public class Course
{
    public int Id{get; set;}

    public string Name{get; set;} = string.Empty;

    public int DurationInMonths{get; set;}

    public List<Student> Students{get; set;} = new List<Student>();
}