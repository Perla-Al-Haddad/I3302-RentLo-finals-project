using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using I3302_RentLo_finals_project.Data;
using I3302_RentLo_finals_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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
        [Authorize(Roles = "PropertyAdministrators")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.UserPropertyRents.ToListAsync());
        }

        public async Task<IActionResult> MyRentals(string id) {
            var rentals = _context.UserPropertyRents.Where(x => x.RenterId == id).ToList();
            for (int i = 0; i < rentals.Count(); i++)
            {
                Property property = _context.Property.Where(x => x.Id == rentals[i].PropertyId).FirstOrDefault();
                
                var images = _context.Images.Where(x => x.PropertyId == property.Id).ToList();
                if (images.Any())
                {
                    property.MainImage = images[0];
                }
                else
                {
                    property.MainImage = new Image();
                }

                if (rentals[i].StartDate.CompareTo(DateTime.Now) > 0)
                {
                    rentals[i].state = "up-coming";
                } else if (rentals[i].EndDate.CompareTo(DateTime.Now) < 0)
                {
                    rentals[i].state = "past";
                } else if (rentals[i].EndDate > DateTime.Now && rentals[i].StartDate < DateTime.Now)
                {
                    rentals[i].state = "active";
                }

                rentals[i].totalNumberOfDays = (rentals[i].EndDate - rentals[i].StartDate).TotalDays;
                rentals[i].totalNumberOfDays = Math.Round(rentals[i].totalNumberOfDays, 2);
                
                rentals[i].totalPrice = property.PricePerDay * rentals[i].totalNumberOfDays;
                rentals[i].totalPrice = Math.Round(rentals[i].totalPrice, 2);

                rentals[i].Property = property;
            }
            return View(rentals);
        }

        // GET: UserPropertyRents/Details/5
        [Authorize(Roles = "PropertyAdministrators")]
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
        [Authorize]
        public IActionResult Create(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            
            ViewBag.PropertyId = id;
            Property property = _context.Property.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.Property_PricePerDay = property.PricePerDay;

            return View();
        }
        [HttpGet]
        [Authorize]
        public List<List<DateTime>> OnGetRentedRangesByPropertyId()
        {
            int id = Int32.Parse(Request.Query["propertyId"]);

            List<UserPropertyRent> propertyRents = _context.UserPropertyRents.Where(x => x.PropertyId == id).ToList();
            var reservedRanges = new List<List<DateTime>>();

            for (int i = 0; i < propertyRents.Count; i++)
            {
                List<DateTime> range = new List<DateTime>();
                range.Add(propertyRents[i].StartDate);
                range.Add(propertyRents[i].EndDate);
                reservedRanges.Add(range);
            }

            return reservedRanges;
        }

        [AcceptVerbs("GET", "POST")]
        [Authorize]
        public IActionResult VerifyDateRangeAvailability(int PropertyId, DateTime StartDate, DateTime EndDate)
        {
            List<UserPropertyRent> propertyRents = _context.UserPropertyRents.Where(x => x.PropertyId == PropertyId).ToList();
            var reservedRanges = new List<List<DateTime>>();

            for (int i = 0; i < propertyRents.Count; i++)
            {
                List<DateTime> range = new List<DateTime>();
                range.Add(propertyRents[i].StartDate);
                range.Add(propertyRents[i].EndDate);
                reservedRanges.Add(range);
            }

            for (int i = 0; i < reservedRanges.Count; i++)
            {
                bool overlap = reservedRanges[i][0] < EndDate && StartDate < reservedRanges[i][1];
                if (overlap)
                {
                    return Json($"Check-in Check-out range is not available");
                }
            }

            return Json(true);
        }

        // POST: UserPropertyRents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("RenterId,PropertyId,CreatedDate,StartDate,EndDate")] UserPropertyRent userPropertyRent)
        {
            _context.Add(userPropertyRent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyRentals), new { id = userPropertyRent.RenterId });
        }

        // GET: UserPropertyRents/Edit/5
        [Authorize]
        [Authorize(Roles = "PropertyAdministrators")]
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
        [Authorize]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PropertyAdministrators")]
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
        [Authorize]
        [Authorize(Roles = "PropertyAdministrators")]
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

        [Authorize]
        public void CancelRental()
        {
            var id = Int32.Parse(Request.Query["rentalId"]);

            var rental = _context.UserPropertyRents.Find(id);
            if (rental != null)
            {
                _context.UserPropertyRents.Remove(rental);
            }

            _context.SaveChanges();
        }

        // POST: UserPropertyRents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        [Authorize(Roles = "PropertyAdministrators")]
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

        [Authorize]
        [Authorize(Roles = "PropertyAdministrators")]
        private bool UserPropertyRentExists(int id)
        {
          return _context.UserPropertyRents.Any(e => e.Id == id);
        }
    }
}
