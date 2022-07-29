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

namespace I3302_RentLo_finals_project.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Images/Delete/5
        [Authorize]
        public void Delete()
        {
            var id = Int32.Parse(Request.Query["imageId"]);
            if (_context.Images == null)
            {
                return;
            }

            var image = _context.Images.Find(id);
            if (image != null)
            {
                _context.Images.Remove(image);
            }

            _context.SaveChanges();
        }

    }
}
