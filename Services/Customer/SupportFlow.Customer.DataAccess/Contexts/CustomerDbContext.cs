using Microsoft.EntityFrameworkCore;
using SupportFlow.Customer.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Customer.DataAccess.Contexts
{
    public class CustomerDbContext: DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options): base(options){ }

        public DbSet<Company> Companies { get; set; }
    }
}
