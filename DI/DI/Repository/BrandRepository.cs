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
    public class BrandRepository : IBrandRepository
    {
        private readonly Iden2Context _iden2Context;
        public BrandRepository(Iden2Context iden2Context)
        {
            _iden2Context = iden2Context;
        }
        public async Task<List<BrandVm>> GetAllBrand()
        {

            var query = from c in _iden2Context.Brands
                        select new { c };
            return await query.Select(x => new BrandVm()
            {
                IdBrand = x.c.IdBrand,
                BrandName = x.c.BrandName

            }).ToListAsync();
        }
    }
}
