using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class ProductCreateVm
    {
        public int IdProduct { get; set; }

        public string ProductName { get; set; }

        public int IdCategory { get; set; }

        public int Price { get; set; }

        public DateTime DateAccept { get; set; }

        public bool UseVoucher { get; set; }

        public int IdBrand { get; set; }

        public IFormFile PhotoReview { get; set; }
    }
}
