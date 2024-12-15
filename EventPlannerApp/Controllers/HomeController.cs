using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EventPlannerApp.Data;
using EventPlannerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace EventPlannerApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EventPlannerContext _context;

        public HomeController(ILogger<HomeController> logger, EventPlannerContext context)
        {
            _logger = logger;
            _context = context;
        }
        //Views genereren
        public IActionResult Index()
        {
            
            var events = _context.Events.Where(e => e.Date >= DateTime.Now).ToList();
            return View(events);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Info()
        {
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult EventData(int id)
        {
            var eventDetails = _context.Events.FirstOrDefault(e => e.Id == id);
            if (eventDetails == null)
            {
                return NotFound();
            }
            return View(eventDetails);
        }

        // Ticket reserveren
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReserveTicket(int eventId, string userEmail)
        {
            var @event = await _context.Events
                .Include(e => e.Tickets)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (@event == null || @event.Date <= DateTime.Now || @event.MaxParticipants <= 0)
            {
                return RedirectToAction("Failed");
            }

            if (string.IsNullOrEmpty(userEmail))
            {
                ModelState.AddModelError(nameof(userEmail), "Email is vereist.");
                return View("EventData", @event); 
            }

          
            var participant = _context.Participants.FirstOrDefault(p => p.Email == userEmail);

            if (participant == null)
            {
                return RedirectToAction("Failed");
            }

            try
            {
                var ticket = new Ticket
                {
                    EventId = eventId,
                    ParticipantId = participant.Id,
                    OrderNumber = Guid.NewGuid().ToString(),
                    IsPaid = false
                };

                _context.Tickets.Add(ticket);

                
                var eventToUpdate = await _context.Events.FindAsync(eventId);
                if (eventToUpdate != null)
                {
                    eventToUpdate.AvailableSlots -= 1;
                    _context.Update(eventToUpdate);
                    await _context.SaveChangesAsync();
                }

                TempData["OrderNumber"] = ticket.OrderNumber;
                TempData["SuccessMessage"] = "Je ticket is succesvol gereserveerd!";
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Error bij het reserveren van ticket.");
                return RedirectToAction("Failed"); 
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Failed()
        {
            return View();
        }
    }
}
