using Data.Data;
using Data.DB;
using DI.DI.Interace;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace DI.DI.Repository
{
    public class ContactRepository : IContactRepository
    {
        private readonly Iden2Context _iden2Context;
        public ContactRepository(Iden2Context iden2Context)
        {
            _iden2Context = iden2Context;
        }

        public async Task<int> Feedback(string UserName, string PhoneNumber, string Email, string Content)
        {
            var a = new Feedback()
            {
                Content = Content,
                Status = true,
                CreatedDate = DateTime.Now,
                Email = Email,
                PhoneNumber = PhoneNumber,
                UserName = UserName
            };
            _iden2Context.Add(a);
            return await _iden2Context.SaveChangesAsync();
        }

        public async Task<ContactVm> GetContact()
        {
            var x =await _iden2Context.Contacts.Where(x => x.Status == true).FirstOrDefaultAsync();
            var contact = new ContactVm();
            contact.Content = x.Content;
            contact.Status = x.Status;
            return contact;
        }

        public async Task<int> SendEmailPromotion(string Email)
        {
            var a = _iden2Context.EmailPromotions;
            var email = new EmailPromotion()
            {
                Email = Email
            };
            a.Add(email);
            return await _iden2Context.SaveChangesAsync();
        }
    }
}
