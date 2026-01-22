using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SchoolIdentityDemo.Models;

public class TeacherProfile
{
    [Key]
    public string UserId { get; set; } = "";

    public IdentityUser? User { get; set; }

    [MaxLength(80)]
    public string Subject { get; set; } = "";

    [MaxLength(20)]
    public string Room { get; set; } = "";
}
