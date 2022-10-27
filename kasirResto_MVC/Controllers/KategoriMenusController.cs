using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using kasirResto.Data;
using kasirResto.Models;
using NETCore.Encrypt;
namespace kasirResto.Controllers
{
    public class KategoriMenusController : Controller
    {
        private readonly kasirRestoContext _context;

        public KategoriMenusController(kasirRestoContext context)
        {
            _context = context;
        }

        // GET: KategoriMenus
        public async Task<IActionResult> Index()
        {
              return View(await _context.KategoriMenus.ToListAsync());
        }

        // GET: KategoriMenus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KategoriMenus == null)
            {
                return NotFound();
            }

            var kategoriMenu = await _context.KategoriMenus
                .FirstOrDefaultAsync(m => m.KategoriId == id);
            if (kategoriMenu == null)
            {
                return NotFound();
            }

            return View(kategoriMenu);
        }

        // GET: KategoriMenus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KategoriMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KategoriId,KategoriNama,KategoriImage,KategoriStatus")] KategoriMenu kategoriMenu, IFormFile KategoriImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (KategoriImage != null)
                    {
                        var srcString = $"{DateTime.Now}";
                        var hashed = EncryptProvider.Md5(srcString);
                        var x = KategoriImage.FileName;
                        var newname = $"p{hashed}.jpg";
                        string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\kategori", newname);
                        var stream = new FileStream(uploadpath, FileMode.Create);
                        KategoriImage.CopyToAsync(stream);
                        ViewBag.Message = KategoriImage;
                        kategoriMenu.KategoriImage = $"img/kategori/{newname}";
                    }
                    else
                    {
                        ViewBag.Message = "Gambar belum di isi.";
                        return View("~/Views/Shared/Error_Message.cshtml");
                    }

                }
                catch
                {
                    ViewBag.Message = "Error while uploading the files.";
                    return View("~/Views/Shared/Error_Message.cshtml");
                }
                _context.Add(kategoriMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Menu", new { area = "" });
            }
            return View(kategoriMenu);
        }

        // GET: KategoriMenus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KategoriMenus == null)
            {
                return NotFound();
            }

            var kategoriMenu = await _context.KategoriMenus.FindAsync(id);
            if (kategoriMenu == null)
            {
                return NotFound();
            }
            KategoriMenu m = _context.KategoriMenus.AsNoTracking().Where(x => x.KategoriId == id).FirstOrDefault();
            ViewBag.datasaatini = m;
            return View(kategoriMenu);
        }

        // POST: KategoriMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KategoriId,KategoriNama,KategoriImage,KategoriStatus")] KategoriMenu kategoriMenu, IFormFile? KategoriImage)
        {
            if (id != kategoriMenu.KategoriId)
            {
                return NotFound();
            }

            KategoriMenu m = _context.KategoriMenus.AsNoTracking().Where(x => x.KategoriId == id).FirstOrDefault();
            if (KategoriImage != null)
            {
                try
                {
                    var srcString = $"{DateTime.Now}";
                    var hashed = EncryptProvider.Md5(srcString);
                    var x = KategoriImage.FileName;
                    var newname = $"p{hashed}.jpg";
                    string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\kategori", newname);
                    var stream = new FileStream(uploadpath, FileMode.Create);
                    KategoriImage?.CopyToAsync(stream);
                    kategoriMenu.KategoriImage = $"img/kategori/{newname}";
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Error while uploading the files." + e;
                    return View("~/Views/Shared/Error_Message.cshtml");
                }
            }
            else
            {
                kategoriMenu.KategoriImage = m.KategoriImage;
            }
            _context.Update(kategoriMenu);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = kategoriMenu.KategoriId });
        }

        // GET: KategoriMenus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KategoriMenus == null)
            {
                return NotFound();
            }

            var kategoriMenu = await _context.KategoriMenus
                .FirstOrDefaultAsync(m => m.KategoriId == id);
            if (kategoriMenu == null)
            {
                return NotFound();
            }

            return View(kategoriMenu);
        }

        // POST: KategoriMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KategoriMenus == null)
            {
                return Problem("Entity set 'kasirRestoContext.KategoriMenus'  is null.");
            }
            var kategoriMenu = await _context.KategoriMenus.FindAsync(id);
            if (kategoriMenu != null)
            {
                _context.KategoriMenus.Remove(kategoriMenu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KategoriMenuExists(int id)
        {
          return _context.KategoriMenus.Any(e => e.KategoriId == id);
        }
    }
}
