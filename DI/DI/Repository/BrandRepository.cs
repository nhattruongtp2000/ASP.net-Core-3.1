using Data.DB;
using DI.DI.Interace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DI.DI.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly Iden2Context _iden2Context;
        public BrandRepository(Iden2Context iden2Context)
        {
            _iden2Context = iden2Context;
        }
        public List<string> GetAllBrand()
        {
            var a = _iden2Context.Brands;
            var Brands = new List<string>();
            foreach(var item in a)
            {
                Brands.Add(item.BrandName);
            }
            return Brands;           
        }
    }
}
