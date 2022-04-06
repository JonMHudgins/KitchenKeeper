#nullable disable
using KitchenKeeper.Data;
using KitchenKeeper.Models;
using KitchenKeeper.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace KitchenKeeper.Controllers
{
    public class JSONsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment Environment;

        public JSONsController(ApplicationDbContext context, IWebHostEnvironment _environment)
        {
            _context = context;
            Environment = _environment;
        }

        // GET: JSONs/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jSON = await _context.JSONs
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (jSON == null)
            {
                return NotFound();
            }
            ViewBag.wwwPath = this.Environment.WebRootPath;
            ViewBag.contentPath = this.Environment.ContentRootPath;
            return View(jSON);
        }

        // GET: JSONs/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }
            ViewBag.LocationNames = _context.StorageLocations;
            ViewBag.Item = item;
            return View();
        }

        // POST: JSONs/Generate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (item == null)
            {
                return NotFound();
            }

            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                JsonSerializer.Serialize(memoryStream, item);
                bytes = memoryStream.ToArray();
            }

            JSON json = new JSON();
            json.ItemID = item.ItemID;
            json.Content = bytes;

            _context.Add(json);
            await _context.SaveChangesAsync();
            TempData["success"] = "Item created successfully!";
            return RedirectToAction("Index", "Items");
        }

        // POST: JSONs/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(SingleFileModel model)
        {
            var item = await _context.Items
    .FirstOrDefaultAsync(m => m.ItemID == model.ItemID);
            if (item == null)
            {
                return NotFound();
            }

            if (model.File == null)
            {
                return NotFound();
            }

            string path = this.Environment.WebRootPath + "/Files";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            FileInfo fileInfo = new FileInfo(model.File.FileName);
            string fileName = "temp" + fileInfo.Extension;
            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.File.CopyTo(stream);
            }

            FileStream objFileStream = new FileStream(fileNameWithPath, FileMode.Open, FileAccess.Read);
            int intLength = Convert.ToInt32(objFileStream.Length);
            JSON json = new JSON();
            json.ItemID = item.ItemID;
            json.Content = new byte[intLength];
            objFileStream.Read(json.Content, 0, intLength);

            // Insert the new json using Entity Framework
            //_context.Add(json);
            //await _context.SaveChangesAsync();

            // Insert the new json using Stored Procedure
            var itemIdParam = new SqlParameter("@ItemID", json.ItemID);
            var contentParam = new SqlParameter("@Content", json.Content);
            await _context.Database.ExecuteSqlRawAsync("exec UploadJSON @ItemID, @Content", itemIdParam, contentParam);

            TempData["success"] = "Item created successfully!";
            return RedirectToAction("Index", "Items");
        }

        // GET: JSONs/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jSON = await _context.JSONs
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (jSON == null)
            {
                return NotFound();
            }
            ViewBag.wwwPath = this.Environment.WebRootPath;
            ViewBag.contentPath = this.Environment.ContentRootPath;
            return View(jSON);
        }

        // POST: JSONs/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jSON = await _context.JSONs.FindAsync(id);
            _context.JSONs.Remove(jSON);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Items");
        }

        private bool JSONExists(int id)
        {
            return _context.JSONs.Any(e => e.ItemID == id);
        }
    }
}
