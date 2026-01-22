using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolIdentityDemo.Data;
using SchoolIdentityDemo.Models;

namespace SchoolIdentityDemo.Controllers;

[Authorize(Roles = "Student,Admin")]
public class StudentProfileController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public StudentProfileController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> EditMy()
    {
        var userId = _userManager.GetUserId(User)!;

        var profile = await _db.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null)
        {
            profile = new StudentProfile { UserId = userId, Grade = 8, NumberInClass = 1, Specialty = "" };
            _db.StudentProfiles.Add(profile);
            await _db.SaveChangesAsync();
        }

        return View(profile);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditMy(StudentProfile model)
    {
        var userId = _userManager.GetUserId(User)!;
        if (model.UserId != userId) return Forbid();

        if (!ModelState.IsValid) return View(model);
        _db.StudentProfiles.Update(model);
        await _db.SaveChangesAsync();

        TempData["msg"] = "Профилът е записан.";
        return RedirectToAction(nameof(EditMy));
    }
}
