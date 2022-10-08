
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationPractical.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        private IConfigurationRoot configuration { get; }
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-BRF1SIL\\SQLEXPRESS;Initial Catalog=testproduct;MultipleActiveResultSets=True;Persist Security Info=true;User Id=sa;Password=123456;Connection Timeout=300;");
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
