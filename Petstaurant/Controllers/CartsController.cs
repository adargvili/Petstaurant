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
    public class CartsController : Controller
    {
        private readonly PetstaurantContext _context;

        public CartsController(PetstaurantContext context)
        {
            _context = context;
        }

        // GET: Carts
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var petstaurantContext = _context.Cart.Include(c => c.User);
            return View(await petstaurantContext.ToListAsync());
        }

        // GET: Carts/Details/5
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Details()
        {
            var u = GetCurrentUserName();
            var c = _context.Cart.FirstOrDefault(p => p.UserName == u);
            var id = c.Id;
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.User).Include(ci => ci.CartItems).ThenInclude(d => d.Dish)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }
            var t = GetCurrentUserType();
            if ((u != cart.UserName) && t != "Admin")
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        //[Authorize(Roles = "Admin")]
        //public IActionResult Create()
        //{
        //    ViewData["UserName"] = new SelectList(_context.User, nameof(Models.User.UserName), nameof(Models.User.UserName));
        //    return View();
        //}

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Create([Bind("Id,TotalPrice,UserName")] Cart cart)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(cart);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["UserName"] = new SelectList(_context.User, nameof(Models.User.UserName), nameof(Models.User.UserName), cart.UserName);
        //    return View(cart);
        //}

        //// GET: Carts/Edit/5
        //[Authorize(Roles = "Admin, Customer")]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cart = await _context.Cart.FindAsync(id);
        //    if (cart == null)
        //    {
        //        return NotFound();
        //    }

        //    var u = GetCurrentUserName();
        //    var t = GetCurrentUserType();
        //    if ((u != cart.UserName) && t != "Admin")
        //    {
        //        return NotFound();
        //    }

        //    ViewData["UserName"] = new SelectList(_context.User, nameof(Models.User.UserName), nameof(Models.User.UserName), cart.UserName);
        //    return View(cart);
        //}

        //// POST: Carts/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin, Customer")]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,TotalPrice,UserName")] Cart cart)
        //{
        //    if (id != cart.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(cart);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CartExists(cart.Id))
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
        //    ViewData["UserName"] = new SelectList(_context.User, nameof(Models.User.UserName), nameof(Models.User.UserName), cart.UserName);
        //    return View(cart);
        //}

        //// GET: Carts/Delete/5
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var cart = await _context.Cart
        //        .Include(c => c.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (cart == null)
        //    {
        //        return NotFound();
        //    }


        //    return View(cart);
        //}

        //// POST: Carts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var cart = await _context.Cart.FindAsync(id);
        //    _context.Cart.Remove(cart);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.Id == id);
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


        public async Task AddToTotalPrice(double price)
        {
            var user = GetCurrentUserName();
            var cart = await _context.Cart.FirstOrDefaultAsync(s => s.UserName == user);
            cart.TotalPrice += price;
            if (cart.TotalPrice < 0)
            {
                cart.TotalPrice = 0;
            }
            await _context.SaveChangesAsync();
        }

        [Authorize(Roles = "Admin, Customer")]
        public async Task<double[]> AddOne(int id)
        {
            var cartitem = await _context.CartItem.Include(d => d.Dish).FirstOrDefaultAsync(s => s.Id == id);
            var u = GetCurrentUserName();
            var c = _context.Cart.FirstOrDefault(p => p.UserName == u);
            if (cartitem != null)
            {
                if (cartitem.CartId != c.Id)
                {
                    double[] arrT = {-1,-1,-1 };
                    return arrT;
                }
                cartitem.Quantity += 1;
                cartitem.Price = cartitem.Dish.Price * cartitem.Quantity;
                await AddToTotalPrice(cartitem.Dish.Price);
                await _context.SaveChangesAsync();
                double[] arr = { cartitem.Price, cartitem.Cart.TotalPrice, cartitem.Quantity };
                return arr;
            }
            double[] arrF = {-1,-1,-1 };
            return arrF;


        }

        [Authorize(Roles = "Admin, Customer")]
        public async Task<double[]> SubtractOne(int id)
        {
            var cartitem = await _context.CartItem.Include(p => p.Dish).FirstOrDefaultAsync(s => s.Id == id);
            var u = GetCurrentUserName();
            var c = _context.Cart.FirstOrDefault(p => p.UserName == u);
            if (cartitem != null)
            {
                if (cartitem.CartId != c.Id)
                {
                    double[] arrT = {-1,-1,-1 };
                    return arrT;
                }
                cartitem.Quantity -= 1;
                if (cartitem.Quantity == 0)
                {
                    await AddToTotalPrice(-cartitem.Dish.Price);
                    _context.CartItem.Remove(cartitem);
                    await _context.SaveChangesAsync();
                    double[] arrT2 = { 0, c.TotalPrice, 0};
                    return arrT2;
                }
                cartitem.Price = cartitem.Dish.Price * cartitem.Quantity;
                await AddToTotalPrice(-cartitem.Dish.Price);
                await _context.SaveChangesAsync();
                double[] arr = { cartitem.Price, cartitem.Cart.TotalPrice, cartitem.Quantity };
                return arr;
            }
            double[] arrT3 = {-1,-1,-1 };
            return arrT3;
        }

    }
}
