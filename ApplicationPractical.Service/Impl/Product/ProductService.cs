using ApplicationPractical.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationPractical.Data.Entities;
using ApplicationPractical.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace ApplicationPractical.Service.Impl
{
    public class ProductService : BaseServices<Product>,IProductService
    {
        public ProductService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<bool> GetProductByName(int index)
        {
            try {
                var data = new Product { Name = "hello" };
                InsertWithDefaultValue(data);
                await SaveChangeAsync();
                var hell = "dung";
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
    }
}
