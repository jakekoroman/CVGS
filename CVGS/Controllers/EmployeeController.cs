using CVGS.Data;
using CVGS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Controllers
{
    public class EmployeeController : ContextController
    {

        public EmployeeController(DBContext context) : base(context)
        {

        }

        public async Task<IActionResult> Index()
        {
            User user = await GetLoggedInUser();
            if (user.UserRole == "MEMBER")
            {
                return RedirectToAction("Index", "Member");
            }
            return View();
        }

      
        public ActionResult Report()
        {
            return View("~/Views/Employee/Report/");
        }
    }
}
