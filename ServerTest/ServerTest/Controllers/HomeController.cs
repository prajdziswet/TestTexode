using Microsoft.AspNetCore.Mvc;
using ServerTest.Models;
using System.Diagnostics;
using ServerTest.Servises;
using System.Xml.Linq;
using System.Globalization;

namespace ServerTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            AddOrGetInDB ind= new AddOrGetInDB();
            KeyObj key = new KeyObj("2022-11-15", "2022-11-17", "USD");
            List<MyCurrency> list = ind.GetCurrencies(key);
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