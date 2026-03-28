using Microsoft.AspNetCore.Mvc;
using CTMS.Data;
using CTMS.Models;

namespace CTMS.Controllers
{
    public class ConcertController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConcertController(ApplicationDbContext context)
        {
            _context = context;
        }

        // READ (List)
        public IActionResult Index()
        {
            var concerts = _context.Concerts.ToList();
            return View(concerts);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        public IActionResult Create(Concert concert)
        {
            Console.WriteLine("POST Create reached");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid");

                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Field: {error.Key}");
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($"Error: {err.ErrorMessage}");
                    }
                }

                return View(concert);
            }

            _context.Concerts.Add(concert);
            _context.SaveChanges();

            Console.WriteLine("Concert saved");
            return RedirectToAction("Index");
        }

        // EDIT (GET)
        public IActionResult Edit(int id)
        {
            var concert = _context.Concerts.Find(id);
            if (concert == null) return NotFound();
            return View(concert);
        }

        // EDIT (POST)
        [HttpPost]
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

        // DELETE (GET)
        public IActionResult Delete(int id)
        {
            var concert = _context.Concerts.Find(id);
            if (concert == null) return NotFound();
            return View(concert);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var concert = _context.Concerts.Find(id);
            _context.Concerts.Remove(concert);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}