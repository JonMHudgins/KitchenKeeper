#nullable disable
using iText.Html2pdf;
using KitchenKeeper.Data;
using KitchenKeeper.Models;
using KitchenKeeper.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace KitchenKeeper.Controllers
{
    public class PDFsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment Environment;

        public PDFsController(ApplicationDbContext context, IWebHostEnvironment _environment)
        {
            _context = context;
            Environment = _environment;
        }

        // GET: PDFs/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pDF = await _context.PDFs
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (pDF == null)
            {
                return NotFound();
            }
            ViewBag.wwwPath = this.Environment.WebRootPath;
            ViewBag.contentPath = this.Environment.ContentRootPath;
            return View(pDF);
        }

        // GET: PDFs/Create
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

        // POST: PDFs/Generate
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
                string htmlContent = @"<div style='text-align: center'><h1>Item Info</h1><br><table style='text-align: left; margin-left: auto; margin-right: auto'><thead><tr><th>Property</th><th>Value</th></tr></thead><tbody><tr><td>ItemID</td><td>"
                + item.ItemID + @"</td></tr><tr><td>Item Name</td><td>" + item.ItemName + @"</td></tr><tr><td>Quantity</td><td>" + item.Quantity
                + @"</td></tr><tr><td>Unit</td><td>" + item.UnitOfMeasure + @"</td></tr><tr><td>Purchase Date</td><td>" + item.PurchaseDate
                + @"</td></tr><tr><td>Expiration Date</td><td>" + item.ExpirationDate + @"</td></tr><tr><td>Location ID</td><td>" + item.LocationID + @"</td></tr></tbody></table>";

                HtmlConverter.ConvertToPdf(htmlContent, memoryStream);

                bytes = memoryStream.ToArray();
            }

            PDF pdf = new PDF();
            pdf.ItemID = item.ItemID;
            pdf.Content = bytes;

            _context.Add(pdf);
            await _context.SaveChangesAsync();
            TempData["success"] = "Item created successfully!";
            return RedirectToAction("Index", "Items");
        }

        // POST: PDFs/Upload
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
            string fileNameWithPath = System.IO.Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.File.CopyTo(stream);
            }

            FileStream objFileStream = new FileStream(fileNameWithPath, FileMode.Open, FileAccess.Read);
            int intLength = Convert.ToInt32(objFileStream.Length);
            PDF pdf = new PDF();
            pdf.ItemID = item.ItemID;
            pdf.Content = new byte[intLength];
            objFileStream.Read(pdf.Content, 0, intLength);

            // Insert the new pdf using Entity Framework
            //_context.Add(pdf);
            //await _context.SaveChangesAsync();

            // Insert the new pdf using Stored Procedure
            var itemIdParam = new SqlParameter("@ItemID", pdf.ItemID);
            var contentParam = new SqlParameter("@Content", pdf.Content);
            await _context.Database.ExecuteSqlRawAsync("exec UploadPDF @ItemID, @Content", itemIdParam, contentParam);

            TempData["success"] = "Item created successfully!";
            return RedirectToAction("Index", "Items");
        }

        // GET: PDFs/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pDF = await _context.PDFs
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (pDF == null)
            {
                return NotFound();
            }
            ViewBag.wwwPath = this.Environment.WebRootPath;
            ViewBag.contentPath = this.Environment.ContentRootPath;
            return View(pDF);
        }

        // POST: PDFs/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pDF = await _context.PDFs.FindAsync(id);
            _context.PDFs.Remove(pDF);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Items");
        }

        private bool PDFExists(int id)
        {
            return _context.PDFs.Any(e => e.ItemID == id);
        }
    }
}
