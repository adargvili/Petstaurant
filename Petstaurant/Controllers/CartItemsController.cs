using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Petstaurant.Data;
using Petstaurant.Models;

namespace Petstaurant.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly PetstaurantContext _context;

        public CartItemsController(PetstaurantContext context)
        {
            _context = context;
        }

        // GET: CartItems
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var petstaurantContext = _context.CartItem.Include(c => c.Cart).Include(c => c.Dish);
            return View(await petstaurantContext.ToListAsync());
        }

        private bool CartItemExists(int id)
        {
            return _context.CartItem.Any(e => e.Id == id);
        }
    }
}
