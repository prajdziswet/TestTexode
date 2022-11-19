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
            DateOnly date1=AddOrGetInDB.ParseDate("2022-11-17");
            DateOnly date2 = AddOrGetInDB.ParseDate("2022-11-17");
            List<MyCurrency> list = BitCoin.getListMyCurrencies(date1, date2);
            date2 = AddOrGetInDB.ParseDate("2022-11-18");
            list = BitCoin.getListMyCurrencies(date1, date2);
            date2 = AddOrGetInDB.ParseDate("2022-11-19");
            list = BitCoin.getListMyCurrencies(date1, date2);
            date1 = AddOrGetInDB.ParseDate("2022-11-19");
            list = BitCoin.getListMyCurrencies(date1, date2);
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