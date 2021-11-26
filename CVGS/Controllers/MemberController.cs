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
        public async Task<IActionResult> Profile([Bind("Id,FirstName, LastName, Gender, BirthDate, ReceivePromomotionalEmails")] User user)
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
        public async Task<IActionResult> Preferences([Bind("Id,FavoritePlatform, FavoriteGameCategory")] User user)
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
            card.UserId = u.Id;
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
        public async Task<IActionResult> ChangePassword([Bind("Id, Password")] User u)
        {

            User user = await base.context.User.FirstOrDefaultAsync((uu) => uu.Id == u.Id);


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
            var addresses = base.context.Address.Where((a) => a.UserId == user.Id).ToArray();
            if (shipping && addresses.Length >= 1)
            {
                return addresses[0];
            }
            if (!shipping && addresses.Length >= 2)
            {
                return addresses[1];
            }
            return new Address();
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

        public async Task<dynamic> GetReportData(int type)
        {
            switch (type)
            {
                case 1:
                    return await base.context.Event.ToListAsync();
                case 2:
                    return await base.context.Game.ToListAsync();
                default:
                    return null;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Address([Bind("ShippingAddress, MailingAddress, SameAddress")] AddressViewModel vm)
        {

            User u = await GetLoggedInUser();

            var addresses = base.context.Address.Where((a) => a.UserId == u.Id).ToArray();
            foreach (Address a in addresses)
            {
                base.context.Remove(a);
                await base.context.SaveChangesAsync();
            }


            vm.ShippingAddress.UserId = u.Id;
            vm.MailingAddress.UserId = u.Id;

            if (vm.SameAddress)
            {
                vm.MailingAddress.City = vm.ShippingAddress.City;
                vm.MailingAddress.Country = vm.ShippingAddress.Country;
                vm.MailingAddress.PostalCode = vm.ShippingAddress.PostalCode;
                vm.MailingAddress.Province = vm.ShippingAddress.Province;
                vm.MailingAddress.Street = vm.ShippingAddress.Street;
            }
            if (vm.ShippingAddress.Id == 0)
            {
                base.context.Add(vm.ShippingAddress);
            }
            else
            {
                base.context.Update(vm.ShippingAddress);
            }
            if (vm.MailingAddress.Id == 0)
            {
                base.context.Add(vm.MailingAddress);
            }
            else
            {
                base.context.Update(vm.MailingAddress);
            }

            await base.context.SaveChangesAsync();
            ViewBag.SuccessMessage = "Successfully updated your address information!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> OtherUserWishList(int? userId)
        {
            ViewBag.Name = context.User.Where(x => x.Id == userId).FirstOrDefault().DisplayName;
            return View(await context.WishList.Where(x => x.UserId == userId).ToListAsync());
        }

        public async Task<IActionResult> WishList()
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

            ViewBag.WishList = context.WishList.Where(x => x.UserId == user.Id);

            ViewData["GameNames"] = new SelectList(context.Game.Select(x => x.Name));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> WishList([Bind("Id, GameId, UserId, GameName")] WishList wishList)
        {
            User u = await GetLoggedInUser();
            var dbContext = context.WishList.Where(x => x.UserId == u.Id);
            wishList.UserId = u.Id;
            wishList.GameId = context.Game.Where(x => x.Name == wishList.GameName).FirstOrDefault().Id;

            try
            {
                base.context.Update(wishList);
                await base.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            ViewData["GameNames"] = new SelectList(context.Game.Select(x => x.Name));
            ViewBag.WishList = context.WishList.Where(x => x.UserId == u.Id);

            return View();
        }

        public async Task<IActionResult> DeleteWishListItem(int? id)
        {
            User u = await GetLoggedInUser();

            var wishList = await context.WishList.FindAsync(id);
            context.WishList.Remove(wishList);
            await context.SaveChangesAsync();

            ViewData["GameNames"] = new SelectList(context.Game.Select(x => x.Name));
            ViewBag.WishList = context.WishList.Where(x => x.UserId == u.Id);
            return RedirectToAction("WishList");
        }

        private List<String> GetFriendUserNames(User user)
        {
            var friendList = context.FriendList.Where(x => x.UserId == user.Id).FirstOrDefault();
            List<Friend> currentFriends = context.Friend.Where(x => x.FriendListId == friendList.Id).ToList();

            var userNames = context.User.Select(x => x.DisplayName).ToList();
            userNames.Remove(user.DisplayName);

            foreach (Friend f in currentFriends)
            {
                userNames.Remove(f.UserName);
            }

            return userNames.ToList();
        }

        public async Task<IActionResult> AddFriend()
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

            ViewData["UserNames"] = new SelectList(GetFriendUserNames(user));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFriend([Bind("Id, FriendListId, UserId, UserName")] Friend friend)
        {
            User u = await GetLoggedInUser();

            int friendUserId = context.User.Where(x => x.DisplayName == friend.UserName).FirstOrDefault().Id;
            int friendListId = context.FriendList.Where(x => x.UserId == u.Id).FirstOrDefault().Id;

            friend.UserId = friendUserId;
            friend.FriendListId = friendListId;

            try
            {
                base.context.Update(friend);
                await base.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            ViewData["UserNames"] = new SelectList(GetFriendUserNames(u));

            return View();
        }

        public async Task<IActionResult> Friends()
        {
            User user = await GetLoggedInUser();
            var friendList = context.FriendList.Where(x => x.UserId == user.Id).FirstOrDefault();
            List<Friend> currentFriend = context.Friend.Where(x => x.FriendListId == friendList.Id).ToList();
            return View(currentFriend);
        }

        private List<String> GetFamilyUserNames(User user)
        {
            var familyList = context.FamilyList.Where(x => x.UserId == user.Id).FirstOrDefault();
            List<Family> currentFamily = context.Family.Where(x => x.FamilyListId == familyList.Id).ToList();

            var userNames = context.User.Select(x => x.DisplayName).ToList();
            userNames.Remove(user.DisplayName);

            foreach (Family f in currentFamily)
            {
                userNames.Remove(f.UserName);
            }

            return userNames.ToList();
        }

        public async Task<IActionResult> AddFamily()
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

            ViewData["UserNames"] = new SelectList(GetFamilyUserNames(user));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFamily([Bind("Id, FamilyListId, UserId, UserName")] Family family)
        {
            User u = await GetLoggedInUser();

            int familyUserId = context.User.Where(x => x.DisplayName == family.UserName).FirstOrDefault().Id;
            int familyListId = context.FamilyList.Where(x => x.UserId == u.Id).FirstOrDefault().Id;

            family.UserId = familyUserId;
            family.FamilyListId = familyListId;

            try
            {
                base.context.Update(family);
                await base.context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            ViewData["UserNames"] = new SelectList(GetFamilyUserNames(u));

            return View();
        }

        public async Task<IActionResult> Family()
        {
            User user = await GetLoggedInUser();
            var familyList = context.FamilyList.Where(x => x.UserId == user.Id).FirstOrDefault();
            List<Family> currentFamily = context.Family.Where(x => x.FamilyListId == familyList.Id).ToList();
            return View(currentFamily);
        }

        public async Task<IActionResult> Games()
        {
            SearchGameViewModel model = new SearchGameViewModel();
            model.Games = await context.Game.ToListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Games([Bind("Name")] SearchGameViewModel model)
        {
            model.Games = await context.Game.Where((game) => game.Name.ToLower().Contains(model.Name)).ToListAsync();
            if (model.Name.Length == 0)
            {
                model.Games = await context.Game.ToListAsync();
            }
            return View(model);
        }

        public async Task<IActionResult> ViewGame(int? id)
        {
            Game game = await base.context.Game.FirstOrDefaultAsync((game) => game.Id == id);
            game.UserGameRating = await base.context.GameRatings.FirstOrDefaultAsync((rating) => rating.GameID == id && rating.UserId == GetUserLoggedInId());

            List<GameRatings> ratings = await base.context.GameRatings.Where((game) => game.GameID == id).ToListAsync();

            double total = 0;
            foreach (GameRatings r in ratings)
            {
                total = r.Rating;
            }
            game.OverAllRating = total <= 0 ? 0 : total / ratings.Count;
            return View(game);
        }


        public IActionResult AddGameRating(int id)
        {

            GameRatings rating = new GameRatings();
            rating.UserId = GetUserLoggedInId();
            rating.GameID = id;
            return View(rating);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddGameRating([Bind("UserId, GameID, Rating")] GameRatings r)
        {
            base.context.Add(r);
            await base.context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EditGameRating(int? id)
        {
            GameRatings rating = await base.context.GameRatings.FirstOrDefaultAsync((rating) => rating.GameID == id && rating.UserId == GetUserLoggedInId());

            return View(rating);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGameRating([Bind("Id, UserId, GameID, Rating")] GameRatings r)
        {
            base.context.Update(r);
            await base.context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private List<String> GetAvailableEvents(User user)
        {
            List<int> registeredEventIds = context.EventRegistry.Where(x => x.UserId == user.Id).Select(x => x.EventId).ToList();
            List<Event> events = context.Event.ToList();
            List<Event> registeredEvents = new List<Event>();

            foreach (int eid in registeredEventIds)
            {
                registeredEvents.Add(context.Event.Where(x => x.Id != eid).FirstOrDefault());
            }

            foreach (Event e in registeredEvents)
            {
                events.Remove(e);
            }

            return events.Select(x => x.Name).ToList();
        }

        public async Task<IActionResult> RegisterForEvent()
        {
            User u = await GetLoggedInUser();
            // TODO: Implement
            // Add select list full of events
            // Can't register for the same event more than once
            // Make the view
            
            ViewData["EventNames"] = new SelectList(GetAvailableEvents(u));

            return View();
        }

    }
}
