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



namespace CVGS.Controllers
{
    public class MemberController : ContextController

    {

        public MemberController(DBContext context): base(context)
        {

        }
        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn())
            {
                return LogoutUser();
            }
          
            User user = await GetLoggedInUser();
            if (user.IsEmployee())
            {
                return RedirectToAction("Index", "Employee");
            }
            return View(user);
        }

        public async Task<IActionResult> Profile()
        {
            if (!IsLoggedIn())
            {
                return LogoutUser();
            }
            User user = await GetLoggedInUser();
            if (user.IsEmployee())
            {
                return RedirectToAction("Index", "Employee");
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile([Bind("ID,FirstName, LastName, Gender, BirthDate, ReceivePromomotionalEmails")] User user)
        {


            User u = await GetLoggedInUser();
            u.FirstName = user.FirstName;
            u.LastName = user.LastName;
            u.Gender = user.Gender;
            u.BirthDate = user.BirthDate;
            u.ReceivePromomotionalEmails = user.ReceivePromomotionalEmails;
            try
                {
                    base.context.Update(u);
                    await base.context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Preferences()
        {
            if (!IsLoggedIn())
            {
                return LogoutUser();
            }
            User user = await GetLoggedInUser();
            if (user.IsEmployee())
            {
                return RedirectToAction("Index", "Employee");
            }

            ViewData["Platforms"] = new SelectList(CVGS.Models.User.Platforms);
            ViewData["Categories"] = new SelectList(CVGS.Models.Game.Categories);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Preferences([Bind("ID,FavoritePlatform, FavoriteGameCategory")] User user)
        {
            User u = await GetLoggedInUser();
            u.FavoriteGameCategory = user.FavoriteGameCategory;
            u.FavoritePlatform = user.FavoritePlatform;
            try
            {
                base.context.Update(u);
                await base.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CreditCard()
        {
            return View();
        }
    }


}
