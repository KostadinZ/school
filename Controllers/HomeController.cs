using Microsoft.AspNetCore.Mvc;

namespace SchoolEventRegistration.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Events");
    }

    public IActionResult Error()
    {
        return View();
    }
}
