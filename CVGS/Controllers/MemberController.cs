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

        public MemberController(DBContext context) : base(context)
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
            if (!IsLoggedIn())
            {
                return LogoutUser();
            }
            ViewBag.CreditCards = await base.context.CreditCard.Where((c) => c.UserId == GetUserLoggedInId()).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreditCard([Bind("CardNumber")] CreditCard card)
        {

            if (!ModelState.IsValid)
            {
                return View(card);
            }

            User u = await GetLoggedInUser();
            card.UserId = u.ID;
            try
            {
                base.context.Add(card);
                await base.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangePassword()
        {
            if (!IsLoggedIn())
            {
                return LogoutUser();
            }
            var user = await GetLoggedInUser();
            if (user == null)
            {
                return NotFound("There was no user found with that verification token!");
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword([Bind("ID, Password")] User u)
        {

            User user = await base.context.User.FirstOrDefaultAsync((uu) => uu.ID == u.ID);


            if (user == null)
            {
                return NotFound("There was no user found.");
            }



            ViewBag.SuccessMessage = "Successfully changed your password! ";

            user.Password = u.Password;
            base.context.Update(user);
            await base.context.SaveChangesAsync();
            return View(user);
        }

        public Address GetAddress(User user, bool shipping)
        {
            var addresses = user.Addresses.ToArray();
            if (shipping && addresses.Length >= 1) 
            {
                return addresses[0];
            } 
            if (!shipping && addresses.Length >= 2)
            {
                return addresses[1];
            }
            return null;
        }

        public async Task<IActionResult> Address()
        {
            if (!IsLoggedIn())
            {
                return LogoutUser();
            }
            User user = await GetLoggedInUser();
            AddressViewModel vm = new AddressViewModel();
            vm.MailingAddress = GetAddress(user, false);
            vm.ShippingAddress = GetAddress(user, true);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Address([Bind("ShippingAddress, MailingAddress, SameAddress")] AddressViewModel vm)
        {

            User u = await GetLoggedInUser();

            foreach(Address a in u.Addresses)
            {
                 base.context.Remove(a);
                await base.context.SaveChangesAsync();
            }

            u.Addresses.Clear();

            u.Addresses.Add(vm.ShippingAddress);
        
            if (vm.SameAddress)
            {
                u.Addresses.Add(vm.ShippingAddress);
            }
            base.context.Update(u);
            await base.context.SaveChangesAsync();
            ViewBag.SuccessMessage = "Successfully updated your address information! " + vm.ShippingAddress.City;
            return View(vm);
        }
    }


}
