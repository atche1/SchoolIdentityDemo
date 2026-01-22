using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SchoolIdentityDemo.Models;

public class HomeworkSubmission
{
    public int Id { get; set; }

    [Required]
    public int HomeworkId { get; set; }

    public Homework? Homework { get; set; }

    [Required]
    public string StudentId { get; set; } = "";

    public IdentityUser? Student { get; set; }

    [Required, MaxLength(5000)]
    public string Content { get; set; } = "";

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    [Range(2,6,ErrorMessage ="The grade must be between 2 and 6")]
    public int? Grade { get; set; }

    [MaxLength(1000)]
    public string? Feedback { get; set; }
}

