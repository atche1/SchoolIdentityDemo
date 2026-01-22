namespace SchoolIdentityDemo.Models.ViewModels;

public class EditUserRoleViewModel
{
    public string UserId { get; set; } = "";
    public string Email { get; set; } = "";
    public string SelectedRole { get; set; } = "Student";
    public List<string> Roles { get; set; } = new();
}