using CVGS.Data;
using CVGS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Controllers
{

    [AllowAnonymous]
    public class LoginController : ContextController
    {

       public LoginController(DBContext context): base(context)
        {
            
        }

        public IActionResult Index()
        {

            Debug.WriteLine("USER ID " + HttpContext.Session.GetString("USER_ID"));
             
            return View();
        }

        [HttpPost]
        public ActionResult LoginForm(User user)
        {
            //TODO: login logic
            Debug.WriteLine("Loggin In: " + user.Email + ", " + user.Password);
            HttpContext.Session.SetInt32("USER_ID", user.ID);
            return View("Index");
        }

        void loginUser()
        {
            //Sets the session variable key as USER_ID
            //Sets the value to the user id
            HttpContext.Session.SetInt32("USER_ID", 1);
        }
    }
}


class SessionContext
{

    User user;
}