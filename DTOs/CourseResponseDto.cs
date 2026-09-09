namespace StudentManagementApi.DTOs;

public class CourseResponseDto
{
    public int Id {get; set;}
    public string Name {get; set;}  = string.Empty;
    public int DurationInMonths{get; set;}

    public List<CourseStudentDto> Students {get; set;} = new List<CourseStudentDto>();
}

public class CourseStudentDto
{
    public int Id {get; set;}
    public string Name {get; set;} = string.Empty;
}