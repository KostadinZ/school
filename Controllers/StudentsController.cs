using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IActionResult> Index()
    {
        var students = await _context.Students
            .Include(student => student.Registrations)
            .OrderBy(student => student.Name)
            .ToListAsync();

        return View(students);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Email,ClassName")] Student student)
    {
        if (await _context.Students.AnyAsync(existing => existing.Email == student.Email))
        {
            ModelState.AddModelError(nameof(Student.Email), "A student with this email already exists.");
        }

        if (ModelState.IsValid)
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Student added successfully.";
            return RedirectToAction(nameof(Index));
        }

        return View(student);
    }
}
