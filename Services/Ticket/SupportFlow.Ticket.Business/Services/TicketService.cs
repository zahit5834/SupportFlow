using Microsoft.EntityFrameworkCore;
using SupportFlow.Ticket.Business.Dtos;
using SupportFlow.Ticket.Business.Interfaces;
using SupportFlow.Ticket.DataAccess.Contexts;
using SupportFlow.Ticket.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SupportFlow.Ticket.Business.Dtos.TicketDtos;

namespace SupportFlow.Ticket.Business.Services
{
    public class TicketService : ITicketService
    {
        private readonly TicketDbContext _context;

        public TicketService(TicketDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateTicketAsync(CreateTicketDto dto, Guid userId, Guid companyId)
        {
            var ticket = new SupportTicket
            {
                Title = dto.Title,
                Description = dto.Description,
                Priority = dto.Priority,
                CreatedByUserId = userId,
                CompanyId = companyId,
                Status = 0
            };

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            // TODO: RabbitMQ ile ticket oluşturma event'i eklenecek.

            return ticket.Id;

        }

        public async Task<List<TicketListDto>> GetCompanyTicketsAsync(Guid companyId)
        {
            return await _context.Tickets
            .Where(x => x.CompanyId == companyId)
            .Select(x => new TicketListDto(x.Id, x.Title, x.Status, x.Priority, x.CreatedAt))
            .ToListAsync();
        }
    }
}
