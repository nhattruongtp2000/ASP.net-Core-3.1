
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Data
{

    public class OrderDetails
    {
        public string IdOrder { get; set; }

        public int IdProduct { get; set; }

        public int Quality { get; set; }

        public int Price { get; set; }

        public Status StatusDetails { get; set; }

        public Order Order { get; set; }
    }
}
