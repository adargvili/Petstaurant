using System;
using System.Collections.Generic;
using System.IO;
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

            return View(await petstaurantContext.ToListAsync());
        }

        //// GET: Dishes/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var dish = await _context.Dish
        //        .Include(d => d.FoodGroup)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (dish == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(dish);
        //}

        // GET: Dishes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Store"] = new MultiSelectList(_context.Store, nameof(Store.Id), nameof(Store.Address));
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
            if (ModelState.IsValid)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    dish.ImageFile.CopyTo(ms);
                    dish.Image = ms.ToArray();
                }
                var q = _context.Dish.FirstOrDefault(u => u.Name == dish.Name);
                if (q == null)
                {
                    dish.Store = new List<Store>();
                    dish.Store.AddRange(_context.Store.Where(x => Store.Contains(x.Id)));
                    _context.Add(dish);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Error"] = "Unable to comply; cannot create the Dish.";
                }
            }
            ViewData["FoodGroupId"] = new SelectList(_context.FoodGroup, nameof(FoodGroup.Id), nameof(FoodGroup.Name), dish.FoodGroupId);
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
            ViewData["Store"] = new SelectList(_context.Store, nameof(Store.Id), nameof(Store.Address));
            ViewData["FoodGroupId"] = new SelectList(_context.FoodGroup, nameof(FoodGroup.Id), nameof(FoodGroup.Name), dish.FoodGroupId);
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FoodGroupId,Description,Price,Created,ImageFile")] Dish dish, int[] Store)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        dish.ImageFile.CopyTo(ms);
                        dish.Image = ms.ToArray();
                    }
                    //TODO: Edit stores and not only adding 
                    dish.Store = new List<Store>();
                    dish.Store.AddRange(_context.Store.Where(x => Store.Contains(x.Id)));
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
            ViewData["FoodGroupId"] = new SelectList(_context.FoodGroup, nameof(FoodGroup.Id), nameof(FoodGroup.Name), dish.FoodGroupId);
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
