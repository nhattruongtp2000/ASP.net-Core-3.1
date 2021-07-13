using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModel.ViewModels
{
    public class LoginVm
    {
     
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Pass { get; set; }

        public bool RememberMe { get; set; }

    }
}
