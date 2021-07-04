using DockerAspTestDbConnection.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DockerAspTestDbConnection.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductDBContext _context;
        public HomeController(ILogger<HomeController> logger, ProductDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //ViewBag.MyCon = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ListProducts()
        {
            var products = _context.Product.ToList();

            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
