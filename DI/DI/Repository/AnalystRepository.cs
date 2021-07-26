using Data.DB;
using Data.Enums;
using DI.DI.Interace;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace DI.DI.Repository
{
    public class AnalystRepository : IAnalystRepository
    {
        private readonly Iden2Context _iden2Context;
        public AnalystRepository(Iden2Context iden2Context)
        {
            _iden2Context = iden2Context;
        }
        public async Task<List<AnalystAccess>> GetAccessForDay(string month, string year)
        {
            int month2 = int.Parse(month);
            int year2 = int.Parse(year);
            var c = _iden2Context.Accesses.Where(x => x.DateAcess.Month == month2 && x.DateAcess.Year == year2);
            var a =await c.Select(x => new AnalystAccess() 
            { 
            DateAccess=x.DateAcess.Day,
            NumberOfAccesss=x.NumberOfAccess    
            }).ToListAsync();
            return a;
        }

        public async Task<List<OrdersVm>> GetALlProcessOrdersPerDay(DateTime date)
        {
            var get = _iden2Context.Orders.Where(x=>x.Status== Status.Process && x.OrderDay.DayOfYear==date.DayOfYear);
            var c = await get.Select(x => new OrdersVm()
            {
                IdOrder = x.IdOrder,
                Status = x.Status,
                IdProduct = 1,
                IdUser = x.IdUser,
                OrderDay = x.OrderDay,
                TotalPice = x.TotalPice
            }).ToListAsync();
            return c;
        }

        public async Task<List<OrdersVm>> GetALlProcessOrdersPerMonthDetails(string month, string year)
        {
            int month2 = int.Parse(month);
            int year2 = int.Parse(year);
            var orderpermonth = _iden2Context.Orders.Where(x => x.OrderDay.Month == month2 && x.OrderDay.Year == year2 &&x.Status== Status.Process);

            var c = await orderpermonth.Select(x => new OrdersVm()
            {
                IdOrder = x.IdOrder,
                Status = x.Status,
                IdProduct = 1,
                IdUser = x.IdUser,
                OrderDay = x.OrderDay,
                TotalPice = x.TotalPice
            }).ToListAsync();

            return c;
        }

        public async Task<List<OrdersVm>> GetOrdersPerDay(DateTime date)
        {
            var get = _iden2Context.Orders.Where(x => x.OrderDay.DayOfYear == date.DayOfYear);
            var x = await get.Select(x => new OrdersVm() 
            { 
               IdOrder=x.IdOrder,
               Status=x.Status,
               IdProduct=1,
               IdUser=x.IdUser,
               OrderDay=x.OrderDay,
               TotalPice=x.TotalPice
            }).ToListAsync();
            return x;
        }

        public async Task<List<SlideVm>> GetSlide()
        {
            var x = _iden2Context.Slides;
            var xx = await x.Select(x => new SlideVm() 
            { 
            SlideName=x.SlideName,
            FromFile=x.FromFile,
            Id=x.Id,
            IdBrand=x.IdBrand,
            IdCategory=x.IdCategory,
            IdProduct=x.IdProduct
            }).ToListAsync();
            return xx;
        }



        public async Task<List<QuantityProducts>> GetTotalQuantityProducts()
        {
            var orderDetails = _iden2Context.OrderDetails;
            var product = _iden2Context.Products;

            var k = from p in orderDetails
                   join  pt in product on p.IdProduct equals pt.IdProduct
                    select new { p, pt };

            var x = await k.Select(x=>new QuantityProducts()
            {
            IdProduct=x.pt.IdProduct,
            TotalQuantity=x.p.Quality*x.p.Price,
            IFromFile=x.pt.PhotoReview,
            Price=x.p.Price,
            Description=x.pt.Description,
            ProductName=x.pt.ProductName
            }).ToListAsync();

            List<int> Id = await product.Select(x => x.IdProduct).ToListAsync();

            List<QuantityProducts> a = new List<QuantityProducts>();

            for(int i = 0; i < Id.Count(); i++)
            {
                var c = new QuantityProducts()
                {
                    IdProduct = Id[i],
                    TotalQuantity = 0
                };
                a.Add(c);
            }
            for(int i = 0; i < a.Count(); i++)
            {
                for(int j = 0; j < x.Count(); j++)
                {
                    if (a[i].IdProduct == x[j].IdProduct)
                    {
                        a[i].TotalQuantity += x[j].TotalQuantity;
                        a[i].IFromFile = x[j].IFromFile;
                        a[i].Price = x[j].Price;
                        a[i].Description = x[j].Description;
                        a[i].ProductName = x[j].ProductName;
                    }
                }
            }
            var aa = a.OrderByDescending(x => x.TotalQuantity).ToList();
            return aa;
        }

            public async Task<List<QuantityProducts>> GetTotalQuantityProductsPerMonth(string month, string year)
            {
                int month2 = int.Parse(month);
                int year2 = int.Parse(year);
                var orderpermonth = from p in _iden2Context.Orders
                                    join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
                                    join ptt in _iden2Context.Products on pt.IdProduct equals ptt.IdProduct
                                    select new { p, pt,ptt };
                var orderDetails = orderpermonth.Where(x => x.p.OrderDay.Month == month2 && x.p.OrderDay.Year == year2);
                var product = _iden2Context.Products;

                var x = await orderDetails.Select(x => new QuantityProducts()
                {
                    IdProduct = x.pt.IdProduct,
                    TotalQuantity = x.pt.Quality * x.pt.Price,
                    IFromFile =x.ptt.PhotoReview,
                    Price=x.pt.Price,
                    Description = x.ptt.Description,
                    ProductName = x.ptt.ProductName

                }).ToListAsync();

                List<int> Id = await product.Select(x => x.IdProduct).ToListAsync();

                List<QuantityProducts> a = new List<QuantityProducts>();

                for (int i = 0; i < Id.Count(); i++)
                {
                    var c = new QuantityProducts()
                    {
                        IdProduct = Id[i],
                        TotalQuantity = 0
                    };
                    a.Add(c);
                }
                for (int i = 0; i < a.Count(); i++)
                {
                    for (int j = 0; j < x.Count(); j++)
                    {
                        if (a[i].IdProduct == x[j].IdProduct)
                        {
                            a[i].TotalQuantity += x[j].TotalQuantity;
                            a[i].IFromFile = x[j].IFromFile;
                            a[i].Price = x[j].Price;
                        a[i].Description = x[j].Description;
                        a[i].ProductName = x[j].ProductName;
                        }
                    }
                }
                var aa = a.OrderByDescending(x => x.TotalQuantity).ToList();
                return aa;
            }
    }
}
