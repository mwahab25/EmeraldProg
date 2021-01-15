using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmeraldProg.Models;

namespace EmeraldProg.Controllers
{
    public class ItemsController : Controller
    {
        private readonly EmeraldContext _context;

        public ItemsController(EmeraldContext context)
        {
            _context = context;
        }

        // GET: Items
        //public async Task<IActionResult> Index()
        //{
        //    var emeraldContext = _context.Items.Include(i => i.Category).Include(i => i.ItemType).Include(i => i.Location);
        //    return View(await emeraldContext.ToListAsync());
        //}

        public async Task<IActionResult> Index(string sortOrder,string currentFilter,string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var items = from s in _context.Items
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                items = items.Where(s => s.ItemName.Contains(searchString)
                                       || s.Vendor.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    items = items.OrderByDescending(s => s.ItemName);
                    break;
                case "Date":
                    items = items.OrderBy(s => s.RunDate);
                    break;
                case "date_desc":
                    items = items.OrderByDescending(s => s.RunDate);
                    break;
                default:
                    items = items.OrderBy(s => s.SerialNo);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Item>.CreateAsync(items.Include(i => i.Category).Include(i => i.ItemType).Include(i => i.Location).AsNoTracking(), pageNumber ?? 1, pageSize));
        }


        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.ItemType)
                .Include(i => i.Location)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName");
            ViewData["ItemTypeID"] = new SelectList(_context.ItemTypes, "ItemTypeID", "ItemTypeName");
            ViewData["LocationID"] = new SelectList(_context.Locations, "LocationID", "LocationName");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,SerialNo,CategoryID,LocationID,ItemTypeID,ItemName,Vendor,Description,ItemPrice,InstalPrice,Qty,PurchasedDate,InstalDate,RunDate")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", item.CategoryID);
            ViewData["ItemTypeID"] = new SelectList(_context.ItemTypes, "ItemTypeID", "ItemTypeName", item.ItemTypeID);
            ViewData["LocationID"] = new SelectList(_context.Locations, "LocationID", "LocationName", item.LocationID);
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", item.CategoryID);
            ViewData["ItemTypeID"] = new SelectList(_context.ItemTypes, "ItemTypeID", "ItemTypeName", item.ItemTypeID);
            ViewData["LocationID"] = new SelectList(_context.Locations, "LocationID", "LocationName", item.LocationID);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemID,SerialNo,CategoryID,LocationID,ItemTypeID,ItemName,Vendor,Description,ItemPrice,InstalPrice,Qty,PurchasedDate,InstalDate,RunDate")] Item item)
        {
            if (id != item.ItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemID))
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryName", item.CategoryID);
            ViewData["ItemTypeID"] = new SelectList(_context.ItemTypes, "ItemTypeID", "ItemTypeName", item.ItemTypeID);
            ViewData["LocationID"] = new SelectList(_context.Locations, "LocationID", "LocationName", item.LocationID);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.ItemType)
                .Include(i => i.Location)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemID == id);
        }
    }
}
