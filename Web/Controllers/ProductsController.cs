using DI.DI.Interace;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _iproductRepository;
        private readonly IAnalystRepository _ianalystRepository;
        private readonly ICartRepository _cartRepository;


        public ProductsController(IProductRepository iproductRepository, IAnalystRepository ianalystRepository, ICartRepository cartRepository)
        {
            _iproductRepository = iproductRepository;
            _ianalystRepository = ianalystRepository;
            _cartRepository = cartRepository;

        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllProduct(string key,int? page)
        {

            var c = await _iproductRepository.GetAll2(page);

            if (key != null)
            {
                TempData["Search"] = key;
                 c = await _iproductRepository.Search(key,page);
            }
            
            return View(c);
        }

        public async Task<IActionResult> GetAllHotProduct()
        {
            var x = await _ianalystRepository.GetTotalQuantityProductsPerMonth(DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString());
            return View(x);
        }


        [HttpGet]
        public async Task<IActionResult> GetProductPerCategory(int IdCategory,int? page)
        {
            var x = await _iproductRepository.GetProductPerCategory(IdCategory,page);
            return View(x);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductPerBrand(int IdBrand,int? page)
        {
            var x = await _iproductRepository.GetProductPerBrand(IdBrand,page);
            return View(x);
        }



        public async Task<IActionResult> ProductDetails(int IdProduct)
        {
            var product = await _iproductRepository.GetProductDetail(IdProduct);
            var c = _cartRepository.GetCartItems().Count();
            TempData["CartCount"] = c;
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Filter(int pricemin,int pricemax,int? page)
        {
            var c = await _iproductRepository.Filters(pricemin, pricemax,page);
            return View(c);
        }

        public async Task<IActionResult> Comment(int IdProduct, string Content)
        {
            await _iproductRepository.AddComment(IdProduct, Content);
            return RedirectToAction("ProductDetails",new {IdProduct=IdProduct });
        }

        
    }
}
