using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolVotingSystem.Data;

namespace SchoolVotingSystem.Controllers;

public class HomeController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var election = await context.Elections
            .AsNoTracking()
            .OrderByDescending(e => e.Id)
            .FirstOrDefaultAsync();

        return View(election);
    }
}
