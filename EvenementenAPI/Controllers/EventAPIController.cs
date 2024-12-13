using Microsoft.AspNetCore.Mvc;
using EvenementenAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using EvenementenAPI.Data;

namespace EvenementenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventAPIController : ControllerBase
    {
        private readonly EventPlannerContext _context;

        public EventAPIController(EventPlannerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events.ToListAsync();
            return Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] Event newEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEvents), new { id = newEvent.Id }, newEvent);
            }
            return BadRequest(ModelState);
        }
    }
}
