using SchoolIdentityDemo.Models;

namespace SchoolIdentityDemo.Models.ViewModels;

public class DashboardViewModel
{
    public string Role { get; set; } = "Student";
    public StudentProfile? StudentProfile { get; set; }
    public TeacherProfile? TeacherProfile { get; set; }
    public string Email { get; set; } = "";
}
