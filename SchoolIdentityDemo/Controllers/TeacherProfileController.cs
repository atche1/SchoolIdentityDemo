using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolIdentityDemo.Data;
using SchoolIdentityDemo.Models;

namespace SchoolIdentityDemo.Controllers;

[Authorize(Roles = "Teacher,Admin")]
public class TeacherProfileController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public TeacherProfileController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> EditMy()
    {
        var userId = _userManager.GetUserId(User)!;

        var profile = await _db.TeacherProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null)
        {
            profile = new TeacherProfile { UserId = userId, Subject = "", Room = "" };
            _db.TeacherProfiles.Add(profile);
            await _db.SaveChangesAsync();
        }

        return View(profile);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditMy(TeacherProfile model)
    {
        var userId = _userManager.GetUserId(User)!;
        if (model.UserId != userId) return Forbid();

        if (!ModelState.IsValid) return View(model);

        _db.TeacherProfiles.Update(model);
        await _db.SaveChangesAsync();

        TempData["msg"] = "Профилът е записан.";
        return RedirectToAction(nameof(EditMy));
    }
}
