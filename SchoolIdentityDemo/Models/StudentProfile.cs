using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SchoolIdentityDemo.Models;

public class StudentProfile
{
    [Key]
    public string UserId { get; set; } = "";

    public IdentityUser? User { get; set; }

    [Range(1, 12)]
    public int Grade { get; set; }

    [Range(1, 40)]
    public int NumberInClass { get; set; }
    [MaxLength(80)]
    public string Specialty { get; set; } = "";
}
