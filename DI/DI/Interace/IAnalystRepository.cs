using ViewModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DI.DI.Interace
{
    public interface IAnalystRepository
    {
        public Task<List<AnalystAccess>> GetAccessForDay(string month, string year);

        public Task<List<QuantityProducts>> GetTotalQuantityProductsPerMonth(string month, string year);

        Task<List<QuantityProducts>> GetTotalQuantityProducts();

        Task<List<OrdersVm>> GetOrdersPerDay(DateTime date);

        Task<List<OrdersVm>> GetALlProcessOrdersPerDay(DateTime date);

        Task<List<OrdersVm>> GetALlProcessOrdersPerMonthDetails(string month, string year);

        Task<List<SlideVm>> GetSlide();


    }
}
