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
    public class VoucherRepository : IVoucherRepository
    {
        private readonly Iden2Context _iden2Context;

        public VoucherRepository(Iden2Context iden2Context)
        {
            _iden2Context = iden2Context;
        }


        public async Task<int> CreateNewVoucher(VoucherVm x)
        {
            var voucher = new Voucher()
            {
                VoucherCode = x.VoucherCode,
                VoucherName = x.VoucherName,
                Status = x.Status,
                ApplyForAll = x.ApplyForAll,
                DiscountAmount = x.DiscountAmount,
                DiscountPercent = x.DiscountPercent,
                FromDate = x.FromDate,
                ToDate = x.ToDate,
                TypeVoucher = x.TypeVoucher,
                Quantity=x.Quantity
            };
             _iden2Context.Add(voucher);
            return await _iden2Context.SaveChangesAsync();

        }

        public async Task<List<VoucherVm>> GetAllVoucher()
        {
            var vouchers = _iden2Context.Vouchers;
            var vouchervm = await vouchers.Select(x => new VoucherVm() 
            {
            VoucherCode=x.VoucherCode,
            VoucherName=x.VoucherName,
            Status=x.Status,
            ApplyForAll=x.ApplyForAll,
            DiscountAmount=x.DiscountAmount,
            DiscountPercent=x.DiscountPercent,
            FromDate=x.FromDate,
            ToDate=x.ToDate,
            TypeVoucher=x.TypeVoucher,
                Quantity = x.Quantity
            }).ToListAsync();

            return vouchervm;
        }
    }
}
