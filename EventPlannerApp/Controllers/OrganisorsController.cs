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
    public class OrganisorsController : Controller
    {
        private readonly EventPlannerContext _context;

        public OrganisorsController(EventPlannerContext context)
        {
            _context = context;
        }

        // GET: Organisors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Organisors.ToListAsync());
        }

        // GET: Organisors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organisor = await _context.Organisors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organisor == null)
            {
                return NotFound();
            }

            return View(organisor);
        }

        // GET: Organisors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organisors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email")] Organisor organisor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(organisor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organisor);
        }

        // GET: Organisors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organisor = await _context.Organisors.FindAsync(id);
            if (organisor == null)
            {
                return NotFound();
            }
            return View(organisor);
        }

        // POST: Organisors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email")] Organisor organisor)
        {
            if (id != organisor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organisor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganisorExists(organisor.Id))
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
            return View(organisor);
        }

        // GET: Organisors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organisor = await _context.Organisors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organisor == null)
            {
                return NotFound();
            }

            return View(organisor);
        }

        // POST: Organisors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organisor = await _context.Organisors.FindAsync(id);
            if (organisor != null)
            {
                _context.Organisors.Remove(organisor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganisorExists(int id)
        {
            return _context.Organisors.Any(e => e.Id == id);
        }
    }
}
