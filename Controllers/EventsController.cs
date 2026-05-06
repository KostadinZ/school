using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolEventRegistration.Data;
using SchoolEventRegistration.Models;

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
        var events = _context.Events.Include(e => e.Registrations).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            events = events.Where(e => e.Title.Contains(searchString));
        }

        ViewBag.SearchString = searchString;
        return View(await events.OrderBy(e => e.Date).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var schoolEvent = await _context.Events
            .Include(e => e.Registrations)
            .ThenInclude(r => r.Student)
            .FirstOrDefaultAsync(e => e.Id == id);

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
    public async Task<IActionResult> Create(Event schoolEvent)
    {
        if (ModelState.IsValid)
        {
            _context.Events.Add(schoolEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(schoolEvent);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var schoolEvent = await _context.Events.FindAsync(id);

        if (schoolEvent == null)
        {
            return NotFound();
        }

        return View(schoolEvent);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Event schoolEvent)
    {
        if (ModelState.IsValid)
        {
            _context.Events.Update(schoolEvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(schoolEvent);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var schoolEvent = await _context.Events.FindAsync(id);

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
        }

        return RedirectToAction(nameof(Index));
    }
}
