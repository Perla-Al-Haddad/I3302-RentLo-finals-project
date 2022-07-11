using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using I3302_RentLo_finals_project.Data;
using I3302_RentLo_finals_project.Models;

namespace I3302_RentLo_finals_project.Views.UserPropertyRents
{
    public class UserPropertyRentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserPropertyRentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserPropertyRents
        public async Task<IActionResult> Index()
        {
              return View(await _context.UserPropertyRents.ToListAsync());
        }

        // GET: UserPropertyRents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserPropertyRents == null)
            {
                return NotFound();
            }

            var userPropertyRent = await _context.UserPropertyRents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userPropertyRent == null)
            {
                return NotFound();
            }

            return View(userPropertyRent);
        }

        // GET: UserPropertyRents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserPropertyRents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreatedDate,StartDate,EndDate")] UserPropertyRent userPropertyRent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userPropertyRent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userPropertyRent);
        }

        // GET: UserPropertyRents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserPropertyRents == null)
            {
                return NotFound();
            }

            var userPropertyRent = await _context.UserPropertyRents.FindAsync(id);
            if (userPropertyRent == null)
            {
                return NotFound();
            }
            return View(userPropertyRent);
        }

        // POST: UserPropertyRents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreatedDate,StartDate,EndDate")] UserPropertyRent userPropertyRent)
        {
            if (id != userPropertyRent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userPropertyRent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPropertyRentExists(userPropertyRent.Id))
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
            return View(userPropertyRent);
        }

        // GET: UserPropertyRents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserPropertyRents == null)
            {
                return NotFound();
            }

            var userPropertyRent = await _context.UserPropertyRents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userPropertyRent == null)
            {
                return NotFound();
            }

            return View(userPropertyRent);
        }

        // POST: UserPropertyRents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserPropertyRents == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserPropertyRents'  is null.");
            }
            var userPropertyRent = await _context.UserPropertyRents.FindAsync(id);
            if (userPropertyRent != null)
            {
                _context.UserPropertyRents.Remove(userPropertyRent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserPropertyRentExists(int id)
        {
          return _context.UserPropertyRents.Any(e => e.Id == id);
        }
    }
}
