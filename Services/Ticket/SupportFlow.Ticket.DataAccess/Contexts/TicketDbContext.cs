using Microsoft.EntityFrameworkCore;
using SupportFlow.Ticket.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Ticket.DataAccess.Contexts
{
    public class TicketDbContext : DbContext
    {
        public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options) { }

        public DbSet<SupportTicket> Tickets { get; set; }
    }
}
