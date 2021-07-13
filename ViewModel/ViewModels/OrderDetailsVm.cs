using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Enums;

namespace ViewModel.ViewModels
{
    public class OrderDetailsVm
    {
        public string IdOrder { get; set; }

        public int IdProduct { get; set; }

        public int Quality { get; set; }

        public int Price { get; set; }

        public string PhotoReview { get; set; }

        public DateTime DateOrder { get; set; }

        public Status StatusDetails { get; set; } 

    }
}
