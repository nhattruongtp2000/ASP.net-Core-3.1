using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DI.DI.Interace
{
    public interface IBrandRepository
    {
        List<string> GetAllBrand();
    }
}
