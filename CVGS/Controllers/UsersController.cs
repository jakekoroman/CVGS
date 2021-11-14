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

    public class Captcha
    {
        public int id;
        public String question;
        public String answer;

        public Captcha(int id, String question, String answer)
        {
            this.id = id;
            this.question = question;
            this.answer = answer;
        }
    }

    public class UsersController : ContextController
    {
        private readonly DBContext _context;

        private readonly Captcha[] Captchas = new Captcha[] {
            new Captcha(1, "What is 2 + 2?", "4"),
            new Captcha(2, "What is 4 + 4?", "8")
        };

        private readonly Random random = new Random();

        public UsersController(DBContext context) : base(context)
        {
            _context = context;
        }

        public Captcha GetRandomCaptcha()
        {
            return Captchas[random.Next(Captchas.Length)];
        }

        public async Task<IActionResult> Login()
        {
            if (IsLoggedIn())
            {
                return RedirectUserByRole(await GetLoggedInUser());
            }
            return View();
        }

        private async Task<bool> AddLoginAttempt(User user)
        {
            user.LoginAttempts = user.LoginAttempts + 1;

            //If attempts hit 3 then set the account to being locked out by giving it a timestamp, in order to login again it must be 60 seconds after the timestamp
            if (user.LoginAttempts == 3)
            {
                user.LockedOut = DateTime.Now;
            }
            base.context.Update(user);
            await base.context.SaveChangesAsync();
            return true;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email, Password")] LoginViewModel vm)
        {

            if (ModelState.IsValid)
            {


                var user = await _context.User.FirstOrDefaultAsync((u) => u.Email == vm.Email);

                if (user == null)
                {
                    ViewBag.ErrorMessage = "There is no user account with that email.";
                    return View(vm);
                }

                if (user.LoginAttempts >= 3 && user.LockedOut != null)
                {
                    TimeSpan timeDiff = DateTime.Now - user.LockedOut;
                    if (timeDiff.TotalSeconds < 60)
                    {
                        ViewBag.ErrorMessage = "This account is currently locked from anymore login attempts.";
                        return View(vm);
                    }
                }

                if (vm.Password != user.Password)
                {
                    await AddLoginAttempt(user);
                    ViewBag.ErrorMessage = "The password is incorrect.";
                    return View(vm);
                }

                if (user.VerififcationToken != null)
                {
                    ViewBag.ErrorMessage = "This account is waiting to be activated.";
                    return View(vm);
                }

                user.LoginAttempts = 0;
                base.context.Update(user);
                await base.context.SaveChangesAsync();

                return RedirectUserByRole(user);
            }
            return View(vm);
        }

        public IActionResult Logout()
        {
            LogoutUser();
            return RedirectToAction("Login");
        }

        // GET: Users
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> ActivateAccount(string verificationToken)
        {

            if (verificationToken == null || verificationToken.Length == 0)
            {
                return NotFound("No verification token!");
            }
            ViewBag.verificationToken = verificationToken;

            var user = await _context.User
               .FirstOrDefaultAsync(m => m.VerififcationToken == verificationToken.Replace(" ", "+"));
            if (user == null)
            {
                return NotFound("No user found!");
            }
            user.VerififcationToken = null;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return View(user);
        }



        // GET: Users/Register
        public async Task<IActionResult> Register()
        {
            if (IsLoggedIn())
            {
                return RedirectUserByRole(await GetLoggedInUser());
            }
            User user = new User();
            Captcha captcha = GetRandomCaptcha();
            user.CaptchaId = captcha.id;
            user.CaptchaQuestion = captcha.question;
            return View(user);
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("FirstName,LastName,DisplayName,BirthDate,Email,Password,CaptchaId,CaptchaQuestion,CaptchaAnswer,JoinDate")] User user)
        {


            Captcha captcha = Captchas.FirstOrDefault((c) => c.id == user.CaptchaId);

            if (captcha == null)
            {
                ViewBag.ErrorMessage = "Missing captcha!" + user.CaptchaId + ", " + user.CaptchaAnswer;
                return View(user);
            }

            if (user.CaptchaAnswer != captcha.answer)
            {
                captcha = GetRandomCaptcha();
                user.CaptchaId = captcha.id;
                user.CaptchaQuestion = captcha.question;
                ViewBag.ErrorMessage = "The captcha answer is incorrect.";
                return View(user);
            }

            if (await base.context.User.FirstOrDefaultAsync((u) => u.Email.ToLower() == user.Email.ToLower()) != null)
            {
                ViewBag.ErrorMessage = "That email is already taken.";
                return View(user);
            }

            if (await base.context.User.FirstOrDefaultAsync((u) => u.DisplayName == user.DisplayName) != null)
            {
                ViewBag.ErrorMessage = "That display name is already taken.";
                return View(user);
            }

            if (await base.context.User.FirstOrDefaultAsync((u) => u.Email == user.Email) != null)
            {
                ViewBag.ErrorMessage = "That email is already taken.";
                return View(user);
            }

            user.Email = user.Email.ToLower();

            user.VerififcationToken = GetVerificationToken();
            SendEmail(user.Email, "Account Activation", "Welcome, " + user.FirstName + ", Please activate your account by visitng the url: https://localhost:44346/Users/ActivateAccount?verificationToken=" + user.VerififcationToken);
            user.UserRole = "MEMBER";
            user.JoinDate = DateTime.UtcNow;
            _context.Add(user);
            await _context.SaveChangesAsync();

            // Creates the new users friends and family list
            // Has to be done after it is pushed into the db so the ID can be populated
            int userId = _context.User.Where(x => x.Email == user.Email).FirstOrDefault().Id;
            FriendList fr = new FriendList();
            FamilyList fl = new FamilyList();
            fr.UserId = userId;
            fl.UserId = userId;
            _context.Add(fr);
            _context.Add(fl);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        public String GetVerificationToken()
        {
            string token;
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);

                token = Convert.ToBase64String(tokenData);
            }
            return token;
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword([Bind("Email")] User u)
        {
            var user = await base.context.User.FirstOrDefaultAsync((us) => us.Email.ToLower() == u.Email.ToLower());
            if (user == null)
            {
                ViewBag.ErrorMessage = "There was no account found with that email.";
                return View();
            }
            user.VerififcationToken = GetVerificationToken();
            base.context.Update(user);
            await base.context.SaveChangesAsync();
            ViewBag.SuccessMessage = "Please check your email with instructions on how to reset your password.";
            SendEmail(user.Email, "Password Reset", "Please visit this link in order to change your password: https://localhost:4436/Users/VerifyChangePassword?verificationToken=" + user.VerififcationToken);
            return View(user);
        }

        public async Task<IActionResult> VerifyChangePassword(string verificationToken)
        {

            if (verificationToken == null)
            {
                return NotFound("No verification token provided!");
            }
            var user = await base.context.User.FirstOrDefaultAsync((u) => u.VerififcationToken == verificationToken.Replace(" ", "+"));

            if (user == null)
            {
                return NotFound("There was no user found with that verification token!");
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyChangePassword([Bind("Id, VerificationToken, Password")] User u)
        {

            User user = await base.context.User.FirstOrDefaultAsync((uu) => uu.Id == u.Id);


            if (user == null)
            {
                return NotFound("There was no user found.");
            }

            if (user.VerififcationToken == null)
            {
                return NotFound("Verification token  invalid.");
            }


            ViewBag.SuccessMessage = "Successfully changed your password! ";


            user.VerififcationToken = null;
            user.Password = u.Password;
            base.context.Update(user);
            await base.context.SaveChangesAsync();
            return View(user);
        }

    }
}
