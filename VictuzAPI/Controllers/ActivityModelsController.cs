using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Victuz.Data;
using Victuz.Models;

namespace VictuzAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityModelsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ActivityModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ActivityModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActivityModel>>> GetActivities()
        {
            return await _context.Activities
                .Include(a => a.Categories)
                .Include(a => a.Location)
                .Include(a => a.Registrations)
                .ToListAsync();
        }

        // GET: api/ActivityModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ActivityModel>> GetActivityModel(int? id)
        {
            var activityModel = await _context.Activities
                .Include(a => a.Categories)
                .Include(a => a.Location)
                .Include(a => a.Registrations)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (activityModel == null)
            {
                return NotFound(new { Message = $"Activiteit met ID {id} niet gevonden." });
            }

            return activityModel;
        }

        // POST: api/ActivityModels
        [HttpPost]
        public async Task<ActionResult<ActivityModel>> PostActivityModel(
            [FromForm] string name,
            [FromForm] string description,
            [FromForm] DateTime dateTime,
            [FromForm] int maxParticipants,
            [FromForm] List<int> categoryIds,
            [FromForm] int locationId,
            [FromForm] string paymentType)
        {
            var activityModel = new ActivityModel
            {
                Name = name,
                Description = description,
                DateTime = dateTime,
                MaxParticipants = maxParticipants,
                CategoryIds = categoryIds,
                LocationId = locationId,
                PaymentType = paymentType
            };

            _context.Activities.Add(activityModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActivityModel), new { id = activityModel.Id }, activityModel);
        }


        // PUT: api/ActivityModels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivityModel(
            int id,
            [FromForm] string name,
            [FromForm] string description,
            [FromForm] DateTime dateTime,
            [FromForm] int maxParticipants,
            [FromForm] List<int> categoryIds,
            [FromForm] int locationId,
            [FromForm] string paymentType)
        {
            var activityModel = await _context.Activities
                .Include(a => a.Categories)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (activityModel == null)
            {
                return NotFound(new { Message = $"Activiteit met ID {id} niet gevonden." });
            }

            activityModel.Name = name;
            activityModel.Description = description;
            activityModel.DateTime = dateTime;
            activityModel.MaxParticipants = maxParticipants;
            activityModel.CategoryIds = categoryIds;
            activityModel.LocationId = locationId;
            activityModel.PaymentType = paymentType;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { Message = "Er is iets fout gegaan tijdens het bijwerken van de activiteit." });
            }

            return NoContent();
        }


        // DELETE: api/ActivityModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivityModel(int? id)
        {
            var activityModel = await _context.Activities.FindAsync(id);
            if (activityModel == null)
            {
                return NotFound(new { Message = $"Activiteit met ID {id} niet gevonden." });
            }

            _context.Activities.Remove(activityModel);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, new { Message = "Er is iets fout gegaan tijdens het verwijderen van de activiteit." });
            }

            return NoContent();
        }

        private bool ActivityModelExists(int? id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }
    }
}
