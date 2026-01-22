using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolIdentityDemo.Data;
using SchoolIdentityDemo.Models;

namespace SchoolIdentityDemo.Controllers;

[Authorize(Roles = "Teacher,Admin")]
public class TeacherHomeworksController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public TeacherHomeworksController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var teacherId = _userManager.GetUserId(User)!;

        var list = await _db.Homeworks
            .Where(h => User.IsInRole("Admin") || h.TeacherId == teacherId)
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync();

        return View(list);
    }

    public IActionResult Create()
        => View(new Homework { DueDate = DateTime.Today.AddDays(7) });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Homework model)
    {
        model.TeacherId = _userManager.GetUserId(User)!;
        model.CreatedAt = DateTime.UtcNow;
        ModelState.Remove(nameof(Homework.TeacherId));
        ModelState.Remove(nameof(Homework.CreatedAt));
        if (!ModelState.IsValid)
            return View(model);
        _db.Homeworks.Add(model);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var hw = await _db.Homeworks.FindAsync(id);
        if (hw == null) return NotFound();

        var teacherId = _userManager.GetUserId(User)!;
        if (!User.IsInRole("Admin") && hw.TeacherId != teacherId) return Forbid();

        return View(hw);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Homework model)
    {
        var hw = await _db.Homeworks.FindAsync(model.Id);
        if (hw == null) return NotFound();

        var teacherId = _userManager.GetUserId(User)!;
        if (!User.IsInRole("Admin") && hw.TeacherId != teacherId) return Forbid();

        if (!ModelState.IsValid) return View(model);

        hw.Title = model.Title;
        hw.Description = model.Description;
        hw.DueDate = model.DueDate;

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var hw = await _db.Homeworks.FindAsync(id);
        if (hw == null) return NotFound();

        var teacherId = _userManager.GetUserId(User)!;
        if (!User.IsInRole("Admin") && hw.TeacherId != teacherId) return Forbid();

        return View(hw);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var hw = await _db.Homeworks.FindAsync(id);
        if (hw == null) return NotFound();

        var teacherId = _userManager.GetUserId(User)!;
        if (!User.IsInRole("Admin") && hw.TeacherId != teacherId) return Forbid();

        _db.Homeworks.Remove(hw);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
