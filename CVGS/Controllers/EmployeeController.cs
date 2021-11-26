using CVGS.Data;
using CVGS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class EmployeeController : ContextController
    {

        public EmployeeController(DBContext context) : base(context)
        {

        }

        public async Task<IActionResult> Index()
        {
            User user = await GetLoggedInUser();
            if (user.UserRole == "MEMBER")
            {
                return RedirectToAction("Index", "Member");
            }
            return View();
        }

      
        public ActionResult Report()
        {
            return View("~/Views/Employee/Report/");
        }

        public async Task<ActionResult> Orders()
        {
            List<Order> orders = await base.context.Orders.ToListAsync();
            foreach (Order order in orders)
            {
                order.OrderItems = await base.context.OrderItems.Where((orderItem) => orderItem.OrderId == order.Id).ToListAsync();
                order.User = await base.context.User.FirstOrDefaultAsync((user) => user.Id == order.UserId);
            }
            return View(orders);
        }

        public async Task<ActionResult> Process(int id)
        {
            Order order = await base.context.Orders.FirstOrDefaultAsync((order) => order.Id == id);

            order.Status = "Processed";
            base.context.Update(order);
            await base.context.SaveChangesAsync();
            return View();
        }
    }
}
