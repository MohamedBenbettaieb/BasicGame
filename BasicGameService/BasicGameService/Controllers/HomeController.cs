using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BasicGame.Models; // adjust namespace if needed

namespace BasicGame.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(); // returns Views/Home/Index.cshtml
        }
        public IActionResult Privacy()
        {
            return View(); // looks for Views/Home/Privacy.cshtml
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(model);
        }
    }
}
