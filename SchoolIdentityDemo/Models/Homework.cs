using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SchoolIdentityDemo.Models;

public class Homework
{
    public int Id { get; set; }
    [Required, MaxLength(120)]
    public string Title { get; set; } = "";

    [Required, MaxLength(2000)]
    public string Description { get; set; } = "";

    [Required]
    public DateTime DueDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public string TeacherId { get; set; } = "";

    public IdentityUser? Teacher { get; set; }

    public List<HomeworkSubmission> Submissions { get; set; } = new();
}
