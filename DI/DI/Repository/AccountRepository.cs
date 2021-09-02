using Data.Data;
using Data.DB;
using Data.Utilities.Constants;
using DI.DI.Interace;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace DI.DI.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly Iden2Context _iden2Context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountRepository(Iden2Context iden2Context, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager )
        {
            _iden2Context = iden2Context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _signInManager = signInManager;
            
        }

        public async Task<int> ChangePassword(ChangePasswordVm request, string UserName)
        {


            var user =await _iden2Context.Users.Where(x => x.UserName == UserName).FirstOrDefaultAsync();
            var x= await _userManager.CheckPasswordAsync(user, request.OldPass);
            if (x ==false)
            {
                return 0;
            }
            else
            {
                await _userManager.ChangePasswordAsync(user, request.OldPass, request.NewPass);
            }
            await _iden2Context.SaveChangesAsync();
            return 1;
        }

        public async Task<int> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
             token = _userManager.
                GenerateEmailConfirmationTokenAsync(user).Result;
            if (user == null)
            {
                return 0;
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return 1;
            }

            return 0;
        }

       

        public async Task<int> EditUser(UserVm request)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var IdUser = user.Id;
            var a =await _iden2Context.Users.Where(x => x.Id == IdUser).FirstOrDefaultAsync();
            a.UserName = request.UserName;
            a.Address = request.Address;
            a.Email = request.Email;
            a.PhoneNumber = request.PhoneNumber;

            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<string> GetEmail()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var EMail = user.Email;
            return EMail;
        }

        public async Task<string> GetId()
        {
            var user =await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var IdUser = user.Id;
            return IdUser;
        }

        public async Task<UserVm> GetUser()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var a = new UserVm() 
            { 
            Address=user.Address,
            Email=user.Email,
            PhoneNumber=user.PhoneNumber,
            UserName=user.UserName                   
            };
            return a;
        }

        public async Task<int> Login(LoginVm request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.UserName, request.Pass, request.RememberMe, false);
            if (result.Succeeded)
            {
                return 1;
            }
            return 0;
        }

        public async void Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> Register(RegisterVm request)
        {
            var user = new AppUser()
            {
                UserName = request.UserName,
                Email = request.Email 

            };        
                var result = await _userManager.CreateAsync(user, request.Pass);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (result.Succeeded)
                {
                return token;
                }
            return "";
        }

        public void SendTo(string To, string Subject, string Body)
        {
            MailMessage mm = new MailMessage();
            mm.To.Add(To);
            mm.Subject = Subject;
            mm.Body = Body;
            mm.From = new MailAddress("nhattruongtp2000@gmail.com");
            mm.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = new System.Net.NetworkCredential("nhattruongtp2000@gmail.com", "kiki0336873310");
            smtp.Send(mm);
        }
    }
}
