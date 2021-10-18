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
            //TODO: session id
            var user = await base.context.User.FirstOrDefaultAsync((u) => u.ID == 1);
            return View(user);
        }
    }
}
