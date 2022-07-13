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
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace I3302_RentLo_finals_project.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public PropertiesController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            this._environment = environment;
        }

        // GET: Properties
        public async Task<IActionResult> Index()
        {
            var properties = _context.Property.ToList();
            for (int i = 0; i < properties.Count(); i++)
            {
                var property_id = properties[i].Id;
                var images = _context.Images.Where(x => x.PropertyId == property_id).ToList();
                if (images.Any())
                {
                    properties[i].MainImage = images[0];
                }
                else
                {
                    properties[i].MainImage = new Image();
                }
            }
            return View(properties);
        }

        // GET: My Properties
        public async Task<IActionResult> MyProperties(string id)
        {
            var properties = _context.Property.Where(x => x.CreatorId == id).ToList();
            for (int i = 0; i < properties.Count(); i++)
            {
                var property_id = properties[i].Id;
                var images = _context.Images.Where(x => x.PropertyId == property_id).ToList();
                if (images.Any())
                {
                    properties[i].MainImage = images[0];
                }
                else
                {
                    properties[i].MainImage = new Image();
                }
            }
            return View(properties);
        }

        // GET: Properties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Property == null)
            {
                return NotFound();
            }

            var @property = await _context.Property
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@property == null)
            {
                return NotFound();
            }

            // fetch the related images to the property
            try
            {
                var images = _context.Images.Where(x => x.PropertyId == property.Id).ToList();
                ViewBag.PropertyImages = images;
            }
            catch (Exception ex)
            {
                ViewBag.PropertyImages = null;
            }

            // fetch the city of the property
            var city = _context.Cities.Where(x => x.Id == property.CityId);
            // fetch the country of the city of the property
            var query = (from c in _context.Cities
                         join cnt in _context.Countries
                         on c.CountryId equals cnt.id
                         where c.Id == property.CityId
                         select new
                         {
                             cnt.CountryName,
                             c.CityName
                         }).FirstOrDefault();

            ViewBag.CityName = query.CityName;
            ViewBag.CountryName = query.CountryName;

            return View(@property);
        }

        // GET: Properties/Create
        [Authorize]
        public IActionResult Create()
        {
            IEnumerable<Category> categoriesList = _context.Categories;
            ViewBag.categoriesList = categoriesList;
            IEnumerable<Country> countriesList = _context.Countries;
            ViewBag.countriesList = countriesList;
            return View();
        }
        [HttpGet]
        public IEnumerable<City> OnGetCitiesByCountryId()
        {
            int countryId = Int32.Parse(Request.Query["countryId"]);
            var cities = _context.Cities.Where(x => x.CountryId == countryId);
            return cities;
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,PropertyTitle,PropertyDescription,ImagePaths,PricePerDay,MaxGuests,NumberOfBeds,NumberOfBathrooms,Favorites,CreatorId,CategoryId,CityId")] Property @property)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@property);
                await _context.SaveChangesAsync();

                int property_id = property.Id;

                ICollection<IFormFile> images = @property.ImagePaths;

                foreach (IFormFile image in images)
                {
                    // Copy image to server
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "img", "uploaded", "properties");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    image.CopyTo(new FileStream(filePath, FileMode.Create));

                    // Create Image in database
                    Image new_image = new Image();
                    new_image.PropertyId = property_id;
                    new_image.ImagePath = Path.Combine("img", "uploaded", "properties", uniqueFileName);

                    _context.Images.Add(new_image);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(@property);
        }

        [Authorize]
        // GET: Properties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Property == null)
            {
                return NotFound();
            }

            var @property = await _context.Property.FindAsync(id);
            if (@property == null)
            {
                return NotFound();
            }
            return View(@property);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PropertyTitle,PropertyDescription,PricePerDay,DateCreated,MaxGuests,NumberOfBeds,NumberOfBathrooms,Favorites")] Property @property)
        {
            if (id != @property.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@property);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PropertyExists(@property.Id))
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
            return View(@property);
        }

        [Authorize]
        // GET: Properties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Property == null)
            {
                return NotFound();
            }

            var @property = await _context.Property
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Property == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Property'  is null.");
            }
            var @property = await _context.Property.FindAsync(id);
            if (@property != null)
            {
                _context.Property.Remove(@property);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PropertyExists(int id)
        {
            return (_context.Property?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
