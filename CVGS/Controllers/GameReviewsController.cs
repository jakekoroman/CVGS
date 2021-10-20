using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CVGS.Data;
using CVGS.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http;

namespace CVGS.Controllers
{
    public class GameReviewsController : Controller
    {
        private readonly DBContext _context;
        private readonly INotyfService _notyf;

        public GameReviewsController(DBContext context, INotyfService notyf)
        {
            _context = context;
            // injects toast support
            _notyf = notyf;
        }

        // GET: GameReviews
        public async Task<IActionResult> Index()
        {
            var dBContext = _context.GameReview.Include(g => g.Game);

            if (HttpContext.Session.GetInt32("approved") == 1)
            {
                _notyf.Success("Approved!", 2);
                HttpContext.Session.Remove("approved");
            }
            else if (HttpContext.Session.GetInt32("approved") == 0)
            {
                _notyf.Error("Deleted!", 2);
                HttpContext.Session.Remove("approved");
            }

            return View(await dBContext.ToListAsync());
        }

        // GET: GameReviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameReview = await _context.GameReview
                .Include(g => g.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameReview == null)
            {
                return NotFound();
            }

            return View(gameReview);
        }

        // GET: GameReviews/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.User, "ID", "ID");
            return View();
        }

        // POST: GameReviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,GameId,UserId")] GameReview gameReview)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameReview);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Name", gameReview.GameId);
            return View(gameReview);
        }

        // GET: GameReviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameReview = await _context.GameReview.FindAsync(id);
            if (gameReview == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Id", gameReview.GameId);
            return View(gameReview);
        }

        // POST: GameReviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,GameId")] GameReview gameReview)
        {
            if (id != gameReview.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameReview);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameReviewExists(gameReview.Id))
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
            ViewData["GameId"] = new SelectList(_context.Game, "Id", "Id", gameReview.GameId);
            return View(gameReview);
        }

        // GET: GameReviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameReview = await _context.GameReview
                .Include(g => g.Game)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gameReview == null)
            {
                return NotFound();
            }

            return View(gameReview);
        }

        // POST: GameReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameReview = await _context.GameReview.FindAsync(id);
            _context.GameReview.Remove(gameReview);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetInt32("approved", 0);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null)
                return NotFound();

            var gameReview = await _context.GameReview.Include(g => g.Game).FirstOrDefaultAsync(m => m.Id == id);

            if (gameReview == null)
            {
                return NotFound();
            }

            gameReview.Approved = true;
            _context.Update(gameReview);
            await _context.SaveChangesAsync();

            // Sets session variable to play the toast notification
            HttpContext.Session.SetInt32("approved", 1);

            return RedirectToAction(nameof(Index));
        }

        private bool GameReviewExists(int id)
        {
            return _context.GameReview.Any(e => e.Id == id);
        }
    }
}
