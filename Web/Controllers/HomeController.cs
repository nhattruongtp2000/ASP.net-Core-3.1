using DI.DI.Interace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ViewModels;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _iproductRepository;
        private readonly IAnalystRepository _ianalystRepository;
        private readonly ICartRepository _cartRepository;

        public HomeController(IProductRepository iproductRepository,IAnalystRepository ianalystRepository,ICartRepository cartRepository)
        {
            _iproductRepository = iproductRepository;
            _ianalystRepository = ianalystRepository;
            _cartRepository = cartRepository;
        }


        [Authorize]
        public async Task<IActionResult> Indexx()
        {
            var x = new HomeVm()
            {
                HomeSlide = await _ianalystRepository.GetSlide(),
                HotSeller = await _ianalystRepository.GetTotalQuantityProductsPerMonth(DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString()),
                TopSeller = await _ianalystRepository.GetTotalQuantityProducts(),
                NewProduct = await _iproductRepository.GetNewProduct()
            };
            var c = _cartRepository.GetCartItems().Count();
            TempData["CartCount"] = c;

            return View(x);
        }
    }
}
