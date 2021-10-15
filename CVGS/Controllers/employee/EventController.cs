using CVGS.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVGS.Controllers.employee
{
    public class EventController : ContextController
    {

        public EventController(DBContext context): base(context)
        {

        }

        public IActionResult Index()
        {
            var events = context.Event.ToArray();
            return View(events);
        }



    }
}
