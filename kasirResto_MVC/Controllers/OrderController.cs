using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using kasirResto.Data;
using kasirResto.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace kasirResto.Controllers
{
    public class OrderController : Controller
    {
        private readonly kasirRestoContext _context;

        public OrderController(kasirRestoContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
              return View(await _context.Pesanans.Where(x => x.PesananStatus==1).ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pesanans == null)
            {
                return NotFound();
            }

            var pesanan = await _context.Pesanans
                .FirstOrDefaultAsync(m => m.PesananId == id);
            if (pesanan == null)
            {
                return NotFound();
            }
            var kasiruser = _context.Users.AsNoTracking().Where(a => a.Id == pesanan.PesananUserId).FirstOrDefault();
            var query =
                    from mn in _context.Menus
                    join pd in _context.PesananDetails on mn.MenuId equals pd.PesananMenuId
                    //join aspuser in _context.AspNetUsers on mn equals aspuser.PesananMenuId

                    where pd.PesananDpid == pesanan.PesananId
                    select new
                    {
                        PesananDid = pd.PesananDid,
                        PesananDpid = pd.PesananDpid,
                        PesananMenuId = pd.PesananMenuId,
                        PesananPrice = pd.PesananPrice,
                        PesananStatus = pd.PesananStatus,
                        PesananQty = pd.PesananQty,
                        MenuId = mn.MenuId,
                        MenuNama = mn.MenuNama,
                        MenuImage = mn.MenuImage,
                        MenuKategoriId = mn.MenuKategoriId,
                        MenuPrice = mn.MenuPrice,
                        MenuStatus = mn.MenuStatus,
                    };
            var q = query.ToList();
            ViewData["kasiruser"] = kasiruser;
            ViewData["pesanan"] = pesanan;
            ViewBag.PesananDetail = q;
            return View(pesanan);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PesananId,PesananCode,PesananDate,PesananUserId,PesananStatus")] Pesanan pesanan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pesanan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pesanan);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pesanans == null)
            {
                return NotFound();
            }

            var pesanan = await _context.Pesanans.FindAsync(id);
            if (pesanan == null)
            {
                return NotFound();
            }
            return View(pesanan);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PesananId,PesananCode,PesananDate,PesananUserId,PesananStatus")] Pesanan pesanan)
        {
            if (id != pesanan.PesananId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pesanan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PesananExists(pesanan.PesananId))
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
            return View(pesanan);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pesanans == null)
            {
                return NotFound();
            }

            var pesanan = await _context.Pesanans
                .FirstOrDefaultAsync(m => m.PesananId == id);
            if (pesanan == null)
            {
                return NotFound();
            }

            return View(pesanan);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pesanans == null)
            {
                return Problem("Entity set 'kasirRestoContext.Pesanans'  is null.");
            }
            var pesanan = await _context.Pesanans.FindAsync(id);
            if (pesanan != null)
            {
                _context.Pesanans.Remove(pesanan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PesananExists(int id)
        {
          return _context.Pesanans.Any(e => e.PesananId == id);
        }
    }
}
