using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModel.ViewModels
{
    public class HomeVm
    {
        public List<QuantityProducts> HotSeller { get; set; }
        public List<QuantityProducts> TopSeller { get; set; }
        public List<SlideVm>  HomeSlide { get; set; }

        public List<ProductVm> NewProduct { get; set; }
    }
}
