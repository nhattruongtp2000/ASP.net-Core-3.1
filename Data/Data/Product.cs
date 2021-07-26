using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Data
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }

        public int IdCategory { get; set; }

        public string Description { get; set; }

        public string Content { get; set; }

        public string ProductName { get; set; }

        public int Price { get; set; }

        public DateTime DateAccept { get; set; }

        public bool UseVoucher { get; set; }

        public int IdBrand { get; set; }

        public string PhotoReview { get; set; }

        public bool IsFree { get; set; }

    }
}
