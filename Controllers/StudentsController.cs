using Microsoft.AspNetCore.Mvc;
using SchoolEventRegistration.Data;
using SchoolEventRegistration.Models;

namespace SchoolEventRegistration.Controllers;

public class StudentsController : Controller
{
    private readonly SchoolEventContext _context;

    public StudentsController(SchoolEventContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View(_context.Students.ToList());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Student student)
    {
        if (ModelState.IsValid)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(student);
    }
}
