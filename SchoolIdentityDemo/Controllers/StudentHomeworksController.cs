using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolIdentityDemo.Data;
using SchoolIdentityDemo.Models;

namespace SchoolIdentityDemo.Controllers;

[Authorize(Roles = "Student,Admin")]
public class StudentHomeworksController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public StudentHomeworksController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _db.Homeworks
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync();

        return View(list);
    }

    public async Task<IActionResult> Details(int id)
    {
        var hw = await _db.Homeworks.FirstOrDefaultAsync(h => h.Id == id);
        if (hw == null) return NotFound();

        var studentId = _userManager.GetUserId(User)!;

        var submission = await _db.HomeworkSubmissions
            .FirstOrDefaultAsync(s => s.HomeworkId == id && s.StudentId == studentId);

        ViewBag.Submission = submission;
        return View(hw);
    }

    public async Task<IActionResult> Submit(int homeworkId)
    {
        var hw = await _db.Homeworks.FindAsync(homeworkId);
        if (hw == null) return NotFound();

        var studentId = _userManager.GetUserId(User)!;

        var existing = await _db.HomeworkSubmissions
            .FirstOrDefaultAsync(s => s.HomeworkId == homeworkId && s.StudentId == studentId);

        if (existing != null)
            return RedirectToAction(nameof(EditSubmission), new { id = existing.Id });

        return View(new HomeworkSubmission { HomeworkId = homeworkId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(HomeworkSubmission model)
    {
        model.StudentId = _userManager.GetUserId(User)!;
        model.SubmittedAt = DateTime.UtcNow;
        ModelState.Remove(nameof(HomeworkSubmission.StudentId));
        ModelState.Remove(nameof(HomeworkSubmission.SubmittedAt));
        if (!ModelState.IsValid) return View(model);

        model.StudentId = _userManager.GetUserId(User)!;
        model.SubmittedAt = DateTime.UtcNow;

        _db.HomeworkSubmissions.Add(model);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = model.HomeworkId });
    }

    public async Task<IActionResult> EditSubmission(int id)
    {
        var sub = await _db.HomeworkSubmissions.FindAsync(id);
        if (sub == null) return NotFound();

        var studentId = _userManager.GetUserId(User)!;
        if (!User.IsInRole("Admin") && sub.StudentId != studentId) return Forbid();

        return View(sub);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditSubmission(HomeworkSubmission model)
    {
        var sub = await _db.HomeworkSubmissions.FindAsync(model.Id);
        if (sub == null) return NotFound();

        var studentId = _userManager.GetUserId(User)!;
        if (!User.IsInRole("Admin") && sub.StudentId != studentId) return Forbid();

        if (!ModelState.IsValid) return View(model);

        sub.Content = model.Content;
        sub.SubmittedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = sub.HomeworkId });
    }
}
