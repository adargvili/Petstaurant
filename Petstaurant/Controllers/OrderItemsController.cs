using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Petstaurant.Data;
using Petstaurant.Models;

namespace Petstaurant.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderItemsController : Controller
    {
        private readonly PetstaurantContext _context;

        public OrderItemsController(PetstaurantContext context)
        {
            _context = context;
        }

        // GET: OrderItems
        //public async Task<IActionResult> Index()
        //{
        //    var petstaurantContext = _context.OrderItem.Include(o => o.Dish).Include(o => o.Order);
        //    return View(await petstaurantContext.ToListAsync());
        //}
        private bool OrderItemExists(int id)
        {
            return _context.OrderItem.Any(e => e.Id == id);
        }
    }
}
