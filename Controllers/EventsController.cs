using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolEventRegistration.Data;
using SchoolEventRegistration.Models;
using SchoolEventRegistration.ViewModels;

namespace SchoolEventRegistration.Controllers;

public class EventsController : Controller
{
    private readonly SchoolEventContext _context;

    public EventsController(SchoolEventContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? searchString)
    {
        var eventsQuery = _context.Events
            .Include(schoolEvent => schoolEvent.Registrations)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            eventsQuery = eventsQuery.Where(schoolEvent => schoolEvent.Title.Contains(searchString));
        }

        var events = await eventsQuery
            .OrderBy(schoolEvent => schoolEvent.Date)
            .Select(schoolEvent => new EventListItemViewModel
            {
                Id = schoolEvent.Id,
                Title = schoolEvent.Title,
                Description = schoolEvent.Description,
                Date = schoolEvent.Date,
                Location = schoolEvent.Location,
                MaxParticipants = schoolEvent.MaxParticipants,
                RegisteredCount = schoolEvent.Registrations.Count
            })
            .ToListAsync();

        ViewData["CurrentFilter"] = searchString;
        return View(events);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var schoolEvent = await _context.Events
            .Include(e => e.Registrations)
            .ThenInclude(r => r.Student)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (schoolEvent == null)
        {
            return NotFound();
        }

        return View(schoolEvent);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Description,Date,Location,MaxParticipants")] Event schoolEvent)
    {
        if (ModelState.IsValid)
        {
            _context.Add(schoolEvent);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Event created successfully.";
            return RedirectToAction(nameof(Index));
        }

        return View(schoolEvent);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var schoolEvent = await _context.Events.FindAsync(id);
        if (schoolEvent == null)
        {
            return NotFound();
        }

        return View(schoolEvent);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Date,Location,MaxParticipants")] Event schoolEvent)
    {
        if (id != schoolEvent.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var registeredCount = await _context.Registrations.CountAsync(registration => registration.EventId == id);
            if (schoolEvent.MaxParticipants < registeredCount)
            {
                ModelState.AddModelError(nameof(Event.MaxParticipants),
                    $"Maximum participants cannot be lower than the current registration count ({registeredCount}).");
                return View(schoolEvent);
            }

            try
            {
                _context.Update(schoolEvent);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Event updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EventExists(schoolEvent.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(schoolEvent);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var schoolEvent = await _context.Events
            .Include(e => e.Registrations)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (schoolEvent == null)
        {
            return NotFound();
        }

        return View(schoolEvent);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var schoolEvent = await _context.Events.FindAsync(id);
        if (schoolEvent != null)
        {
            _context.Events.Remove(schoolEvent);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Event deleted successfully.";
        }

        return RedirectToAction(nameof(Index));
    }

    private Task<bool> EventExists(int id)
    {
        return _context.Events.AnyAsync(e => e.Id == id);
    }
}
