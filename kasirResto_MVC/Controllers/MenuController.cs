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
using Microsoft.AspNetCore.Authorization;

namespace kasirResto.Controllers
{
    [AllowAnonymous]
    public class MenuController : Controller
    {
        private readonly kasirRestoContext _context;

        public MenuController(kasirRestoContext context)
        {
            _context = context;
        }

        // GET: Menu
        public async Task<IActionResult> Index()
        {

            //ViewBag.sessi = HttpContext.Session.GetString("AnyKey");
            List<Menu> m = await _context.Menus.ToListAsync();
            List<KategoriMenu> km = await _context.KategoriMenus.ToListAsync();
            ViewData["Menu"] = m;
            ViewData["KategoriMenu"] = km;
            //var tupleModel = new Tuple<List<Menu>, List<KategoriMenu>>(m, km);
            return View();
        }

        // GET: Menu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Menu/Create
        public IActionResult Create()
        {
            List<KategoriMenu> kategori = _context.KategoriMenus.ToList();
            ViewBag.kategori = kategori;
            return View();
        }

        // POST: Menu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuId,MenuNama,MenuImage,MenuKategoriId,MenuPrice,MenuStatus")] Menu menu, IFormFile MenuImage)
        {
            if (MenuImage != null)
            {
                try
                {

                    var srcString = $"{DateTime.Now}";
                    var hashed = EncryptProvider.Md5(srcString);
                    var x = MenuImage.FileName;
                    var newname = $"p{hashed}.jpg";
                    string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\produk", newname);
                    var stream = new FileStream(uploadpath, FileMode.Create);
                    MenuImage.CopyToAsync(stream);
                    ViewBag.Message = MenuImage;
                    menu.MenuImage = $"img/produk/{newname}";
                }
                catch
                {
                    ViewBag.Message = "Error while uploading the files.";
                    return View("~/Views/Shared/Error_Message.cshtml");
                }
            }
            else
            {
                ViewBag.Message = "Gambar belum di isi.";
                return View("~/Views/Shared/Error_Message.cshtml");
            }
            _context.Add(menu);
            await _context.SaveChangesAsync();
            return RedirectToAction("index");
        }

        // GET: Menu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            List<KategoriMenu> kategori = _context.KategoriMenus.ToList();
            ViewBag.kategori = kategori;
            Menu m = _context.Menus.AsNoTracking().Where(x => x.MenuId == id).FirstOrDefault();
            ViewBag.datasaatini = m;
            return View(menu);
        }

        // POST: Menu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuId,MenuNama,MenuImage,MenuKategoriId,MenuPrice,MenuStatus")] Menu menu, IFormFile? MenuImage)
        {
            if (id != menu.MenuId)
            {
                return NotFound();
            }

            Menu m = _context.Menus.AsNoTracking().Where(x => x.MenuId == menu.MenuId).FirstOrDefault();
            try
            {
                if (MenuImage != null)
                {
                    if (MenuImage.FileName != null)
                    {
                        var srcString = $"{DateTime.Now}";
                        var hashed = EncryptProvider.Md5(srcString);
                        var newname = $"p{hashed}.jpg";
                        string uploadpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\produk", newname);
                        var stream = new FileStream(uploadpath, FileMode.Create);
                        MenuImage.CopyToAsync(stream);
                        menu.MenuImage = $"img/produk/{newname}";
                    }
                    else
                    {
                        menu.MenuImage = $"{m.MenuImage}";
                    }
                }
                else
                {
                    menu.MenuImage = $"{m.MenuImage}";
                }
            }
            catch (InvalidCastException e)
            {
                ViewBag.Message = "Error while uploading the files. @(e)";
                return View("~/Views/Shared/Error_Message.cshtml");
            }
            _context.Update(menu);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = menu.MenuId });
        }

        // GET: Menu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Menus == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Menus == null)
            {
                return Problem("Entity set 'kasirRestoContext.Menus'  is null.");
            }
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
          return _context.Menus.Any(e => e.MenuId == id);
        }
    }
}
