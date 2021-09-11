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

        // GET: Carts/Details/5
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Details()
        {
            var u = GetCurrentUserName();
            var c = _context.Cart.FirstOrDefault(p => p.UserName == u);
            if (c == null)
            {
                return NotFound();
            }
            var id = c.Id;

            var cart = await _context.Cart
                .Include(c => c.User).Include(ci => ci.CartItems).ThenInclude(d => d.Dish)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }
            var t = GetCurrentUserType();
            if (u != cart.UserName)
            {
                return NotFound();
            }

            return View(cart);
        }

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
        [Authorize(Roles = "Customer")]
        public async Task<bool> AddDishToCart(int id)
        {
            var user = GetCurrentUserName();
            var cart = await _context.Cart.FirstOrDefaultAsync(s => s.UserName == user);

            var q = _context.CartItem.FirstOrDefault(u => u.DishId == id && u.CartId == cart.Id);
            var d = _context.Dish.FirstOrDefault(u => u.Id == id);
            if (d == null)
            {
                return false;
            }
            if (q == null)
            {
                CartItem cartItem = new CartItem();
                cartItem.CartId = cart.Id;
                cartItem.Quantity = 1;
                cartItem.DishId = id;
                cartItem.Price = d.Price;
                cart.TotalPrice += d.Price;
                _context.Add(cartItem);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                q.Quantity += 1;
                q.Price += d.Price;
                cart.TotalPrice += d.Price;
                await _context.SaveChangesAsync();
                return true;
            }
        }
        [Authorize(Roles = "Customer")]

        public async Task AddToTotalPrice(double price)
        {
            var user = GetCurrentUserName();
            var cart = await _context.Cart.FirstOrDefaultAsync(s => s.UserName == user);
            cart.TotalPrice += price;
            if (cart.TotalPrice < 0)
            {
                cart.TotalPrice = 0;
            }
        }
        [Authorize(Roles = "Customer")]
        public async Task<double[]> RemoveCartItem(int id)
        {
            var cartitem = await _context.CartItem.Include(d => d.Dish).FirstOrDefaultAsync(s => s.Id == id);
            var u = GetCurrentUserName();
            var c = _context.Cart.FirstOrDefault(p => p.UserName == u);
            if (cartitem != null)
            {
                if (cartitem.CartId != c.Id)
                {
                    double[] arrF = { 0, c.TotalPrice };
                    return arrF;
                }
                await AddToTotalPrice(-cartitem.Price);
                _context.CartItem.Remove(cartitem);
                await _context.SaveChangesAsync();
                double[] arrT = { 1, c.TotalPrice };
                return arrT;
            }
            double[] arrF2 = { 0, c.TotalPrice };
            return arrF2;
        }
        [Authorize(Roles = "Customer")]

        public async Task<double[]> ClearCart()
        {
            var u = GetCurrentUserName();
            var c = _context.Cart.FirstOrDefault(p => p.UserName == u);
            var cartitems = _context.CartItem.ToList().Where(p => p.CartId == c.Id);

            if (cartitems.Count() == 0)
            {
                double[] arrF3 = { 0, c.TotalPrice };
                return arrF3;
            }

            foreach (CartItem cartitem in cartitems)
            {
                if (cartitem != null)
                {
                    await AddToTotalPrice(-cartitem.Price);
                }  
            }
            _context.CartItem.RemoveRange(cartitems);
            await _context.SaveChangesAsync();
            double[] arrT = { 1, c.TotalPrice };
            return arrT;
        }

        [Authorize(Roles = "Customer")]

        public async Task<int> ToCheckout()
        {
            var u = GetCurrentUserName();
            var c = _context.Cart.FirstOrDefault(p => p.UserName == u);

            if (c.TotalPrice==0)
            {
                return 0;
            }
            return 1;


        }


        [Authorize(Roles = "Customer")]
        public async Task<double[]> AddOne(int id)
        {
            var cartitem = await _context.CartItem.Include(d => d.Dish).FirstOrDefaultAsync(s => s.Id == id);
            var u = GetCurrentUserName();
            var c = _context.Cart.FirstOrDefault(p => p.UserName == u);
            if (cartitem != null)
            {
                if (cartitem.CartId != c.Id)
                {
                    double[] arrF = {-1,-1,-1 };
                    return arrF;
                }
                cartitem.Quantity += 1;
                cartitem.Price = cartitem.Dish.Price * cartitem.Quantity;
               
                await AddToTotalPrice(cartitem.Dish.Price);
                await _context.SaveChangesAsync();

                double[] arrT = { cartitem.Price, cartitem.Cart.TotalPrice, cartitem.Quantity };
                return arrT;
            }
            double[] arrF2 = {-1,-1,-1 };
            return arrF2;


        }

        [Authorize(Roles = "Customer")]
        public async Task<double[]> SubtractOne(int id)
        {
            var cartitem = await _context.CartItem.Include(p => p.Dish).FirstOrDefaultAsync(s => s.Id == id);
            var u = GetCurrentUserName();
            var c = _context.Cart.FirstOrDefault(p => p.UserName == u);
            if (cartitem != null)
            {
                if (cartitem.CartId != c.Id)
                {
                    double[] arrF = {-1,-1,-1 };
                    return arrF;
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
                double[] arrT = { cartitem.Price, cartitem.Cart.TotalPrice, cartitem.Quantity };
                return arrT;
            }
            double[] arrF2 = {-1,-1,-1 };
            return arrF2;
        }

    }
}
