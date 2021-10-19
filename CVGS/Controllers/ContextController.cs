using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CVGS.Data;
using CVGS.Models;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace CVGS.Controllers
{
    public class ContextController : Controller
    {
        public readonly DBContext context;

        public ContextController(DBContext context)
        {
            this.context = context;
        }


        public async Task<User> GetLoggedInUser()
        {
            return await context.User.FirstOrDefaultAsync((u) => u.ID == GetUserLoggedInId());
        }

        public void LoginUser(User user)
        {
            //Set login session
            HttpContext.Session.SetInt32("user_id", user.ID);
        }

        public ActionResult LogoutUser()
        {
            //Remove login session
            HttpContext.Session.Remove("user_id");
            return RedirectToAction("Login", "Users");
        }

        public bool IsLoggedIn()
        {
            return HttpContext.Session.GetInt32("user_id") != null;
        }

        public int GetUserLoggedInId()
        {
            return Convert.ToInt32(HttpContext.Session.GetInt32("user_id"));
        }
        public ActionResult RedirectUserByRole(User user)
        {
            LoginUser(user);
            if (user.UserRole == "MEMBER")
            {
                return RedirectToAction("Index", "Member");
            }
            return RedirectToAction("Index", "Employee");
        }
    }
}
