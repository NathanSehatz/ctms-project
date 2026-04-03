using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CTMS.Data;
using CTMS.Models;

namespace CTMS.Controllers
{
    public class TicketTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List ticket types for a specific concert
        public IActionResult Index(int concertId)
        {
            var concert = _context.Concerts.FirstOrDefault(c => c.Id == concertId);
            if (concert == null) return NotFound();

            ViewBag.ConcertTitle = concert.Title;
            ViewBag.ConcertId = concert.Id;

            var ticketTypes = _context.TicketTypes
                .Where(t => t.ConcertId == concertId)
                .ToList();

            return View(ticketTypes);
        }

        // GET: Create ticket type for a concert
        public IActionResult Create(int concertId)
        {
            var concert = _context.Concerts.FirstOrDefault(c => c.Id == concertId);
            if (concert == null) return NotFound();

            ViewBag.ConcertTitle = concert.Title;
            ViewBag.ConcertId = concert.Id;

            var ticketType = new TicketType
            {
                ConcertId = concertId
            };

            return View(ticketType);
        }

        // POST: Create ticket type
        [HttpPost]
        public IActionResult Create(TicketType ticketType)
        {
            if (ModelState.IsValid)
            {
                _context.TicketTypes.Add(ticketType);
                _context.SaveChanges();
                return RedirectToAction("Index", new { concertId = ticketType.ConcertId });
            }

            var concert = _context.Concerts.FirstOrDefault(c => c.Id == ticketType.ConcertId);
            ViewBag.ConcertTitle = concert?.Title;
            ViewBag.ConcertId = ticketType.ConcertId;

            return View(ticketType);
        }

        // GET: Edit ticket type
        public IActionResult Edit(int id)
        {
            var ticketType = _context.TicketTypes.FirstOrDefault(t => t.Id == id);
            if (ticketType == null) return NotFound();

            var concert = _context.Concerts.FirstOrDefault(c => c.Id == ticketType.ConcertId);
            ViewBag.ConcertTitle = concert?.Title;
            ViewBag.ConcertId = ticketType.ConcertId;

            return View(ticketType);
        }

        // POST: Edit ticket type
        [HttpPost]
        public IActionResult Edit(TicketType ticketType)
        {
            if (ModelState.IsValid)
            {
                _context.TicketTypes.Update(ticketType);
                _context.SaveChanges();
                return RedirectToAction("Index", new { concertId = ticketType.ConcertId });
            }

            var concert = _context.Concerts.FirstOrDefault(c => c.Id == ticketType.ConcertId);
            ViewBag.ConcertTitle = concert?.Title;
            ViewBag.ConcertId = ticketType.ConcertId;

            return View(ticketType);
        }

        // GET: Delete ticket type
        public IActionResult Delete(int id)
        {
            var ticketType = _context.TicketTypes
                .Include(t => t.Concert)
                .FirstOrDefault(t => t.Id == id);

            if (ticketType == null) return NotFound();

            return View(ticketType);
        }

        // POST: Delete ticket type
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var ticketType = _context.TicketTypes.FirstOrDefault(t => t.Id == id);
            if (ticketType == null) return NotFound();

            int concertId = ticketType.ConcertId;

            _context.TicketTypes.Remove(ticketType);
            _context.SaveChanges();

            return RedirectToAction("Index", new { concertId = concertId });
        }
    }
}