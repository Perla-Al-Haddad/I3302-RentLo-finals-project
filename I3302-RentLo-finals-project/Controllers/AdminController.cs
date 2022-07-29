using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace I3302_RentLo_finals_project.Controllers
{
    [Authorize(Roles = "PropertyAdministrators")]
    public class AdminController : Controller
    {
        // GET: HomeController
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}
