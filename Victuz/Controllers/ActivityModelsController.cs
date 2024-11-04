using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Victuz.Data;
using Victuz.Models;

namespace Victuz.Controllers
{
    public class ActivityModelsController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly ApplicationDbContext _context;

        public ActivityModelsController(ApplicationDbContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ActivityModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Activities.Include(a => a.Location).Include(a => a.Categories);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ActivityModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityModel = await _context.Activities
                .Include(a => a.Registrations)
                .Include(a => a.Categories)
                .Include(a => a.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityModel == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            bool isRegistered = false;

            if (user != null)
            {
                isRegistered = await _context.Registrations
                    .AnyAsync(r => r.Activity.Id == id && r.Member.Id == user.Id);
            }

            var viewModel = new ActivityDetailsViewModel
            {
                Activity = activityModel,
                IsRegistered = isRegistered
            };

            return View(viewModel);
        }

        // GET: ActivityModels/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
            ViewData["CategoryIds"] = new MultiSelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: ActivityModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,DateTime,MaxParticipants,CategoryIds,LocationId")] ActivityModel activityModel)
        {
            if (ModelState.IsValid)
            {
                activityModel.Categories = await _context.Categories
                    .Where(c => activityModel.CategoryIds.Contains(c.Id))
                    .ToListAsync();
                _context.Add(activityModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
            ViewData["CategoryIds"] = new MultiSelectList(_context.Categories, "Id", "Name");
            return View(activityModel);
        }

        // GET: ActivityModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityModel = await _context.Activities
                .Include(a => a.Categories)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (activityModel == null)
            {
                return NotFound();
            }

            activityModel.CategoryIds = activityModel.Categories.Select(c => c.Id).ToList();

            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", activityModel.LocationId);
            ViewData["CategoryIds"] = new MultiSelectList(_context.Categories, "Id", "Name", activityModel.CategoryIds);
            return View(activityModel);
        }

        // POST: ActivityModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Description,DateTime,MaxParticipants,CategoryIds,LocationId")] ActivityModel activityModel)
        {
            if (id != activityModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Haal de bestaande activiteit op inclusief de categorieën
                    var existingActivity = await _context.Activities
                        .Include(a => a.Categories)
                        .FirstOrDefaultAsync(a => a.Id == id);

                    if (existingActivity == null)
                    {
                        return NotFound();
                    }

                    // Update de eigenschappen van de activiteit
                    existingActivity.Name = activityModel.Name;
                    existingActivity.Description = activityModel.Description;
                    existingActivity.DateTime = activityModel.DateTime;
                    existingActivity.MaxParticipants = activityModel.MaxParticipants;
                    existingActivity.LocationId = activityModel.LocationId;

                    // Verwijder de bestaande categorieën
                    existingActivity.Categories.Clear();

                    // Voeg de nieuwe categorieën toe
                    if (activityModel.CategoryIds != null)
                    {
                        var selectedCategories = await _context.Categories
                            .Where(c => activityModel.CategoryIds.Contains(c.Id))
                            .ToListAsync();

                        foreach (var category in selectedCategories)
                        {
                            existingActivity.Categories.Add(category);
                        }
                    }

                    _context.Update(existingActivity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityModelExists(activityModel.Id))
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

            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", activityModel.LocationId);
            ViewData["CategoryIds"] = new MultiSelectList(_context.Categories, "Id", "Name", activityModel.CategoryIds);
            return View(activityModel);
        }

        // GET: ActivityModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityModel = await _context.Activities
                .Include(a => a.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityModel == null)
            {
                return NotFound();
            }

            return View(activityModel);
        }

        // POST: ActivityModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var activityModel = await _context.Activities.FindAsync(id);
            if (activityModel != null)
            {
                _context.Activities.Remove(activityModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityModelExists(int? id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(string SearchQuery)
        {
            var activities = _context.Activities
                .Include(a => a.Categories)
                .Where(a => a.Name.Contains(SearchQuery) ||
                            a.Description.Contains(SearchQuery) ||
                            a.Categories.Any(c => c.Name.Contains(SearchQuery)) ||
                            a.Location.City.Contains(SearchQuery) ||
                            a.Location.Street.Contains(SearchQuery));
            return View(activities);
        }
    }
}
