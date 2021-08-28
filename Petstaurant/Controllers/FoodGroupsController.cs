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
    public class FoodGroupsController : Controller
    {
        private readonly PetstaurantContext _context;

        public FoodGroupsController(PetstaurantContext context)
        {
            _context = context;
        }

        // GET: FoodGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.FoodGroup.ToListAsync());
        }

        // GET: FoodGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodGroup = await _context.FoodGroup
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodGroup == null)
            {
                return NotFound();
            }

            return View(foodGroup);
        }

        // GET: FoodGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FoodGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] FoodGroup foodGroup)
        {
            if (ModelState.IsValid)
            {
                var q = _context.FoodGroup.FirstOrDefault(u => u.Name == foodGroup.Name);
                if (q == null)
                {
                    _context.Add(foodGroup);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewData["Error"] = "Unable to comply; cannot create the FoodGroup.";
                }
            }
            return View(foodGroup);
        }

        // GET: FoodGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodGroup = await _context.FoodGroup.FindAsync(id);
            if (foodGroup == null)
            {
                return NotFound();
            }
            return View(foodGroup);
        }

        // POST: FoodGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] FoodGroup foodGroup)
        {
            if (id != foodGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(foodGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodGroupExists(foodGroup.Id))
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
            return View(foodGroup);
        }

        // GET: FoodGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodGroup = await _context.FoodGroup
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodGroup == null)
            {
                return NotFound();
            }

            return View(foodGroup);
        }

        // POST: FoodGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodGroup = await _context.FoodGroup.FindAsync(id);
            _context.FoodGroup.Remove(foodGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodGroupExists(int id)
        {
            return _context.FoodGroup.Any(e => e.Id == id);
        }
    }
}
