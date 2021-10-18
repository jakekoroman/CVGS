using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CVGS.Data;
using CVGS.Models;

namespace CVGS.Controllers
{
    public class EventController : ContextController
    {

        public EventController(DBContext context): base(context)
        {

        }

        public async Task<IActionResult> Index()
        {
            return View(await base.context.Event.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Name, Date")] Event e)
        {
            if (ModelState.IsValid)
            {
                base.context.Add(e);
                await base.context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(e);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var e = await base.context.Event
                .FirstOrDefaultAsync(m => m.Id == id);
            if (e == null)
            {
                return NotFound();
            }

            return View(e);
        }

     
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await base.context.Event.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Date")] Event e)
        {
            if (id != e.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    base.context.Update(e);
                    await base.context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(e.Id))
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
            return View(e);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await base.context.Event
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await base.context.Event.FindAsync(id);
           base.context.Event.Remove(employee);
            await base.context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool EventExists(int id)
        {
            return base.context.Event.Any(e => e.Id == id);
        }
    }
}
