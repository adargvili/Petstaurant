﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
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
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Index()
        {
            var ut = GetCurrentUserType();
            var u = GetCurrentUserName();
            if (ut == "Admin"){
            var petstaurantContext = _context.Order.Include(o => o.Store).Include(o => o.User).OrderBy(o=> o.Id);
                return View(await petstaurantContext.ToListAsync());
            }
            else
            {
                var petstaurantContextUser = _context.Order.Include(o => o.Store).Include(o => o.User).Where(o=> o.UserName ==u).OrderBy(o=>o.Id);
                return View(await petstaurantContextUser.ToListAsync());
            }
            
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "Customer, Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Store)
                .Include(o => o.User).Include(o=> o.OrderItems)
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
            List<Dish> dishes = new List<Dish>();
            foreach(OrderItem o in order.OrderItems)
            {
                var d = await _context.Dish.FindAsync(o.DishId);
                if (d != null)
                {
                    dishes.Add(d);
                }

            }
            ViewData["dishes"] = dishes;

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
                if (order.PostalCode.StartsWith("0"))
                {
                    ViewData["Error"] = "Israeli postal code format is required";
                    return View(order);
                }

                var oa = order.Address.Split();
                bool c = false;
                foreach (string s in oa)
                {
                    if ((!s.Any(x => !char.IsLetter(x)) && s.Any(char.IsDigit))|| (s.Length<2&& !s.Any(char.IsDigit))) {
                        c = true;
                        break;
                    }
                }

                if (c ||!char.IsDigit(order.Address.Last()) || order.Address.StartsWith(" ") || order.Address.EndsWith(" ") || order.Address.Count(f => f == ',')>1 || char.IsDigit(order.Address[0])|| !order.Address.Contains(" ")|| Regex.IsMatch(order.Address, @"\s{2,}")|| !order.Address.Any(char.IsDigit)|| order.Address.All(char.IsDigit)|| order.Address.Split().Length>5|| order.Address.Count(Char.IsWhiteSpace)>5)
                {
                    ViewData["Error"] = "Israeli address format is required";
                    return View(order);
                }
                c = false;
                var u = GetCurrentUserName();
                var cart = await _context.Cart.FirstOrDefaultAsync(s => s.UserName == u);
                var cartitems = _context.CartItem.ToList().Where(p => p.CartId == cart.Id);
                if (cartitems.Count() == 0 || cart.TotalPrice == 0)
                {
                    ViewData["Error"] = "Your cart is empty; cannot make your order.";
                    return View(order);
                }

                var store = await _context.Store.FirstOrDefaultAsync(p => p.Id == order.StoreId);
                if(store== null)
                {
                    ViewData["Error"] = "You have to choose a store.";
                    return View(order);
                }
                order.UserName = u;
                order.TotalPrice = cart.TotalPrice;
                foreach (CartItem cartitem in cartitems)
                {
                    if (cartitem != null)
                    {
                        var dish = _context.Dish.FirstOrDefault(p => p.Id == cartitem.DishId);
                        if (dish == null)
                        {
                            ViewData["Error"] = "There is something wrong with one or more of your items; Please try again";
                            return View(order);
                        }
                        var dishStores = _context.Dish.Where(c => c.Id == dish.Id).SelectMany(c => c.Store).ToList();
                        var check = 0;
                        foreach(Store s in dishStores)
                        {
                            if(s.Id == order.StoreId)
                            {
                                check = 1;
                                break;
                            }
                        }
                        if (check ==0)
                        {
                            ViewData["Error"] = dish.Name + "dish is not available in your city; Please remove this dish from cart and try again";
                            return View(order);
                        }
                        OrderItem orderitem = new OrderItem();
                        orderitem.DishId = cartitem.DishId;
                        orderitem.Price = cartitem.Price;
                        orderitem.Quantity = cartitem.Quantity;
                        if (order.OrderItems == null)
                        {
                            order.OrderItems = new List<OrderItem>();
                        }
                        order.OrderItems.Add(orderitem);
                    }
                }
                _context.Add(order);
               await DeleteCartAfterOrder(cart.Id);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { @id = order.Id});
            }
            ViewData["StoreId"] = new SelectList(_context.Store, nameof(Store.Id), nameof(Store.Address), order.StoreId);
            ViewData["UserName"] = new SelectList(_context.User, nameof(Models.User.UserName), nameof(Models.User.UserName), order.UserName);
            return View(order);
        }

        //// GET: Orders/Edit/5
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var order = await _context.Order.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["StoreId"] = new SelectList(_context.Store, nameof(Store.Id), nameof(Store.Address), order.StoreId);
        //    ViewData["UserName"] = new SelectList(_context.User, nameof(Models.User.UserName), nameof(Models.User.UserName), order.UserName);
        //    return View(order);
        //}

        //// POST: Orders/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,CreditCard,StoreId,Address,PostalCode,PhoneNumber,TotalPrice,UserName,OrderTime")] Order order)
        //{
        //    if (id != order.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(order);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!OrderExists(order.Id))
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
        //    ViewData["StoreId"] = new SelectList(_context.Store, nameof(Store.Id), nameof(Store.Address), order.StoreId);
        //    ViewData["UserName"] = new SelectList(_context.User, nameof(Models.User.UserName), nameof(Models.User.UserName), order.UserName);
        //    return View(order);
        //}

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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search(string queryUserName, string phoneNumber, string postalCode)
        {

            var q = from a in _context.Order.Include(o => o.Store).Include(o => o.User)
                    where ((a.PhoneNumber.Contains(phoneNumber)|| phoneNumber ==null) && (a.UserName.Contains(queryUserName) || queryUserName == null) &&
                            (a.PostalCode.Contains(postalCode) || postalCode == null))
                    orderby a.Id ascending
                    select a;

            return PartialView(nameof(Search), await q.ToListAsync());
        }
    }
}
