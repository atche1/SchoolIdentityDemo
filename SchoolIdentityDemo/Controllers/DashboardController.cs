using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolIdentityDemo.Data;
using SchoolIdentityDemo.Models.ViewModels;

namespace SchoolIdentityDemo.Controllers;

[Authorize]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public DashboardController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User)!;
        var email = User.Identity?.Name ?? "";

        var vm = new DashboardViewModel { Email = email };

        if (User.IsInRole("Admin"))
        {
            vm.Role = "Admin";
            return View(vm);
        }

        if (User.IsInRole("Teacher"))
        {
            vm.Role = "Teacher";
            vm.TeacherProfile = await _db.TeacherProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
            return View(vm);
        }

        vm.Role = "Student";
        vm.StudentProfile = await _db.StudentProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        return View(vm);
    }
}
