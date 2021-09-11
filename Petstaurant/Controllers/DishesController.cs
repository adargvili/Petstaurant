using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Petstaurant.Data;
using Petstaurant.Models;

namespace Petstaurant.Controllers
{
    public class DishesController : Controller
    {
        private readonly PetstaurantContext _context;

        public DishesController(PetstaurantContext context)
        {
            _context = context;
        }

        // GET: Dishes
        public async Task<IActionResult> Index()
        {
            var petstaurantContext = _context.Dish.Include(d => d.FoodGroup).Include(u=>u.Store);

            var t = GetCurrentUserType();
            if (t == "Admin")
            {
                ViewData["UserAdmin"] = UserType.Admin;
            }
            ViewData["StoreList"] = new ArrayList(_context.Store.ToList());
            ViewData["FoodGroupList"] = new ArrayList(_context.FoodGroup.ToList());
            return View(await petstaurantContext.ToListAsync());
        }

        // GET: Dishes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Store"] = new MultiSelectList(_context.Store, nameof(Store.Id), nameof(Store.City));
            ViewData["FoodGroupId"] = new SelectList(_context.FoodGroup, nameof(FoodGroup.Id), nameof(FoodGroup.Name));
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,FoodGroupId,Description,Price,ImageFile")] Dish dish, int[] Store)
        {
            ViewData["Store"] = new MultiSelectList(_context.Store, nameof(Models.Store.Id), nameof(Models.Store.City));
            ViewData["FoodGroupId"] = new SelectList(_context.FoodGroup, nameof(FoodGroup.Id), nameof(FoodGroup.Name), dish.FoodGroupId);
            if (ModelState.IsValid)
            {
                var q = _context.Dish.FirstOrDefault(u => u.Name == dish.Name);
                if (q == null)
                {
                    if (dish.Price == 0)
                    {
                        ViewData["Error"] = "Choose a postive price";
                        return View(dish);
                    }
                    if (!dish.Name.All(x => char.IsLetter(x) || x == ' ') || dish.Name.StartsWith(" ") || dish.Name.EndsWith(" ") || dish.Name.Count(Char.IsWhiteSpace) > 3 || (dish.Name.Count(Char.IsWhiteSpace) > dish.Name.Split().Length - 1))
                    {
                        ViewData["Error"] = "Please enter a valid dish name";
                        return View(dish);
                    }

                    if (dish.Description.StartsWith(" ") || dish.Description.EndsWith(" ") || (dish.Description.Count(Char.IsWhiteSpace) > dish.Description.Split().Length - 1))
                    {
                        ViewData["Error"] = "Please enter a valid description name";
                        return View(dish);
                    }
                    if (dish.ImageFile == null)
                    {
                        String path = "./wwwroot/pics/defaultpic.jpeg";
                        using (var stream = System.IO.File.OpenRead(path))
                        {
                            dish.ImageFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
                            using (MemoryStream ms = new MemoryStream())
                            {
                                dish.ImageFile.CopyTo(ms);
                                dish.Image = ms.ToArray();
                            }
                        }
                    }
                    else
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            dish.ImageFile.CopyTo(ms);
                            dish.Image = ms.ToArray();
                        }
                    }
                    var fg = _context.FoodGroup.FirstOrDefault(f => f.Id == dish.FoodGroupId);
                    if (fg == null)
                    {
                        ViewData["Error"] = "You have to choose Food Group.";
                        return View(dish);
                    }
                    dish.Store = new List<Store>();
                    dish.Store.AddRange(_context.Store.Where(x => Store.Contains(x.Id)));
                    if (dish.Store.Count==0)
                    {
                        ViewData["Error"] = "You have to choose at least one store.";
                        return View(dish);
                    }
                    _context.Add(dish);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Error"] = "Unable to comply; cannot create the Dish.";
                }
            }
            return View(dish);
        }

        // GET: Dishes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            ViewData["Store"] = new SelectList(_context.Store, nameof(Store.Id), nameof(Store.City));
            ViewData["FoodGroupId"] = new SelectList(_context.FoodGroup, nameof(FoodGroup.Id), nameof(FoodGroup.Name), dish.FoodGroupId);
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FoodGroupId,Description,Price,ImageFile")] Dish dish, int[] Store)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }
            ViewData["Store"] = new SelectList(_context.Store, nameof(Models.Store.Id), nameof(Models.Store.City));
            ViewData["FoodGroupId"] = new SelectList(_context.FoodGroup, nameof(FoodGroup.Id), nameof(FoodGroup.Name), dish.FoodGroupId);

            Boolean flagForImage = true;
            if (dish.ImageFile == null)
            {
                ModelState.Remove("Image");
                ModelState.Remove("ImageFile");
                dish.Image = _context.Dish.AsNoTracking().Where(d => d.Id == dish.Id).First().Image;
                flagForImage = false;
            }

            if (ModelState.IsValid)
            {

                if (dish.Price == 0)
                {
                    ViewData["Error"] = "Choose a postive price";
                    return View(dish);
                }
                if (!dish.Name.All(x => char.IsLetter(x) || x == ' ') || dish.Name.StartsWith(" ") || dish.Name.EndsWith(" ") || dish.Name.Count(Char.IsWhiteSpace) > 3 || (dish.Name.Count(Char.IsWhiteSpace) > dish.Name.Split().Length - 1))
                {
                    ViewData["Error"] = "Please enter a valid dish name";
                    return View(dish);
                }

                if (dish.Description.StartsWith(" ") || dish.Description.EndsWith(" ") || (dish.Description.Count(Char.IsWhiteSpace) > dish.Description.Split().Length - 1))
                {
                    ViewData["Error"] = "Please enter a valid description name";
                    return View(dish);
                }
                var fg = _context.FoodGroup.FirstOrDefault(f => f.Id == dish.FoodGroupId);
                if (fg == null)
                {
                    ViewData["Error"] = "You have to choose Food Group.";
                    return View(dish);
                }
                try
                {

                    if (flagForImage)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            dish.ImageFile.CopyTo(ms);
                            dish.Image = ms.ToArray();
                        }
                    }

                    var x = _context.Dish.Where(d => d.Id == dish.Id).Include(s => s.Store).First();
                    x.Store = new List<Store>();
                    await _context.SaveChangesAsync();
                    _context.Entry(x).State = EntityState.Detached;

                    dish.Store = new List<Store>();
                    dish.Store.AddRange(_context.Store.Where(x => Store.Contains(x.Id)));
                    if (dish.Store.Count == 0)
                    {
                        ViewData["Error"] = "You have to choose at least one store.";
                        return View(dish);
                    }


                    var cartItems = _context.CartItem.Where(c => c.DishId == id).ToList();

                    foreach (CartItem ci in cartItems)
                    {
                        var c = _context.Cart.FirstOrDefault(p => p.Id == ci.CartId);
                        if (ci.Price!=ci.Quantity*dish.Price)
                        {
                            c.TotalPrice -= ci.Price;
                            ci.Price = ci.Quantity * dish.Price;
                            c.TotalPrice += ci.Quantity * dish.Price;
                            if (c.TotalPrice < 0)
                            {
                                c.TotalPrice = 0;
                            }
                            if (ci.Price < 0)
                            {
                                ci.Price = 0;
                            }
                        }
                    }

                    _context.Update(dish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishExists(dish.Id))
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
            return View(dish);
        }

        // GET: Dishes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .Include(d => d.FoodGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dish = await _context.Dish.FindAsync(id);
            var cartItems = _context.CartItem.Where(c => c.DishId == id).ToList();

            foreach (CartItem ci in cartItems)
            {
                var c = _context.Cart.FirstOrDefault(p => p.Id == ci.CartId);
                if (ci != null)
                {
                    c.TotalPrice -= dish.Price*ci.Quantity;
                    if (c.TotalPrice < 0)
                    {
                        c.TotalPrice = 0;
                    }
                    _context.CartItem.Remove(ci);
                }
            }
            _context.Dish.Remove(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string dishName, string[] cities, string selectFoodType)
        {
            var q = from d in _context.Dish.Include(a => a.Store).Include(a=>a.FoodGroup)
                    where ((d.Name.Contains(dishName) || dishName == null)
                    && (d.FoodGroup.Name.Equals(selectFoodType) || selectFoodType == null) 
                    && (d.Store.Any(s=>cities[0].Contains(s.City))))
                    orderby d.Name ascending
                    select d;
            if (GetCurrentUserType() == "Admin")
            {
                return PartialView("SearchAdmin", await q.ToListAsync());
            }
            else {
                return PartialView("SearchCustomer", await q.ToListAsync());
            }

        }

        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.Id == id);
        }

        private string GetCurrentUserName()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var u = claims.First().Value;
            return u;
        }


        private string GetCurrentUserType()
        {
            if (!User.Identity.IsAuthenticated) {
                return null;            
            }
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var u = claims.Skip(1).First().Value;
            return u;
        }
    }
}
