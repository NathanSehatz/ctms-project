using CTMS.Data;
using CTMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CTMS.Controllers;

[Authorize]
public class ReservationController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReservationController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = int.Parse(userIdClaim);

        var reservations = _context.Reservations
            .Where(r => r.UserId == userId)
            .Include(r => r.TicketType)
            .ThenInclude(t => t.Concert)
            .ToList();

        return View(reservations);
    }

    public IActionResult Create(int ticketTypeId)
    {
        var ticket = _context.TicketTypes
            .Include(t => t.Concert)
            .FirstOrDefault(t => t.Id == ticketTypeId);

        if (ticket == null)
        {
            return NotFound();
        }

        ViewBag.TicketTypeId = ticketTypeId;
        ViewBag.TicketName = ticket.Name;
        ViewBag.Available = ticket.QuantityAvailable;

        return View();
    }

    [HttpPost]
    public IActionResult Create(int ticketTypeId, int quantity)
    {
        var ticket = _context.TicketTypes
            .Include(t => t.Concert)
            .FirstOrDefault(t => t.Id == ticketTypeId);

        if (ticket == null)
        {
            return NotFound();
        }

        ViewBag.TicketTypeId = ticketTypeId;
        ViewBag.TicketName = ticket.Name;
        ViewBag.Available = ticket.QuantityAvailable;

        if (quantity <= 0)
        {
            ModelState.AddModelError("", "Quantity must be greater than 0.");
            return View();
        }

        if (quantity > ticket.QuantityAvailable)
        {
            ModelState.AddModelError("", $"Only {ticket.QuantityAvailable} ticket(s) are available.");
            return View();
        }

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = int.Parse(userIdClaim);

        var reservation = new Reservation
        {
            TicketTypeId = ticketTypeId,
            Quantity = quantity,
            UserId = userId
        };

        ticket.QuantityAvailable -= quantity;

        _context.Reservations.Add(reservation);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}