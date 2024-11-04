using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Victuz.Data;
using Victuz.Models;

namespace Victuz.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Person> _userManager;

        public RegistrationsController(ApplicationDbContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Registrations
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Verkrijg de ID van de ingelogde gebruiker

            var registrations = await _context.Registrations
                .Include(r => r.Activity) // Zorg ervoor dat de activiteit wordt opgenomen
                .Where(r => r.Member.Id == userId) // Filter op de ingelogde gebruiker
                .ToListAsync();

            return View(registrations);
        }

        // GET: Registrations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // GET: Registrations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(registration);
        }

        // GET: Registrations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
            {
                return NotFound();
            }
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Registration registration)
        {
            if (id != registration.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistrationExists(registration.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(registration);
        }

        // GET: Registrations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _context.Registrations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (registration == null)
            {
                return NotFound();
            }

            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration != null)
            {
                _context.Registrations.Remove(registration);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistrationExists(int id)
        {
            return _context.Registrations.Any(e => e.Id == id);
        }

        //De code voor het aanmelden voor een activiteit
        [HttpPost]
        public async Task<IActionResult> Register(int activityId)
        {
            // Haal de ingelogde gebruiker op
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(); // Indien gebruiker niet ingelogd is, geen toegang
            }

            // Controleer of de activiteit bestaat
            var activity = await _context.Activities.FindAsync(activityId);
            if (activity == null)
            {
                return NotFound(); // Activiteit bestaat niet
            }

            // Controleer of de gebruiker al geregistreerd is voor deze activiteit
            var existingRegistration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.Activity.Id == activityId && r.Member.Id == user.Id);
            if (existingRegistration != null)
            {
                // Als de gebruiker al is geregistreerd, melding tonen
                TempData["Message"] = "Je bent al ingeschreven voor deze activiteit.";
                return RedirectToAction("Details", "ActivityModels", new { id = activityId });
            }

            // Maak een nieuwe registratie aan
            var registration = new Registration
            {
                Member = (Member)user,
                Activity = activity
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Je bent succesvol ingeschreven!";
            return RedirectToAction("Details", "ActivityModels", new { id = activityId });
        }

        //Code voor het afmelden van een activiteit
        [HttpPost]
        public async Task<IActionResult> Unregister(int activityId)
        {
            // Haal de ingelogde gebruiker op
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(); // Indien gebruiker niet ingelogd is, geen toegang
            }

            // Zoek de bestaande registratie op basis van de activiteit en de gebruiker
            var registration = await _context.Registrations
                .FirstOrDefaultAsync(r => r.Activity.Id == activityId && r.Member.Id == user.Id);

            if (registration == null)
            {
                // Indien geen registratie gevonden, melding tonen
                TempData["Message"] = "Je bent niet ingeschreven voor deze activiteit.";
                return RedirectToAction("Details", "ActivityModels", new { id = activityId });
            }

            // Verwijder de registratie en sla wijzigingen op
            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Je bent succesvol uitgeschreven.";
            return RedirectToAction("Details", "ActivityModels", new { id = activityId });
        }

        //Aanwezigheidcode
        public async Task<IActionResult> AttendanceList(int activityId)
        {
            var registrations = await _context.Registrations
                .Where(r => r.Activity != null && r.Activity.Id == activityId)
                .Include(r => r.Member)
                .Include(r => r.Activity)
                .ToListAsync();

            if (!registrations.Any())
            {
                return NotFound("Geen registraties gevonden voor deze activiteit");
            }

            return View(registrations);
        }

        [HttpPost]
        public async Task<IActionResult> MarkAttendance(int registrationId, bool isPresent)
        {
            var registration = await _context.Registrations
                .Include(r => r.Activity) // Zorg ervoor dat Activity wordt ingeladen
                .FirstOrDefaultAsync(r => r.Id == registrationId);

            if (registration == null)
            {
                return NotFound();
            }

            // Update de aanwezigheid
            registration.IsPresent = isPresent;
            await _context.SaveChangesAsync();

            return RedirectToAction("AttendanceList", new { activityId = registration.Activity.Id });
        }
    }
}
