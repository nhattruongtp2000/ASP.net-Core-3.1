using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Data
{
    public class Access
    {
        [Key]
        public DateTime DateAcess { get; set; }

        public int NumberOfAccess { get; set; }
    }
}
