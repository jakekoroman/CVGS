using CVGS.Data;
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

        public IActionResult Index()
        {
            return View();
        }

      
        public ActionResult Report()
        {
            return View("~/Views/Employee/Report/");
        }
    }
}
