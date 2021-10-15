using CVGS.Data;
using CVGS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Controllers
{
 
    public class RegisterController : ContextController
    {
        public RegisterController(DBContext context) : base(context)
        {

        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult RegisterForm(User user)
        {
            //TODO: login logic
            Debug.WriteLine("Loggin In: " + user.Email + ", " + user.Password);
            HttpContext.Session.SetInt32("USER_ID", user.ID);
            return View("Index");
        }

    }
}
