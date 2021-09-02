using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace DI.DI.Interace
{
    public interface IBrandRepository
    {
        Task<List<BrandVm>> GetAllBrand();
    }
}
