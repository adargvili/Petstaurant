﻿using System;
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
    
    public class StoresController : Controller
    {
        private readonly PetstaurantContext _context;

        public StoresController(PetstaurantContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        // GET: Stores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Store.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        // GET: Stores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .FirstOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        [Authorize(Roles = "Admin")]
        // GET: Stores/Create
        public IActionResult Create()
        {
            ViewData["Dish"] = new SelectList(_context.Dish, nameof(Dish.Id), nameof(Dish.Name));
            return View();
        }

        public IActionResult TLVcity()
        {
            var store = _context.Store.First(s => s.City.Equals("Tel-Aviv"));
            var split = store.Address.Split(" ");
            var builder ="";
            for (int i = 0; i < split.Length; i++)
            {
                builder+=split[i];
                builder+="+";
            }
            builder = builder.Substring(0, builder.Length - 1);
            builder+=",";

            var city = store.City.Split("-");
            builder += city[0];
            builder += "+";
            builder += city[1];

            ViewData["StoreAddress"] = builder;
            return View();
        }

        public IActionResult JerusalemCity()
        {
            var store = _context.Store.First(s => s.City.Equals("Jerusalem"));
            var split = store.Address.Split(" ");
            var builder = "";
            for (int i = 0; i < split.Length; i++)
            {
                builder += split[i];
                builder += "+";
            }
            builder = builder.Substring(0, builder.Length - 1);
            builder += ",";
            builder += store.City;

            ViewData["StoreAddress"] = builder;
            return View();
        }

        public IActionResult Kmalachi()
        {
            var store = _context.Store.First(s => s.City.Equals("Kiryat-Malachi"));
            var split = store.Address.Split(" ");
            var builder = "";
            for (int i = 0; i < split.Length; i++)
            {
                builder += split[i];
                builder += "+";
            }
            builder = builder.Substring(0, builder.Length - 1);
            builder += ",";

            var city = store.City.Split("-");
            builder += city[0];
            builder += "+";
            builder += city[1];


            ViewData["StoreAddress"] = builder;
            return View();
        }
        public IActionResult BshevaCity()
        {
            var store = _context.Store.First(s => s.City.Equals("Beer-Sheva"));
            var split = store.Address.Split(" ");
            var builder = "";
            for (int i = 0; i < split.Length; i++)
            {
                builder += split[i];
                builder += "+";
            }
            builder = builder.Substring(0, builder.Length - 1);
            builder += ",";

            var city = store.City.Split("-");
            builder += city[0];
            builder += "+";
            builder += city[1];

            ViewData["StoreAddress"] = builder;
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,City,Address,PostalCode")] Store store, int[] Dish)
        {
            if (ModelState.IsValid)
            {
                var q = _context.Store.FirstOrDefault(u => (store.City == u.City) && (store.Address == u.Address));
                if (q == null)
                {
                    store.Dish = new List<Dish>();
                    store.Dish.AddRange(_context.Dish.Where(x => Dish.Contains(x.Id)));
                    _context.Add(store);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    ViewData["Error"] = "Unable to comply; cannot create the Store.";
                }
            }
            return View(store);
        }

        // GET: Stores/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }
            return View(store);
        }



        // POST: Stores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("Id,City,Address,PostalCode")] Store store)
        {
            if (id != store.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(store);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.Id))
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
            return View(store);
        }
        [Authorize(Roles = "Admin")]
        // GET: Stores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .FirstOrDefaultAsync(m => m.Id == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Stores/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var store = await _context.Store.FindAsync(id);
            _context.Store.Remove(store);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(int id)
        {
            return _context.Store.Any(e => e.Id == id);
        }
    }
}
