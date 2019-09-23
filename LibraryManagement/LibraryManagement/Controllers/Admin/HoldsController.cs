using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryData;
using LibraryData.Models;

namespace LibraryManagement.Controllers.Admin
{
    public class HoldsController : Controller
    {
        private readonly LibraryContext _context;

        public HoldsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: Holds
        public async Task<IActionResult> Index()
        {
            return View(await _context.Holds.ToListAsync());
        }

        // GET: Holds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hold = await _context.Holds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hold == null)
            {
                return NotFound();
            }

            return View(hold);
        }

        // GET: Holds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Holds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,HoldPlaced")] Hold hold)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hold);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hold);
        }

        // GET: Holds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hold = await _context.Holds.FindAsync(id);
            if (hold == null)
            {
                return NotFound();
            }
            return View(hold);
        }

        // POST: Holds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,HoldPlaced")] Hold hold)
        {
            if (id != hold.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hold);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoldExists(hold.Id))
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
            return View(hold);
        }

        // GET: Holds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hold = await _context.Holds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hold == null)
            {
                return NotFound();
            }

            return View(hold);
        }

        // POST: Holds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hold = await _context.Holds.FindAsync(id);
            _context.Holds.Remove(hold);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoldExists(int id)
        {
            return _context.Holds.Any(e => e.Id == id);
        }
    }
}
