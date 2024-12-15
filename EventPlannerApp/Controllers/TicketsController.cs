using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventPlannerApp.Data;
using EventPlannerApp.Models;

namespace EventPlannerApp.Controllers
{
    public class TicketsController : Controller
    {
        private readonly EventPlannerContext _context;

        public TicketsController(EventPlannerContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Tickets()
        {
            var cashiers = _context.Cashiers.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            ViewData["Cashiers"] = cashiers;

            var tickets = _context.Tickets
                .Include(t => t.Cashier)
                .Include(t => t.Event)
                .Include(t => t.Participant)
                .ToList();

            return View(tickets);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Cashier)
                .Include(t => t.Event)
                .Include(t => t.Participant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["CashierId"] = new SelectList(_context.Cashiers, "Id", "Name");
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name");
            ViewData["ParticipantId"] = new SelectList(_context.Participants, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderNumber,IsPaid,EventId,ParticipantId,CashierId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Tickets));
            }
            ViewData["CashierId"] = new SelectList(_context.Cashiers, "Id", "Name", ticket.CashierId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", ticket.EventId);
            ViewData["ParticipantId"] = new SelectList(_context.Participants, "Id", "Name", ticket.ParticipantId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["CashierId"] = new SelectList(_context.Cashiers, "Id", "Name", ticket.CashierId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", ticket.EventId);
            ViewData["ParticipantId"] = new SelectList(_context.Participants, "Id", "Name", ticket.ParticipantId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderNumber,IsPaid,EventId,ParticipantId,CashierId")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Tickets));
            }
            ViewData["CashierId"] = new SelectList(_context.Cashiers, "Id", "Name", ticket.CashierId);
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name", ticket.EventId);
            ViewData["ParticipantId"] = new SelectList(_context.Participants, "Id", "Name", ticket.ParticipantId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Cashier)
                .Include(t => t.Event)
                .Include(t => t.Participant)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int ticketId)
        {
            var ticket = await _context.Tickets.Include(t => t.Event).FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket != null)
            {
                // Als het ticket gekoppeld is aan een evenement, verhoog de beschikbare plaatsen
                if (ticket.Event != null)
                {
                    ticket.Event.AvailableSlots += 1;
                    _context.Update(ticket.Event);
                }

                // Verwijder het ticket
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Tickets));
        }


        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }

        public async Task<IActionResult> MarkAsPaid(int ticketId, int cashierId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket != null)
            {
                ticket.IsPaid = true;
                ticket.CashierId = cashierId;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Tickets));
        }
    }
}
