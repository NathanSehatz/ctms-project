using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CTMS.Data;
using CTMS.Models;

namespace CTMS.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Create reservation
        public IActionResult Create(int ticketTypeId)
        {
            var ticket = _context.TicketTypes
                .Include(t => t.Concert)
                .FirstOrDefault(t => t.Id == ticketTypeId);

            if (ticket == null) return NotFound();

            ViewBag.TicketName = ticket.Name;
            ViewBag.Available = ticket.QuantityAvailable;
            ViewBag.TicketTypeId = ticket.Id;

            return View();
        }

        // POST: Create reservation
        [HttpPost]
        public IActionResult Create(int ticketTypeId, int? quantity)
        {
            var ticket = _context.TicketTypes
                .Include(t => t.Concert)
                .FirstOrDefault(t => t.Id == ticketTypeId);

            if (ticket == null) return NotFound();

            if (quantity == null || quantity <= 0)
            {
                ModelState.AddModelError("", "Please enter a valid quantity");

                ViewBag.TicketName = ticket.Name;
                ViewBag.Available = ticket.QuantityAvailable;
                ViewBag.TicketTypeId = ticket.Id;

                return View();
            }

            if (quantity > ticket.QuantityAvailable)
            {
                ModelState.AddModelError("", "Not enough tickets available");

                ViewBag.TicketName = ticket.Name;
                ViewBag.Available = ticket.QuantityAvailable;
                ViewBag.TicketTypeId = ticket.Id;

                return View();
            }

            var reservation = new Reservation
            {
                TicketTypeId = ticketTypeId,
                Quantity = quantity.Value,
                UserId = 1
            };

            ticket.QuantityAvailable -= quantity.Value;

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return RedirectToAction("Index", "TicketType", new { concertId = ticket.ConcertId });
        }
        
        public IActionResult Index()
        {
            var reservations = _context.Reservations
                .Include(r => r.TicketType)
                .ThenInclude(t => t.Concert)
                .ToList();

            return View(reservations);
        }
    }
}