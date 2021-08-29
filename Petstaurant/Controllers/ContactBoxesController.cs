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
    public class ContactBoxesController : Controller
    {
        private readonly PetstaurantContext _context;

        public ContactBoxesController(PetstaurantContext context)
        {
            _context = context;
        }

        // GET: ContactBoxes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ContactBox.ToListAsync());
        }

        // GET: ContactBoxes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactBox = await _context.ContactBox
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactBox == null)
            {
                return NotFound();
            }

            return View(contactBox);
        }

        // GET: ContactBoxes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContactBoxes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Subject,Body")] ContactBox contactBox)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactBox);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactBox);
        }

        // GET: ContactBoxes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactBox = await _context.ContactBox.FindAsync(id);
            if (contactBox == null)
            {
                return NotFound();
            }
            return View(contactBox);
        }

        // POST: ContactBoxes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Subject,Body")] ContactBox contactBox)
        {
            if (id != contactBox.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contactBox);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactBoxExists(contactBox.Id))
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
            return View(contactBox);
        }

        // GET: ContactBoxes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactBox = await _context.ContactBox
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactBox == null)
            {
                return NotFound();
            }

            return View(contactBox);
        }

        // POST: ContactBoxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactBox = await _context.ContactBox.FindAsync(id);
            _context.ContactBox.Remove(contactBox);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactBoxExists(int id)
        {
            return _context.ContactBox.Any(e => e.Id == id);
        }
    }
}
