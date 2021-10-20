﻿using CVGS.Data;
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

namespace CVGS.Controllers
{
    public class ReportController : ContextController
    {

        public ReportController(DBContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            return View();
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
            return View(await base.context.User.ToListAsync());
        }
        public IActionResult Sales()
        {
            return View();
        }

       
        
    }
}
