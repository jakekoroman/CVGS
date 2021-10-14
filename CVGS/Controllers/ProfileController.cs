using CVGS.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Controllers
{
    public class ProfileController : Controller
    {

        private readonly CVGSContext _context;

        public ProfileController(CVGSContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            ViewBag.LoggedIn = true;
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
