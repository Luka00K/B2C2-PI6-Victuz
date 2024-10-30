using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Victuz.Data;
using Victuz.Models;

namespace Victuz.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Haal aankomende activiteiten op en sorteer op datum
            var upcomingActivities = await _context.Activities
                .Where(a => a.DateTime >= DateTime.Now)
                .OrderBy(a => a.DateTime)
                .ToListAsync();

            // Voeg de activiteiten toe aan de ViewData om te gebruiken in de View
            ViewData["UpcomingActivities"] = upcomingActivities;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
