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

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _context = applicationDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var upcomingEvents = _context.Activities
                                               .Where(e => e.DateTime > DateTime.Now)
                                               .OrderBy(e => e.DateTime)
                                               .Take(10)
                                               .ToList();


            var highlightActivity = upcomingEvents.FirstOrDefault(); 
            var remainingActivities = upcomingEvents.Skip(1).ToList(); 

            ViewBag.HighlightActivity = highlightActivity;

            return View(remainingActivities);
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
