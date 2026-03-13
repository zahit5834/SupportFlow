using MassTransit;
using Microsoft.EntityFrameworkCore;
using SupportFlow.Messaging.Events;
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
        private readonly IPublishEndpoint _publishEndpoint;
        public TicketService(TicketDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
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

            await _publishEndpoint.Publish(new TicketCreatedEvent
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                CompanyId = ticket.CompanyId,
                CreatedByUserId = ticket.CreatedByUserId,
                CreatedAt = ticket.CreatedAt
            });

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
