using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CTMS.Data;

namespace CTMS.Controllers;

[Authorize(Roles = "Admin,Organizer")]
public class ReportsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Availability()
    {
        var concerts = _context.Concerts
            .Include(c => c.TicketTypes)
            .ToList();

        return View(concerts);
    }
}