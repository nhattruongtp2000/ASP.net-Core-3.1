using DI.DI.Interace;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Analyst/[Action]")]
    public class AnalystController : Controller
    {
        private readonly IAnalystRepository _IanalystRepository;
        public AnalystController(IAnalystRepository IanalystRepository)
        {
            _IanalystRepository = IanalystRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AnalystAccess()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AnalystAccess(string month, string year)
        {
            var c = await _IanalystRepository.GetAccessForDay(month, year);
            List<int> listmonth = new List<int>();

            ViewBag.month = month;
            ViewBag.year = year;

            for(int i = 0; i <30; i++)
            {
                if (i<c.Count() && c[i]==null)
                {
                    listmonth.Add(0);
                }
                else if(i < c.Count() && c[i] != null)
                {
                    listmonth.Add(c[i].NumberOfAccesss);
                }
                else
                {
                    listmonth.Add(0);
                }
            }
            ViewBag.listmonth = JsonConvert.SerializeObject(listmonth);
            return View(c);
        }

        public async Task<IActionResult> AnalystQuantityProduct()
        {
            var a = await _IanalystRepository.GetTotalQuantityProducts();

            return View(a);
        }

        public IActionResult AnalystQuantityProductPerMonth()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AnalystQuantityProductPerMonth(string month,string year)
        {
            ViewBag.month = month;
            ViewBag.year = year;
            var a = await _IanalystRepository.GetTotalQuantityProductsPerMonth(month,year);
            var Quantity = new List<int>();
            for(int i = 0; i < a.Count(); i++)
            {
                Quantity.Add(a[i].TotalQuantity);
            }
            ViewBag.list = JsonConvert.SerializeObject(Quantity);

            return View(a);
        }
    }
}
