using Data.DB;
using Data.Enums;
using DI.DI.Interace;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ViewModels;
using X.PagedList;

namespace DI.DI.Repository
{
    public class OrderRepository : IOrderRepository
    {

        private readonly Iden2Context _iden2Context;

        public OrderRepository(Iden2Context iden2Context)
        {
            _iden2Context = iden2Context;
        }

        public async Task<int> ChangeStatusDetails(string IdOrder, int IdProduct,string x)
        {

            var change = await _iden2Context.OrderDetails.FirstOrDefaultAsync((x => x.IdOrder == IdOrder && x.IdProduct == IdProduct));
              if(x== "Process")
                {
                    change.StatusDetails = Status.Process;
                }
                else if (x == "Complete")
                {
                    change.StatusDetails = Status.Complete;
                }
                else
                {
                    change.StatusDetails = Status.Delivering;
                }
                return await _iden2Context.SaveChangesAsync();

        }

        

        public async Task<IPagedList<OrdersVm>> GetAll(int? page)
        {
            var orders = from p in _iden2Context.Orders
                         join pt in _iden2Context.OrderDetails on p.IdOrder equals pt.IdOrder
                         select new { p, pt };

            var pageNumber = page ?? 1;
            int pageSize = 10;

            var c =await orders.Select(x => new OrdersVm() 
            { 
            IdOrder=x.p.IdOrder,
            IdProduct=x.pt.IdProduct,
            Status=x.p.Status,
            IdUser=x.p.IdUser,
            OrderDay=x.p.OrderDay,
            TotalPice=x.p.TotalPice
            }).ToPagedListAsync(pageNumber,pageSize);
            return c;
        }

        public async Task<OrderDetailsVm> GetDetails(string IdOrder,int IdProduct)
        {
            var ordersDetails =await _iden2Context.OrderDetails.FirstOrDefaultAsync(x => x.IdOrder == IdOrder && x.IdProduct==IdProduct);
            var c = new OrderDetailsVm()
            {
                IdOrder = ordersDetails.IdOrder,
                StatusDetails = ordersDetails.StatusDetails,
                IdProduct = ordersDetails.IdProduct,
                Price = ordersDetails.Price,
                Quality = ordersDetails.Quality
            };
            return c;
        }

        
    }
}
