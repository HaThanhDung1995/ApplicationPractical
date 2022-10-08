using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationPractical.Service.Interface
{
    public interface IProductService
    {
        Task<bool> GetProductByName(int index);
    }
}
