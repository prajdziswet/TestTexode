using Microsoft.AspNetCore.Mvc;
using ServerTest.Models;
using System.Diagnostics;
using ServerTest.Servises;
using System.Xml.Linq;
using System.Globalization;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System;

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
            return View();
        }

        [HttpGet]
        public IActionResult GetData(string currencyName="USD",string date1=null,string date2=null)
        {
            if (String.IsNullOrEmpty(date1))
            {
                date1=DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd");

            };
            if (String.IsNullOrEmpty(date2)) date2 = date1;

            try
            {
                KeyObj key = new KeyObj(date1, date2, currencyName);
                List<MyCurrency> list = Cashe.GetOrCreate(key).Result;
                string json = JsonSerializer.Serialize<MyCurrency[]>(list.ToArray());
                return Content(json);
            }
            catch (Exception e)
            {
                return StatusCode(404);
            }
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