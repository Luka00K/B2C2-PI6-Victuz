using Microsoft.AspNetCore.Mvc;
using Victuz.Data;

namespace Victuz.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CalendarController(ApplicationDbContext context) {
            _context = context;
        }


        public IActionResult Calendar()
        {
            return View(_context.Activities.ToList());
        }
    }
}
