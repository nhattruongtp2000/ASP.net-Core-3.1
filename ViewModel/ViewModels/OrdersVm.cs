using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Enums;

namespace ViewModel.ViewModels
{
    public class OrdersVm
    {
        public string IdOrder { get; set; }

        public int IdProduct { get; set; }

        public string IdUser { get; set; }

        public DateTime OrderDay { get; set; }

        public int TotalPice { get; set; }

        public Status Status { get; set; }

    }
}
