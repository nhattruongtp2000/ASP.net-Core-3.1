using DI.DI.Interace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Product/[Action]")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _IproductRepository;
        private readonly IBrandRepository _IbrandRepository;
        public ProductController(IProductRepository productRepository, IBrandRepository IbrandRepository)
        {
            _IproductRepository = productRepository;
            _IbrandRepository = IbrandRepository;
        }

        
        public async Task<IActionResult> Index(string key)
        {
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }

            if (TempData["Edit"] != null)
            {
                ViewBag.Edit = TempData["Edit"];
            }

            if (TempData["AddImage"] != null)
            {
                ViewBag.AddImage = TempData["AddImage"];
            }



            var prSearch = await _IproductRepository.Search(key);
            var products = await _IproductRepository.GetAll();
            if (prSearch.Count > 0)
            {
                return View(prSearch);
            }           

            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var brands = _IbrandRepository.GetAllBrand();
            ViewBag.Brands = brands.Select(x => new SelectListItem()
            {
                Value = x.ToString(),
                Text=x.ToString()
            });
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVm request)
        {
            var products = await _IproductRepository.Create(request);
            if (products>0)
            {
                TempData["Success"] = "Add Successful";
                return RedirectToAction("Index");
            }
            return Content("Add Failure");            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int IdProduct)
        {
            var c = await _IproductRepository.GetProduct(IdProduct);
            return View(c); 
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int IdProduct,ProductVm request)
        {
            var product = await _IproductRepository.Edit(IdProduct, request);
            if (product >0)
            {
                TempData["Edit"] = "Edit Successful";
                return RedirectToAction("Index");
            }
            return Content("Edit Failure");
        }


        [HttpGet]
        public IActionResult AddImages(int IdProduct)
        {
            ViewBag.Id = IdProduct;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddImages(int IdProduct, List<IFormFile> formFile)
        {
            var a = await _IproductRepository.AddImages(IdProduct, formFile);
            if (a != 0)
            {
                TempData["AddImage"] = "Add new Images Success";
                return Redirect("Admin/Product/Index");
            }
            return Content("Add Failure");
        }

    }
}
