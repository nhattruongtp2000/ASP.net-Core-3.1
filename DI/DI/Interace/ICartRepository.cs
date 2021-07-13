using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace DI.DI.Interace
{
    public interface ICartRepository
    {
        void AddtoCart(int IdProduct);

        void UpdateCart(int IdProduct, int Quantity);

        void RemoveCart(int IdProduct);

        void ClearCart();

        void SaveCart(List<CartItem> ls);

        List<CartItem> GetCartItems();

        Task<List<ProductVm>> GetAll();

        Task<string> Purchase(int total);

        Task<List<OrderDetailsVm>> Checkout(string IdUser);

        
    }
}
