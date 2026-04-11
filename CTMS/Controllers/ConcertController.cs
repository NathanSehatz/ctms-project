using CTMS.Data;
using CTMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CTMS.Controllers;

[Authorize]
public class ConcertController : Controller
{
    private readonly ApplicationDbContext _context;

    public ConcertController(ApplicationDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View(_context.Concerts.ToList());
    }
    
    [AllowAnonymous]
    public IActionResult Details(int id)
    {
        var concert = _context.Concerts
            .Include(c => c.TicketTypes)
            .FirstOrDefault(c => c.Id == id);

        if (concert == null)
        {
            return NotFound();
        }

        return View(concert);
    }

    [Authorize(Roles = "Admin,Organizer")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Organizer")]
    public IActionResult Create(Concert concert)
    {
        if (ModelState.IsValid)
        {
            _context.Concerts.Add(concert);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(concert);
    }

    [Authorize(Roles = "Admin,Organizer")]
    public IActionResult Edit(int id)
    {
        var concert = _context.Concerts.Find(id);
        if (concert == null) return NotFound();
        return View(concert);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Organizer")]
    public IActionResult Edit(Concert concert)
    {
        if (ModelState.IsValid)
        {
            _context.Concerts.Update(concert);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(concert);
    }

    [Authorize(Roles = "Admin,Organizer")]
    public IActionResult Delete(int id)
    {
        var concert = _context.Concerts.Find(id);
        if (concert == null) return NotFound();
        return View(concert);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin,Organizer")]
    public IActionResult DeleteConfirmed(int id)
    {
        var concert = _context.Concerts.Find(id);
        if (concert != null)
        {
            _context.Concerts.Remove(concert);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}