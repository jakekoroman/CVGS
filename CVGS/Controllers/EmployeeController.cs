using CVGS.Data;
using CVGS.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

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

            // Add charge to the sales table
            List<int> gameIds = new List<int>();
            order.OrderItems = await base.context.OrderItems.Where((orderItem) => orderItem.OrderId == order.Id).ToListAsync();

            foreach (OrderItem orderItem in order.OrderItems)
                gameIds.Add(orderItem.GameId);

            // This is probably really slow but i have so much stuff to do it isn't worth my time
            foreach (int gameId in gameIds)
            {
                Sale sale = new Sale();
                sale.OrderId = id;
                sale.GameId = order.OrderItems.Where(x => x.GameId == gameId).FirstOrDefault().GameId;
                sale.Total = context.Game.Where(x => x.Id == gameId).FirstOrDefault().Price;
                context.Update(sale);
            }

            await base.context.SaveChangesAsync();
            return View();
        }
    }
}
