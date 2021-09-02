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

        // GET: CartItems/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cartItem = await _context.CartItem
        //        .Include(c => c.Cart)
        //        .Include(c => c.Dish)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (cartItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(cartItem);
        //}

        // GET: CartItems/Create
        [Authorize(Roles = "Admin, Customer")]
        public IActionResult Create()
        {
            ViewData["CartId"] = new SelectList(_context.Cart, nameof(Cart.Id), nameof(Cart.Id));
            ViewData["DishId"] = new SelectList(_context.Dish, nameof(Dish.Id), nameof(Dish.Name));
            return View();
        }

        // POST: CartItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Create([Bind("Id,CartId,Quantity,Price,DishId")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                var u = GetCurrentUserName();
                var c = _context.Cart.FirstOrDefault(p => p.UserName == u);
                if(c == null)
                {
                    return NotFound();
                }
                cartItem.CartId = c.Id;

                var q = _context.CartItem.FirstOrDefault(u => u.DishId == cartItem.DishId && u.CartId ==cartItem.CartId);
                if (q == null)
                {
                    _context.Add(cartItem);
                    c.CartItems.Add(cartItem);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Carts");
                }
                else
                {
                    q.Quantity += cartItem.Quantity;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details","Carts");
                }
            }

            ViewData["CartId"] = new SelectList(_context.Cart, nameof(Cart.Id), nameof(Cart.Id), cartItem.CartId);
            ViewData["DishId"] = new SelectList(_context.Dish, nameof(Dish.Id), nameof(Dish.Name), cartItem.DishId);
            return View(cartItem);
        }

        //// GET: CartItems/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cartItem = await _context.CartItem.FindAsync(id);
        //    if (cartItem == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CartId"] = new SelectList(_context.Cart, nameof(Cart.Id), nameof(Cart.Id), cartItem.CartId);
        //    ViewData["DishId"] = new SelectList(_context.Dish, nameof(Dish.Id), nameof(Dish.Name), cartItem.DishId);
        //    return View(cartItem);
        //}

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,CartId,Quantity,Price,DishId")] CartItem cartItem)
        //{
        //    if (id != cartItem.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(cartItem);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CartItemExists(cartItem.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CartId"] = new SelectList(_context.Cart, nameof(Cart.Id), nameof(Cart.Id), cartItem.CartId);
        //    ViewData["DishId"] = new SelectList(_context.Dish, nameof(Dish.Id), nameof(Dish.Name), cartItem.DishId);
        //    return View(cartItem);
        //}

        // GET: CartItems/Delete/5
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItem
                .Include(c => c.Cart)
                .Include(c => c.Dish)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }


            var u = GetCurrentUserName();
            var t = GetCurrentUserType();
            if ((u != cartItem.Cart.UserName) && t != "Admin")
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartItem = await _context.CartItem.FindAsync(id);
            _context.CartItem.Remove(cartItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartItemExists(int id)
        {
            return _context.CartItem.Any(e => e.Id == id);
        }


        private string GetCurrentUserName()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var u = claims.First().Value;
            return u;
        }

        private string GetCurrentUserType()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var u = claims.Skip(1).First().Value;
            return u;
        }
    }
}
