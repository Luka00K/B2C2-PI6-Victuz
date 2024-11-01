using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
            return await _context.Activities.ToListAsync();
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
                return NotFound();
            }

            return activityModel;
        }

        // PUT: api/ActivityModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivityModel(int? id, ActivityModel activityModel)
        {
            if (id != activityModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(activityModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivityModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ActivityModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ActivityModel>> PostActivityModel(ActivityModel activityModel)
        {
            activityModel.Id = null;


            _context.Activities.Add(activityModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActivityModel", new { id = activityModel.Id }, activityModel);
        }

        // DELETE: api/ActivityModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivityModel(int? id)
        {
            var activityModel = await _context.Activities.FindAsync(id);
            if (activityModel == null)
            {
                return NotFound();
            }

            _context.Activities.Remove(activityModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ActivityModelExists(int? id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }
    }
}
