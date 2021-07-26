using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Data
{
    public class Slide
    {
        [Key]
        public int Id { get; set; }

        public string SlideName { get; set; }

        public int IdProduct { get; set; }

        public int IdCategory { get; set; }

        public int IdBrand { get; set; }


        public string FromFile { get; set; }
    }
}
