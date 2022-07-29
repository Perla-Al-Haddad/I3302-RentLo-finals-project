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
using Microsoft.AspNetCore.Identity;
using I3302_RentLo_finals_project.Authorization;

namespace I3302_RentLo_finals_project.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<ApplicationUser> _userManager;


        public PropertiesController(
            ApplicationDbContext context,
            IHostingEnvironment environment,
            IAuthorizationService authorizationService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this._environment = environment;
            this._authorizationService = authorizationService;
            this._userManager = userManager;
        }

        // GET: Properties
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString, int PropertyCategory, int PropertyCountry, int PropertyCity)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["PropertyCountry"] = PropertyCountry;

            IQueryable<Property> properties = _context.Property;
            List<Property> propertiesList;

            if (!String.IsNullOrEmpty(searchString))
            {
                properties = properties.Where(s => s.PropertyTitle.Contains(searchString)
                                       || s.PropertyDescription.Contains(searchString));
            }
            if (PropertyCategory != 0)
            {
                properties = properties.Where(s => s.CategoryId == PropertyCategory);
            }
            if (PropertyCountry != 0)
            {
                var countryCities = from m in _context.Cities
                                    where m.CountryId == PropertyCountry
                                    select m.Id;
                List<int> cityIds = countryCities.ToList();
                properties = properties.Where(s => cityIds.Contains(s.CityId));
            }
            if (PropertyCity != 0)
            {
                properties = properties.Where(s => s.CityId == PropertyCity);
            }

            propertiesList = properties.ToList();
            propertiesList.Reverse();

            for (int i = 0; i < propertiesList.Count(); i++)
            {
                var property_id = propertiesList[i].Id;

                Category category = _context.Categories.Where(x => x.Id == propertiesList[i].CategoryId).FirstOrDefault();
                propertiesList[i].Category = category;

                var images = _context.Images.Where(x => x.PropertyId == property_id).ToList();
                if (images.Any())
                {
                    propertiesList[i].MainImage = images[0];
                }
                else
                {
                    propertiesList[i].MainImage = new Image();
                }
            }

            var firstCountry = _context.Countries.FirstOrDefault();

            var PropertyListModel = new PropertyListModel
            {
                Properties = propertiesList,
                Categories = await _context.Categories.Distinct().ToListAsync(),
                Countries = await _context.Countries.Distinct().ToListAsync(),
                Cities = await _context.Cities.Where(x => x.CountryId == PropertyCountry).Distinct().ToListAsync()
            };

            return View(PropertyListModel);
        }

        // GET: My Properties
        [Authorize(Roles = "PropertyAdministrators,PropertyManagers")]
        public async Task<IActionResult> MyProperties(string id)
        {
            var properties = _context.Property.Where(x => x.CreatorId == id).ToList();
            for (int i = 0; i < properties.Count(); i++)
            {
                var property_id = properties[i].Id;

                Category category = _context.Categories.Where(x => x.Id == properties[i].CategoryId).FirstOrDefault();
                properties[i].Category = category;

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
            properties.Reverse();
            return View(properties);
        }

        // GET: Properties/Details/5
        [AllowAnonymous]
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
        [Authorize(Roles = "PropertyAdministrators,PropertyManagers")]
        public IActionResult Create()
        {
            IEnumerable<Category> categoriesList = _context.Categories;
            ViewBag.categoriesList = categoriesList;
            IEnumerable<Country> countriesList = _context.Countries;
            ViewBag.countriesList = countriesList;
            IEnumerable<City> citiesList = _context.Cities.Where(x => x.CountryId == 1).ToList();
            ViewBag.citiesList = citiesList;
            return View();
        }
        [HttpGet]
        public IEnumerable<City> OnGetCitiesByCountryId()
        {
            if (Request.Query["countryId"] != "")
            {
                int countryId = Int32.Parse(Request.Query["countryId"]);
                var cities = _context.Cities.Where(x => x.CountryId == countryId);
                return cities;
            }
            return _context.Cities;
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PropertyAdministrators,PropertyManagers")]
        public async Task<IActionResult> Create([Bind("Id,PropertyTitle,PropertyDescription,ImagePaths,PricePerDay,CreatorId,CategoryId,CityId")] Property @property)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@property);
                await _context.SaveChangesAsync();

                int property_id = property.Id;

                ICollection<IFormFile> images = @property.ImagePaths;

                if (images != null)
                {
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
                }

                return RedirectToAction(nameof(MyProperties), new {id = property.CreatorId});
            }
            return View(@property);
        }

        [Authorize(Roles = "PropertyAdministrators,PropertyManagers")]
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

            var isAuthorized = _authorizationService.AuthorizeAsync(
                                            User, @property,
                                            PropertyOperations.Update);

            IEnumerable<Category> categoriesList = _context.Categories;
            ViewBag.categoriesList = categoriesList;
            IEnumerable<Country> countriesList = _context.Countries;
            ViewBag.countriesList = countriesList;
            IEnumerable<City> citiesList = _context.Cities.Where(x => x.CountryId == 1).ToList();
            ViewBag.citiesList = citiesList;

            ViewBag.creatorId = @property.CreatorId;

            try
            {
                var images = _context.Images.Where(x => x.PropertyId == property.Id).ToList();
                ViewBag.PropertyImages = images;
            }
            catch (Exception ex)
            {
                ViewBag.PropertyImages = null;
            }

            if (!isAuthorized.Result.Succeeded)
            {
                return Forbid();
            }

            return View(@property);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PropertyAdministrators,PropertyManagers")]
        public IActionResult Edit([Bind("Id,PropertyTitle,PropertyDescription,ImagePaths,PricePerDay,CreatorId,CategoryId,CityId")] Property obj)
        {
            var isAuthorized = _authorizationService.AuthorizeAsync(
                                            User, obj,
                                            PropertyOperations.Update);

            if (!isAuthorized.Result.Succeeded)
            {
                return Forbid();
            }

            ICollection<IFormFile> images = obj.ImagePaths;
            //var images = _context.Images.Where(x => x.PropertyId == property.Id).ToList();

            if (images != null)
            {
                foreach (IFormFile image in images)
                {
                    // Copy image to server
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "img", "uploaded", "properties");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    image.CopyTo(new FileStream(filePath, FileMode.Create));

                    // Create Image in database
                    Image new_image = new Image();
                    new_image.PropertyId = obj.Id;
                    new_image.ImagePath = Path.Combine("img", "uploaded", "properties", uniqueFileName);

                    _context.Images.Add(new_image);
                }
            }

            _context.Property.Update(obj);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "PropertyAdministrators,PropertyManagers")]
        // GET: Properties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Property == null)
            {
                return NotFound();
            }

            var @property = await _context.Property
                .FirstOrDefaultAsync(m => m.Id == id);

            var isAuthorized = _authorizationService.AuthorizeAsync(
                                            User, @property,
                                            PropertyOperations.Delete);

            if (!isAuthorized.Result.Succeeded)
            {
                return Forbid();
            }

            if (@property == null)
            {
                return NotFound();
            }

            return View(@property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PropertyAdministrators,PropertyManagers")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Property == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Property'  is null.");
            }
            var @property = await _context.Property.FindAsync(id);

            var isAuthorized = _authorizationService.AuthorizeAsync(
                                            User, @property,
                                            PropertyOperations.Delete);

            if (!isAuthorized.Result.Succeeded)
            {
                return Forbid();
            }

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
