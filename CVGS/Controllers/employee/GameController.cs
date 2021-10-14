using CVGS.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Controllers.employee
{
    public class GameController : ContextController
    {

        public GameController(DBContext context) : base(context)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
