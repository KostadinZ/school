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
            .Include(registration => registration.Event)
            .Include(registration => registration.Student)
            .OrderByDescending(registration => registration.RegistrationDate)
            .ToListAsync();

        return View(registrations);
    }

    public async Task<IActionResult> Create(int? eventId)
    {
        await PopulateDropDowns(eventId);
        return View(new Registration { EventId = eventId ?? 0, RegistrationDate = DateTime.Now });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EventId,StudentId")] Registration registration)
    {
        var schoolEvent = await _context.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(e => e.Id == registration.EventId);

        if (schoolEvent == null)
        {
            ModelState.AddModelError(nameof(Registration.EventId), "Please choose a valid event.");
        }
        else if (schoolEvent.Registrations.Count >= schoolEvent.MaxParticipants)
        {
            ModelState.AddModelError(nameof(Registration.EventId), "This event is full.");
        }

        if (!await _context.Students.AnyAsync(student => student.Id == registration.StudentId))
        {
            ModelState.AddModelError(nameof(Registration.StudentId), "Please choose a valid student.");
        }

        var alreadyRegistered = await _context.Registrations.AnyAsync(existing =>
            existing.EventId == registration.EventId && existing.StudentId == registration.StudentId);
        if (alreadyRegistered)
        {
            ModelState.AddModelError(string.Empty, "This student is already registered for the selected event.");
        }

        if (ModelState.IsValid)
        {
            registration.RegistrationDate = DateTime.Now;
            _context.Add(registration);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Registration completed successfully.";
            return RedirectToAction(nameof(Index));
        }

        await PopulateDropDowns(registration.EventId);
        return View(registration);
    }

    private async Task PopulateDropDowns(int? selectedEventId = null)
    {
        var events = await _context.Events
            .Include(schoolEvent => schoolEvent.Registrations)
            .OrderBy(schoolEvent => schoolEvent.Date)
            .ToListAsync();

        ViewBag.EventId = new SelectList(
            events.Select(schoolEvent => new
            {
                schoolEvent.Id,
                DisplayName = $"{schoolEvent.Title} ({Math.Max(0, schoolEvent.MaxParticipants - schoolEvent.Registrations.Count)} seats left)"
            }),
            "Id",
            "DisplayName",
            selectedEventId);

        ViewBag.StudentId = new SelectList(
            await _context.Students.OrderBy(student => student.Name).ToListAsync(),
            "Id",
            "Name");
    }
}
