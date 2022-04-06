#nullable disable
using KitchenKeeper.Data;
using KitchenKeeper.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KitchenKeeper.Controllers
{
    public class StorageLocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StorageLocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StorageLocations
        public async Task<IActionResult> Index()
        {
            return View(await _context.StorageLocations.ToListAsync());
        }

        // GET: StorageLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageLocation = await _context.StorageLocations
                .FirstOrDefaultAsync(m => m.LocationID == id);
            if (storageLocation == null)
            {
                return NotFound();
            }

            return View(storageLocation);
        }

        // GET: StorageLocations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StorageLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationID,LocationName")] StorageLocation storageLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(storageLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(storageLocation);
        }

        // GET: StorageLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageLocation = await _context.StorageLocations.FindAsync(id);
            if (storageLocation == null)
            {
                return NotFound();
            }
            return View(storageLocation);
        }

        // POST: StorageLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocationID,LocationName")] StorageLocation storageLocation)
        {
            if (id != storageLocation.LocationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(storageLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StorageLocationExists(storageLocation.LocationID))
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
            return View(storageLocation);
        }

        // GET: StorageLocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var storageLocation = await _context.StorageLocations
                .FirstOrDefaultAsync(m => m.LocationID == id);
            if (storageLocation == null)
            {
                return NotFound();
            }

            return View(storageLocation);
        }

        // POST: StorageLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var storageLocation = await _context.StorageLocations.FindAsync(id);
            _context.StorageLocations.Remove(storageLocation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StorageLocationExists(int id)
        {
            return _context.StorageLocations.Any(e => e.LocationID == id);
        }
    }
}
