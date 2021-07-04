using Microsoft.AspNetCore.Mvc;
using TPSS.Common;

namespace Leemo.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult CommingSoon()
        {
            return View();
        }
    }
}
