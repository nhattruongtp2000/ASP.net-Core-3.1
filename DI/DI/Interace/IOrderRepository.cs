
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace DI.DI.Interace
{
    public interface IOrderRepository
    {
        Task<List<OrdersVm>> GetAll();

        Task<OrderDetailsVm> GetDetails(string IdOrder,int IdProduct);

        Task<int> ChangeStatusDetails(string IdOrder, int IdProduct,string x);

        
        
    }
}
