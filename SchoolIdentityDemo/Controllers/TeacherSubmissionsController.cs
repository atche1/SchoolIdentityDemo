using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolIdentityDemo.Data;
using SchoolIdentityDemo.Models;

namespace SchoolIdentityDemo.Controllers;

[Authorize(Roles = "Teacher,Admin")]
public class TeacherSubmissionsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public TeacherSubmissionsController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> ForHomework(int homeworkId)
    {
        var hw = await _db.Homeworks.FindAsync(homeworkId);
        if (hw == null) return NotFound();

        var teacherId = _userManager.GetUserId(User)!;
        if (!User.IsInRole("Admin") && hw.TeacherId != teacherId) return Forbid();

        ViewBag.HomeworkTitle = hw.Title;

        var list = await _db.HomeworkSubmissions
            .Where(s => s.HomeworkId == homeworkId)
            .Include(s => s.Student)
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync();

        return View(list);
    }

    public async Task<IActionResult> Grade(int id)
    {
        var sub = await _db.HomeworkSubmissions
            .Include(s => s.Homework)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sub == null) return NotFound();

        var teacherId = _userManager.GetUserId(User)!;
        if (!User.IsInRole("Admin") && sub.Homework!.TeacherId != teacherId) return Forbid();

        return View(sub);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Grade(HomeworkSubmission model)
    {
        var sub = await _db.HomeworkSubmissions
            .Include(s => s.Homework)
            .FirstOrDefaultAsync(s => s.Id == model.Id);

        if (sub == null) return NotFound();

        var teacherId = _userManager.GetUserId(User)!;
        if (!User.IsInRole("Admin") && sub.Homework!.TeacherId != teacherId) return Forbid();

        sub.Grade = model.Grade;
        sub.Feedback = model.Feedback;

        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(ForHomework), new { homeworkId = sub.HomeworkId });
    }
}
