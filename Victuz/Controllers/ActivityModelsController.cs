﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Victuz.Data;
using Victuz.Models;
using Victuz.Viewmodels;

namespace Victuz.Controllers
{
    public class ActivityModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Person> _userManager;

        public ActivityModelsController(ApplicationDbContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ActivityModels
        public async Task<IActionResult> Index()
        {
            var activiteiten = await _context.Activities
                                             .Include(a => a.Category)
                                             .Select(a => new ActivityViewModel
                                             {
                                                 Id = a.Id,
                                                 Name = a.Name,
                                                 Description = a.Description,
                                                 DateTime = a.DateTime,
                                                 MaxParticipants = a.MaxParticipants,
                                                 CategoryName = a.Category.Name // Koppel de naam van de categorie
                                             })
                                             .ToListAsync();

            return View(activiteiten);
        }

        // GET: ActivityModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityModel = await _context.Activities
                .Include(a => a.Registrations) // Laad registraties om de status te controleren
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityModel == null)
            {
                return NotFound();
            }

            // Haal de ingelogde gebruiker op
            var user = await _userManager.GetUserAsync(User);
            bool isRegistered = false;

            // Controleer of de gebruiker is geregistreerd voor deze activiteit
            if (user != null)
            {
                isRegistered = await _context.Registrations
                    .AnyAsync(r => r.Activity.Id == id && r.Member.Id == user.Id);
            }

            // Maak het viewmodel aan
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
            ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: ActivityModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,DateTime,MaxParticipants,CategoryId")] ActivityModel activityModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "Name", activityModel.CategoryId);
            return View(activityModel);
        }

        // GET: ActivityModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activityModel = await _context.Activities.FindAsync(id);
            if (activityModel == null)
            {
                return NotFound();
            }

            ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "Name", activityModel.CategoryId);
            return View(activityModel);
        }

        // POST: ActivityModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,DateTime,MaxParticipants,CategoryId")] ActivityModel activityModel)
        {
            if (id != activityModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityModelExists((int)activityModel.Id))
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

            ViewBag.CategoryList = new SelectList(_context.Categories, "Id", "Name", activityModel.CategoryId);
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
                .Include(a => a.Category) // Zorg ervoor dat je de categorie ophaalt
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activityModel = await _context.Activities.FindAsync(id);
            if (activityModel != null)
            {
                _context.Activities.Remove(activityModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityModelExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }
    }
}
