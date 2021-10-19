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
        String question;
        String answer;

        public Captcha(String question, String answer)
        {
            this.question = question;
            this.answer = answer;
        }
    }

    public class UsersController : ContextController
    {
        private readonly DBContext _context;

        private readonly Captcha[] Captchas = new Captcha[] {
            new Captcha("What is 2 + 2?", "2"),
            new Captcha("What is 4 + 4?", "8")
        };

        private readonly Random random = new Random();

        public UsersController(DBContext context): base(context)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email, Password")] LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {

                var user = await _context.User.FirstOrDefaultAsync((u) => u.Password == vm.Password);
                if (user == null)
                {
                    ViewBag.ErrorMessage = "The email or password is invalid.";
                    return View(vm);
                }

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
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }


        public async Task<IActionResult> ActivateAccount(string verificationToken)
        {

            if (verificationToken == null || verificationToken.Length == 0)
            {
                return NotFound("No verification token!");
            }
            ViewBag.verificationToken = verificationToken;
     
            var user = await _context.User
               .FirstOrDefaultAsync(m => m.VerififcationToken == verificationToken);
            if (user == null)
            {
                return NotFound("No user found!");
            }
            user.VerififcationToken = null;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return RedirectUserByRole(user);
        }



        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Register
        public async Task<IActionResult> Register()
        {
            if (IsLoggedIn())
            {
                return RedirectUserByRole(await GetLoggedInUser());
            }
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("FirstName,LastName,DisplayName,BirthDate,Email,Password,Country,City,PostalCode,Street,Province")] User user)
        {
           
                 user.VerififcationToken = GetVerificationToken();
                //TODO: Send email with this link https://localhost:44346/Users/ActivateAccount?verificationToken={user.verificationToken}
                user.UserRole = "MEMBER";
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,BirthDate,Email,Password,VerififcationToken,Country,City,PostalCode,Street,Province,FavoritePlatform,FavoriteGameCategory")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

       

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.ID == id);
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
    }
}
