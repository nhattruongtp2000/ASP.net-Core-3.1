using DI.DI.Interace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountRepository _IaccountRepository;
        private readonly IOrderRepository _IorderRepository;

        public AccountsController(IAccountRepository IaccountRepository, IOrderRepository IorderRepository)
        {
            _IaccountRepository = IaccountRepository;
            _IorderRepository = IorderRepository;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm request)
        {
            var x = await _IaccountRepository.Register(request);
            if (ModelState.IsValid)
            {
                if (x != null)
                {
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Accounts", new { x, email = request.UserName }, Request.Scheme);
                    _IaccountRepository.SendTo(request.UserName, "Confirmation email link", confirmationLink);
                    return RedirectToAction(nameof(SuccessRegistration));
                }
                ModelState.AddModelError(string.Empty, "Invalid Login attemp");
            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var x = await _IaccountRepository.ConfirmEmail(token, email);
            if (x == 0)
            {
                return Content("Faild");
            }
            return View(nameof(ConfirmEmail));
        }

        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }




        [HttpPost]
        public IActionResult SendEmail(string To, string Subject, string Body)
        {
            _IaccountRepository.SendTo(To, Subject, Body);
            ViewBag.Message = "Gui mail cho" + To + "Thanh cong";
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm request)
        {
            var x = await _IaccountRepository.Login(request);
            if (ModelState.IsValid)
            {
                if (x == 1)
                {
                    return RedirectToAction("Indexx", "Home");

                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(request);
        }

        public async Task<IActionResult> GetUser()
        {
            if (TempData["Success"] != null)
            {
                ViewBag.Success = TempData["Success"];
            }

            var c = await _IaccountRepository.GetUser();
            return View(c);
        }

        [HttpPost]
        public async Task<IActionResult> GetUser(UserVm     request)
        {
            var c = await _IaccountRepository.EditUser(request);
            if (c == 1)
            {
                TempData["Success"] = "Edit Success";
                return RedirectToAction("GetUser");
            }
            return View();
            
        }

        public async Task<IActionResult> OrderHistory(string IdUser,int? page)
        {
            var x = await _IorderRepository.OrderHistory(IdUser, page);
            return View(x);
        }

        public async Task<IActionResult> OrderHistoryDetails(string IdOrder)
        {
            var x =await _IorderRepository.GetDetails(IdOrder);
            return View(x);
        }


        public  IActionResult ChangePassword()
        {
            if (TempData["Change"] != null)
            {
                ViewBag.Change = TempData["Change"];
            }
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVm request)
        {
            string UserName = User.Identity.Name;

            var x = await _IaccountRepository.ChangePassword(request,UserName);
            if (ModelState.IsValid)
            {
                if (x == 1)
                {
                    TempData["Change"] = "Change Password Success";
                    return RedirectToAction("ChangePassword");
                }
                else
                {
                    TempData["Change"] = "Change Password Failure ";
                    return RedirectToAction("ChangePassword");
                }
            }
            
            return View();
        }

    }
}
