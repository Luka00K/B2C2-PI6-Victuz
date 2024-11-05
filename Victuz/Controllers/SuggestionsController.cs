using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Victuz.Data;
using Victuz.Models;

namespace Victuz.Controllers
{
    public class SuggestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuggestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Suggestions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Suggestions.ToListAsync());
        }

        // GET: Suggestions/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var suggestion = await _context.Suggestions
                .Include(s => s.Likes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (suggestion == null)
                return NotFound();

            // Haal het gebruikers-ID en de rol van de ingelogde gebruiker op
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isBoardMember = User.IsInRole("BoardMember");

            // Bepaal het aantal likes
            int likeCount = suggestion.Likes.Count;

            // Sla de like-informatie op in ViewBag
            ViewBag.IsBoardMember = isBoardMember;
            ViewBag.LikeCount = likeCount;

            // Als de gebruiker geen BoardMember is, controleer dan of hij het voorstel al geliket heeft
            if (!isBoardMember)
            {
                bool isLikedByCurrentUser = suggestion.Likes.Any(like => like.UserId == userId);
                ViewBag.IsLikedByCurrentUser = isLikedByCurrentUser;
            }

            return View(suggestion);
        }

        // POST: Suggestions/ToggleLike
        [HttpPost]
        public async Task<IActionResult> ToggleLike(int id)
        {
            var suggestion = await _context.Suggestions
                .Include(s => s.Likes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (suggestion == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Controleer of de gebruiker al een like heeft gegeven
            var existingLike = suggestion.Likes.FirstOrDefault(like => like.UserId == userId);

            if (existingLike == null)
            {
                // Voeg een like toe als deze nog niet bestaat
                suggestion.Likes.Add(new Like { UserId = userId, SuggestionId = suggestion.Id });
            }
            else
            {
                // Verwijder de like als deze al bestaat
                suggestion.Likes.Remove(existingLike);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = id });
        }

        // GET: Suggestions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suggestions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Id,Description")] Suggestion suggestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suggestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(suggestion);
        }

        // GET: Suggestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suggestion = await _context.Suggestions.FindAsync(id);
            if (suggestion == null)
            {
                return NotFound();
            }
            return View(suggestion);
        }

        // POST: Suggestions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Title")] Suggestion suggestion)
        {
            if (id != suggestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suggestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuggestionExists(suggestion.Id))
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
            return View(suggestion);
        }

        // GET: Suggestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suggestion = await _context.Suggestions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suggestion == null)
            {
                return NotFound();
            }

            return View(suggestion);
        }

        // POST: Suggestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suggestion = await _context.Suggestions.FindAsync(id);
            if (suggestion != null)
            {
                _context.Suggestions.Remove(suggestion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuggestionExists(int id)
        {
            return _context.Suggestions.Any(e => e.Id == id);
        }
    }
}
