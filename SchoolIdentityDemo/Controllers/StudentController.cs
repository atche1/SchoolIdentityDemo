using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace SchoolIdentityDemo.Controllers;
[Authorize(Roles = "Student,Admin")]
public class StudentController : Controller
{
    public IActionResult Index() => View();
}