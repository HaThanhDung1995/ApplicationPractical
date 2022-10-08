using ApplicationPractical.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationPractical.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public virtual DbSet<Product> Product { get; set; }
    }
}
