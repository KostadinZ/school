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

    public IActionResult Index(string? searchString)
    {
        var events = _context.Events.Include(e => e.Registrations).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            events = events.Where(e => e.Title.Contains(searchString));
        }

        ViewBag.SearchString = searchString;
        return View(events.OrderBy(e => e.Date).ToList());
    }

    public IActionResult Details(int id)
    {
        var schoolEvent = _context.Events
            .Include(e => e.Registrations)
            .ThenInclude(r => r.Student)
            .FirstOrDefault(e => e.Id == id);

        if (schoolEvent == null)
        {
            return NotFound();
        }

        return View(schoolEvent);
    }

    public IActionResult Create()
    {
        return View(new Event());
    }

    [HttpPost]
    public IActionResult Create(Event schoolEvent)
    {
        if (ModelState.IsValid)
        {
            _context.Events.Add(schoolEvent);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(schoolEvent);
    }

    public IActionResult Edit(int id)
    {
        var schoolEvent = _context.Events.Find(id);

        if (schoolEvent == null)
        {
            return NotFound();
        }

        return View(schoolEvent);
    }

    [HttpPost]
    public IActionResult Edit(Event schoolEvent)
    {
        if (ModelState.IsValid)
        {
            _context.Events.Update(schoolEvent);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(schoolEvent);
    }

    public IActionResult Delete(int id)
    {
        var schoolEvent = _context.Events.Find(id);

        if (schoolEvent == null)
        {
            return NotFound();
        }

        return View(schoolEvent);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var schoolEvent = _context.Events.Find(id);

        if (schoolEvent != null)
        {
            _context.Events.Remove(schoolEvent);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }
}
