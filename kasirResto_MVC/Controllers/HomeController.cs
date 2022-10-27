using kasirResto.Areas.Identity.Data;
using kasirResto.Data;
using kasirResto.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using NETCore.Encrypt;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static NuGet.Packaging.PackagingConstants;

namespace kasirResto.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Data.kasirRestoContext _context;
        private readonly UserManager<kasirRestoUser> _userManager;
        private readonly SignInManager<kasirRestoUser> _SignInManager;
        
        public HomeController(ILogger<HomeController> logger, kasirRestoContext context, UserManager<kasirRestoUser> userManager, SignInManager<kasirRestoUser> SignInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _SignInManager = SignInManager;
        }
        
        public async Task<IActionResult> Index(int? kategoriid)
        {
            if (!_SignInManager.IsSignedIn(User)) {
                LocalRedirect(Url.Content("~/Identity/Account/Login"));
            }
            var datanama = "xxx";
            var userName = _userManager.GetUserName(User)!;
            if (!string.IsNullOrEmpty(userName))
            {
                datanama = userName;
            }
            ViewBag.datanama = datanama;

            var userID = _userManager.GetUserId(User)!;

            Pesanan p = _context.Pesanans.AsNoTracking().Where(
                x => x.PesananUserId == userID 
                && x.PesananStatus==0
                ).FirstOrDefault();
            if (p == null)
            {
                DateTime date = DateTime.Now;
                var uid = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
                Pesanan pnew = new Pesanan();
                pnew.PesananStatus = 0;
                pnew.PesananUserId = userID;
                pnew.PesananDate = date;
                pnew.PesananCode = uid;
                _context.Add(pnew);
                await _context.SaveChangesAsync();
                p = pnew;
            }
            else
            {
                
            }

            ViewData["pesanan"] = p;
            

            //_context.Add(pesanan);
            //await _context.SaveChangesAsync();

            List<Menu> m = _context.Menus.ToList();
            List<PesananDetail> pdetail = _context.PesananDetails.ToList();

            //var query =
            //        from mn in _context.Menus
            //        join pd in _context.PesananDetails on mn.MenuId equals pd.PesananMenuId
            //        where pd.PesananDpid == p.PesananId
            //        select new
            //        {
            //            PesananDid = pd.PesananDid,
            //            PesananDpid = pd.PesananDpid,
            //            PesananMenuId = pd.PesananMenuId,
            //            PesananPrice = pd.PesananPrice,
            //            PesananStatus = pd.PesananStatus,
            //            PesananQty = pd.PesananQty,
            //            MenuId = mn.MenuId,
            //            MenuNama = mn.MenuNama,
            //            MenuImage = mn.MenuImage,
            //            MenuKategoriId = mn.MenuKategoriId,
            //            MenuPrice = mn.MenuPrice,
            //            MenuStatus = mn.MenuStatus,
            //        };
            //var q = query.ToList();
            var q = _context.Menus.Join(
                _context.PesananDetails,
                mn => mn.MenuId,
                pd => pd.PesananMenuId,
                (mn, pd) => new
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
                }).Where(x=>x.PesananDpid==p.PesananId).ToList();
            _logger.LogInformation(q.ToString());
            //ViewData["PesananDetail"] = q;
            ViewBag.PesananDetail = q;
            if (kategoriid.HasValue)
            {
                m = (List<Menu>)m.Where(x => x.MenuKategoriId == kategoriid).ToList();
            }
            List<KategoriMenu> km = _context.KategoriMenus.ToList();
            ViewData["Menu"] = m;
            ViewData["KategoriMenu"] = km;
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




        public async Task<IActionResult> InsertDetailPesanan(PesananDetail pesanandetail)
        {
            if (ModelState.IsValid)
            {
                var userID = _userManager.GetUserId(User)!;
                Pesanan p = _context.Pesanans.AsNoTracking().Where(
                x => x.PesananUserId == userID
                && x.PesananStatus == 0
                ).FirstOrDefault();

                PesananDetail pd = _context.PesananDetails.AsNoTracking().Where(
                x => x.PesananMenuId == pesanandetail.PesananMenuId
                && x.PesananStatus == 1 && x.PesananDpid == p.PesananId
                ).FirstOrDefault();

                if(pd== null)
                {
                    _context.Add(pesanandetail);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    pesanandetail.PesananDid = pd.PesananDid;
                    pesanandetail.PesananQty = pd.PesananQty + pesanandetail.PesananQty;
                    _context.Update(pesanandetail);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateDetailPesanan([Bind("PesananDid,PesananQty")] PesananDetail pesanandetail)
        {
            var x = pesanandetail.PesananDid;
            if (ModelState.IsValid)
            {
                PesananDetail p = _context.PesananDetails.AsNoTracking().Where(
                x => x.PesananDid == pesanandetail.PesananDid
                && x.PesananStatus == 1
                ).FirstOrDefault();

                if (p == null)
                {
                    ViewBag.Message = "Tidak ditemukan detail pesanan" ;
                    return View("~/Views/Shared/Error_Message.cshtml");
                }
                else
                {
                    p.PesananQty = pesanandetail.PesananQty;
                    _context.Update(p);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> PrintOrderPDFHtml(string idorder)
        {
            if (!_SignInManager.IsSignedIn(User))
            {
                LocalRedirect(Url.Content("~/Identity/Account/Login"));
            }
            var datanama = "xxx";
            var userName = _userManager.GetUserName(User)!;
            if (!string.IsNullOrEmpty(userName))
            {
                datanama = userName;
            }
            ViewBag.datanama = datanama;

            var userID = _userManager.GetUserId(User)!;

            Pesanan p = _context.Pesanans.AsNoTracking().Where(
                x => x.PesananId == int.Parse(idorder)
                ).FirstOrDefault();
            var query =
                    from mn in _context.Menus
                    join pd in _context.PesananDetails on mn.MenuId equals pd.PesananMenuId
                    where pd.PesananDpid == p.PesananId
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

            ViewData["pesanan"] = p;
            ViewBag.PesananDetail = q;
            return View();
        }

        public async Task<IActionResult> PrintOrderPDF(string idorder)
        {
            Pesanan p = _context.Pesanans.AsNoTracking().Where(
                x => x.PesananId == int.Parse(idorder)
                ).FirstOrDefault();
            p.PesananStatus = 1;
            _context.Update(p);
            await _context.SaveChangesAsync();
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            BlinkConverterSettings settings = new BlinkConverterSettings();
            settings.ViewPortSize = new Syncfusion.Drawing.Size(1440, 0);
            htmlConverter.ConverterSettings = settings;
            var url = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, "/Home/PrintOrderPDFHtml?idorder="+ idorder);
            PdfDocument document = htmlConverter.Convert(url);
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "order-"+p.PesananCode+".pdf");
        }

        public async Task<IActionResult> DeleteDetailPesanan(int id)
        {
            var pesanan = await _context.PesananDetails.FindAsync(id);
            if (pesanan != null)
            {
                _context.PesananDetails.Remove(pesanan);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }




}