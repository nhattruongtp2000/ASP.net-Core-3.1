using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Data.Data
{
    public class Order
    {
        [Key]
        public string IdOrder { get; set; }

        public string IdUser { get; set; }

        public DateTime OrderDay { get; set; }

        public int TotalPice { get; set; }
        public Status Status { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }

    }
}
