using Data.DB;
using DI.DI.Interace;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DI.DI.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly Iden2Context _iden2Context;

        public CategoryRepository(Iden2Context iden2Context )
        {
            _iden2Context = iden2Context;


        }
        public async Task<List<CategoryVm>> GetAllCategory()
        {

                var query = from c in _iden2Context.Categories
                            select new { c };
                return await query.Select(x => new CategoryVm()
                {
                    IdCategory=x.c.IdCategory,
                    NameCategory=x.c.NameCategory

                }).ToListAsync();           
        }
    }
}
