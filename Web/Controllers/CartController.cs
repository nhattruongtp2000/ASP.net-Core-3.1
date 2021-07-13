using DI.DI.Interace;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly IAccountRepository _accountRepository;
        public CartController(ICartRepository cartRepository, IAccountRepository accountRepositor)
        {
            _cartRepository = cartRepository;
            _accountRepository = accountRepositor;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartRepository.GetAll();
            return View(cart);
        }

        public IActionResult AddToCart(int IdProduct)
        {
            _cartRepository.AddtoCart(IdProduct);
            var c = _cartRepository.GetCartItems().Count();
            TempData["CartCount"] = c;
            return RedirectToAction("ProductDetails","Products",new {IdProduct=IdProduct });
        }

        [HttpPost]
        public IActionResult UpdateCart(int IdProduct, int Quantity)
        {
            _cartRepository.UpdateCart(IdProduct, Quantity);
            return Ok();
        }

        public IActionResult RemoveCart(int IdProduct)
        {
            _cartRepository.RemoveCart(IdProduct);
            return RedirectToAction("Cart");

        }


       
        public IActionResult Cart()
        {
            
            return View(_cartRepository.GetCartItems());
        }

        public async Task<IActionResult> Purchase(int Total)
        {
           string a= await _cartRepository.Purchase(Total);
            _accountRepository.SendTo(User.Identity.Name, "Đơn hàng đã được xác nhận", "ĐƠn hàng" + a + "đang trong quá trị xử lý,cảm ơn bạn đã tin tương chúng tôi");
            return Ok();
        }

        public async Task<IActionResult> Checkout()
        {
            var IdUser =await _accountRepository.GetId();
            var a = await _cartRepository.Checkout(IdUser);
            return View(a);
        }

        public IActionResult Success()
        {
            return View();
        }
       

    }
}
