using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmeraldProg.Models;
using Microsoft.EntityFrameworkCore;

namespace EmeraldProg.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmeraldContext _context;

        public HomeController(ILogger<HomeController> logger, EmeraldContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var totalamount = _context.Items
                .Sum(i => (i.ItemPrice*i.Qty)+i.InstalPrice);

            var contentsamount = _context.Items.Where(i=>i.ItemTypeID !=1 && i.ItemTypeID !=3)
                .Sum(i => (i.ItemPrice * i.Qty) + i.InstalPrice);

            var itemscount = _context.Items
                .Count();

            var firstrundate = _context.Items
                .Min(i => i.RunDate);

            var lastrundate = _context.Items
                .Max(i => i.RunDate);



            ViewBag.TotalAmount = ((double)totalamount).ToString("C");

            ViewBag.ContentsAmount = ((double)contentsamount).ToString("C");

            ViewBag.ItemsCount = itemscount;

            ViewBag.FirstRunDate = ((DateTime)firstrundate).ToString("dd/MM/yyyy");
            ViewBag.LastRunDate = ((DateTime)lastrundate).ToString("dd/MM/yyyy");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
