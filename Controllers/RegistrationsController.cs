using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolEventRegistration.Data;
using SchoolEventRegistration.Models;

namespace SchoolEventRegistration.Controllers;

public class RegistrationsController : Controller
{
    private readonly SchoolEventContext _context;

    public RegistrationsController(SchoolEventContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var registrations = await _context.Registrations
            .Include(r => r.Event)
            .Include(r => r.Student)
            .ToListAsync();

        return View(registrations);
    }

    public async Task<IActionResult> Create()
    {
        await FillDropDowns();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Registration registration)
    {
        var schoolEvent = await _context.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == registration.EventId);

        if (schoolEvent == null)
        {
            ModelState.AddModelError("", "Please choose an event.");
        }
        else if (schoolEvent.Registrations.Count >= schoolEvent.MaxParticipants)
        {
            ModelState.AddModelError("", "This event is full.");
        }

        bool alreadyRegistered = await _context.Registrations.AnyAsync(r =>
            r.EventId == registration.EventId && r.StudentId == registration.StudentId);

        if (alreadyRegistered)
        {
            ModelState.AddModelError("", "This student is already registered for this event.");
        }

        if (ModelState.IsValid)
        {
            registration.RegistrationDate = DateTime.Now;
            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        await FillDropDowns();
        return View(registration);
    }

    private async Task FillDropDowns()
    {
        ViewBag.EventId = new SelectList(await _context.Events.ToListAsync(), "Id", "Title");
        ViewBag.StudentId = new SelectList(await _context.Students.ToListAsync(), "Id", "Name");
    }
}
