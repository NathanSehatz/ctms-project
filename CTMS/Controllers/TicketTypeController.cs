using CTMS.Data;
using CTMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CTMS.Controllers;

[Authorize]
public class TicketTypeController : Controller
{
    private readonly ApplicationDbContext _context;

    public TicketTypeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public IActionResult Index(int concertId)
    {
        var ticketTypes = _context.TicketTypes
            .Where(t => t.ConcertId == concertId)
            .ToList();

        var concert = _context.Concerts.Find(concertId);

        ViewBag.ConcertTitle = concert?.Title;
        ViewBag.ConcertId = concertId;

        return View(ticketTypes);
    }

    [Authorize(Roles = "Admin,Organizer")]
    public IActionResult Create(int concertId)
    {
        ViewBag.ConcertId = concertId;
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Organizer")]
    public IActionResult Create(TicketType ticketType)
    {
        if (ModelState.IsValid)
        {
            _context.TicketTypes.Add(ticketType);
            _context.SaveChanges();
            return RedirectToAction("Index", new { concertId = ticketType.ConcertId });
        }
        return View(ticketType);
    }

    [Authorize(Roles = "Admin,Organizer")]
    public IActionResult Edit(int id)
    {
        var ticket = _context.TicketTypes.Find(id);
        if (ticket == null) return NotFound();
        return View(ticket);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Organizer")]
    public IActionResult Edit(TicketType ticketType)
    {
        if (ModelState.IsValid)
        {
            _context.TicketTypes.Update(ticketType);
            _context.SaveChanges();
            return RedirectToAction("Index", new { concertId = ticketType.ConcertId });
        }
        return View(ticketType);
    }

    [Authorize(Roles = "Admin,Organizer")]
    public IActionResult Delete(int id)
    {
        var ticket = _context.TicketTypes.Find(id);
        if (ticket == null) return NotFound();
        return View(ticket);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin,Organizer")]
    public IActionResult DeleteConfirmed(int id)
    {
        var ticket = _context.TicketTypes.Find(id);
        if (ticket != null)
        {
            int concertId = ticket.ConcertId;
            _context.TicketTypes.Remove(ticket);
            _context.SaveChanges();
            return RedirectToAction("Index", new { concertId });
        }
        return RedirectToAction("Index");
    }
}