using DI.DI.Interace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Home/[Action]")]
    public class HomeController : Controller
    {
        private readonly IAccountRepository _IaccountRepository;
        private readonly IAnalystRepository _IanalystRepository;

        
        public HomeController( IAccountRepository iaccountRepository,IAnalystRepository ianalystRepository)
        {
            _IaccountRepository = iaccountRepository;
            _IanalystRepository = ianalystRepository;
        }


        [Authorize]        
        public async Task<IActionResult> Index()
        {
            await _IaccountRepository.CountAccess();
            var x =await GetOrdersPerDay();
            var y = await GetALlProcessOrdersPerDay();
            ViewBag.GetOD = x;
            ViewBag.GetAP = y;
            return View();
        }

        public async Task<int> GetOrdersPerDay()
        {
            var date = DateTime.Now;
            var x =await  _IanalystRepository.GetOrdersPerDay(date);

            return x.Count();
        }
        public async Task<IActionResult> GetOrdersPerDayDetails()
        {
            var date = DateTime.Now;
            var x = await _IanalystRepository.GetOrdersPerDay(date);
            return View(x);
        }

        public async Task<int> GetALlProcessOrdersPerDay()
        {
            var date = DateTime.Now;
            var x = await _IanalystRepository.GetALlProcessOrdersPerDay(date);

            return x.Count();
        }

        [HttpGet]
        public async Task<IActionResult> GetALlProcessOrdersPerDayDetails()
        {
            var date = DateTime.Now;
            var x = await _IanalystRepository.GetALlProcessOrdersPerDay(date);
            return View(x);
        }




        [HttpPost]
        public async Task<IActionResult> GetALlProcessOrdersPerMonthDetails(string month, string year)
        {
            var x = await _IanalystRepository.GetALlProcessOrdersPerMonthDetails(month,year);
            return View(x);
        }

    }
}
