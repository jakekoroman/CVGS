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
using System.Net.Mail;

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
            return await context.User.FirstOrDefaultAsync((u) => u.Id == GetUserLoggedInId());
        }

        public void LoginUser(User user)
        {
            //Set login session
            HttpContext.Session.SetInt32("user_id", user.Id);
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

        //prog3050visionary
        //Prog3050Visionary___1234

        public void SendEmail(String to, String subject, String body)
        {
            String from = "prog3050visionary@gmail.com";
            MailMessage mail = new MailMessage();
            mail.To.Add(to);
            mail.From = new MailAddress(from);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential(from, "Prog3050Visionary___1234"); // Enter seders User name and password  
            smtp.EnableSsl = true;
            smtp.Send(mail);

        }
    }
}
