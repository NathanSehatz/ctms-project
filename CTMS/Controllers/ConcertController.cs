namespace CTMS.Controllers;

using Microsoft.AspNetCore.Mvc;
using CTMS.Models;
using System.Collections.Generic;

public class ConcertController : Controller
{
    public IActionResult Index()
    {
        var concerts = new List<Concert>
        {
            new Concert { Id = 1, Title = "Metal Night", Location = "Toronto", Date = DateTime.Now }
        };

        return View(concerts);
    }
}