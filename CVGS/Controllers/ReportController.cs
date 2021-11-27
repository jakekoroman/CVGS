using CVGS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVGS.Models;
using ClosedXML.Excel;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using System.Dynamic;

namespace CVGS.Controllers
{
    public class ReportController : ContextController
    {

        public ReportController(DBContext context) : base(context)
        {
        }

        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn())
            {
                return LogoutUser();
            }

            User user = await GetLoggedInUser();
            if (!user.IsEmployee())
            {
                return RedirectToAction("Index", "Employee");
            }
            return View(user);
        }

        public async Task<IActionResult> GameList()
        {
            return View(await base.context.Game.ToListAsync());
        }

        public async Task<IActionResult> GameDetail()
        {
            return View(await base.context.Game.ToListAsync());
        }

        public async Task<IActionResult> MemberList()
        {
            return View(await base.context.User.ToListAsync());
        }

        public async Task<IActionResult> MemberDetail()
        {
            return View(await base.context.User.ToListAsync());
        }

        public async Task<IActionResult> WishList()
        {
            dynamic model = new ExpandoObject();
            model.Users = await context.User.ToListAsync();
            model.WishList = await context.WishList.ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Sales()
        {
            dynamic model = new ExpandoObject();
            model.Games = await context.Game.ToListAsync();
            model.Sales = await context.Sales.ToListAsync();
            return View(model);
        }
    }
}
