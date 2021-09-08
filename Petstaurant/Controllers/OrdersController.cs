﻿using System;
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
    public class OrdersController : Controller
    {
        private readonly PetstaurantContext _context;

        public OrdersController(PetstaurantContext context)
        {
            _context = context;
        }

        // GET: Orders
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var petstaurantContext = _context.Order.Include(o => o.Store).Include(o => o.User);
            return View(await petstaurantContext.ToListAsync());
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Store)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            var u = GetCurrentUserName();
            var t = GetCurrentUserType();
            if ((u != order.UserName) && t != "Admin")
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "Customer")]
        public IActionResult Create()
        {
            ViewData["StoreId"] = new SelectList(_context.Store, nameof(Store.Id), nameof(Store.Address));
            ViewData["UserName"] = new SelectList(_context.User, nameof(Models.User.UserName), nameof(Models.User.UserName));
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create([Bind("Id,CreditCard,StoreId,Address,PostalCode,PhoneNumber,TotalPrice,UserName")] Order order)
        {

            if (ModelState.IsValid)
            {
                //var u = GetCurrentUserName();
                //var cart = await _context.Cart.FirstOrDefaultAsync(s => s.UserName == u);
                //var cartitems = _context.CartItem.ToList().Where(p => p.CartId == cart.Id);
                //if (cartitems.Count() == 0 || cart.TotalPrice == 0)
                //{
                //    ViewData["Error"] = "Your cart is empty; cannot make your order.";
                //    return View(order);
                //}
                //order.UserName = u;
                //order.CartId = cart.Id;
                //order.TotalPrice = cart.TotalPrice;
                //foreach (CartItem cartitem in cartitems)
                //{
                //    if (cartitem != null)
                //    {
                //        OrderItem orderitem = new OrderItem();
                //        orderitem.DishId = cartitem.DishId;
                //        orderitem.Price = cartitem.Price;
                //        orderitem.Quantity = cartitem.Quantity;
                //        if (order.OrderItems == null)
                //        {
                //            order.OrderItems = new List<OrderItem>();
                //        }
                //        order.OrderItems.Add(orderitem);
                //    }
                //}
                _context.Add(order);
             //   await DeleteCartAfterOrder(cart.Id);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoreId"] = new SelectList(_context.Store, nameof(Store.Id), nameof(Store.Address), order.StoreId);
            ViewData["UserName"] = new SelectList(_context.User, nameof(Models.User.UserName), nameof(Models.User.UserName), order.UserName);
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["StoreId"] = new SelectList(_context.Store, nameof(Store.Id), nameof(Store.Address), order.StoreId);
            ViewData["UserName"] = new SelectList(_context.User, nameof(Models.User.UserName), nameof(Models.User.UserName), order.UserName);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreditCard,StoreId,Address,PostalCode,PhoneNumber,TotalPrice,UserName,OrderTime")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StoreId"] = new SelectList(_context.Store, nameof(Store.Id), nameof(Store.Address), order.StoreId);
            ViewData["UserName"] = new SelectList(_context.User, nameof(Models.User.UserName), nameof(Models.User.UserName), order.UserName);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Store)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

        [Authorize(Roles = "Admin")]
        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }


        	
        [Authorize(Roles = "Customer")]
        public async Task DeleteCartAfterOrder(int id)
        {
            var u = GetCurrentUserName();
            var user = _context.User.FirstOrDefault(c => c.UserName == u);
            var cartToDelete = _context.Cart.FirstOrDefault(c => c.UserName == u);
            if (id != cartToDelete.Id || cartToDelete == null || user == null)
	
            {
                return;
            }
            _context.Cart.Remove(cartToDelete);
            user.Cart = new Cart();
        }
}
}
