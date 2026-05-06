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

    public IActionResult Index()
    {
        var registrations = _context.Registrations
            .Include(r => r.Event)
            .Include(r => r.Student)
            .ToList();

        return View(registrations);
    }

    public IActionResult Create()
    {
        FillDropDowns();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Registration registration)
    {
        var schoolEvent = _context.Events
            .Include(e => e.Registrations)
            .FirstOrDefault(e => e.Id == registration.EventId);

        if (schoolEvent == null)
        {
            ModelState.AddModelError("", "Please choose an event.");
        }
        else if (schoolEvent.Registrations.Count >= schoolEvent.MaxParticipants)
        {
            ModelState.AddModelError("", "This event is full.");
        }

        bool alreadyRegistered = _context.Registrations.Any(r =>
            r.EventId == registration.EventId && r.StudentId == registration.StudentId);

        if (alreadyRegistered)
        {
            ModelState.AddModelError("", "This student is already registered for this event.");
        }

        if (ModelState.IsValid)
        {
            registration.RegistrationDate = DateTime.Now;
            _context.Registrations.Add(registration);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        FillDropDowns();
        return View(registration);
    }

    private void FillDropDowns()
    {
        ViewBag.EventId = new SelectList(_context.Events.ToList(), "Id", "Title");
        ViewBag.StudentId = new SelectList(_context.Students.ToList(), "Id", "Name");
    }
}
