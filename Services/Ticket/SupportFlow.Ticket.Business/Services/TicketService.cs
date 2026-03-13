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
using System.Security.Cryptography.X509Certificates;
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

        public async Task AddCommentAsync(Guid ticketId, Guid userId, string fullName, AddCommentDto dto)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == ticketId);
            if (ticket == null) throw new Exception("Ticket bulunamadı");

            var comment = new TicketComment
            {
                TicketId = ticketId,
                UserId = userId,
                Message = dto.Message
            };

            await _context.TicketComments.AddAsync(comment);
            await _context.SaveChangesAsync();

            await _publishEndpoint.Publish(new TicketCommentAddedEvent
            {
                TicketId = ticket.Id,
                TicketTitle = ticket.Title,
                CommenterFullName = fullName,
                Message = dto.Message,
                TicketOwnerUserId = ticket.CreatedByUserId
            });


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

        public async Task<List<TicketCommentListDto>> GetCommentsAsync(Guid ticketId)
        {
            return await _context.TicketComments
                .Where(x => x.Id == ticketId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new TicketCommentListDto(x.Id, x.UserId, x.Message, x.CreatedAt))
                .ToListAsync();
        }

        public async Task<List<TicketListDto>> GetCompanyTicketsAsync(Guid companyId)
        {
            return await _context.Tickets
            .Where(x => x.CompanyId == companyId)
            .Select(x => new TicketListDto(x.Id, x.Title, x.Status, x.Priority, x.CreatedAt))
            .ToListAsync();
        }

        public async Task UpdateStatusAsync(Guid ticketId, UpdateTicketStatusDto dto)
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == ticketId);
            if (ticket == null) throw new Exception("Ticket bulunamadı");

            ticket.Status = dto.Status;
            ticket.UpdatedAt = DateTime.UtcNow;

            // Eğer bir personel atandıysa güncelle
            if (dto.AssignedStaffId.HasValue)
                ticket.AssignedStaffId = dto.AssignedStaffId.Value;

            await _context.SaveChangesAsync();

            // Mesajı fırlat: Artık Notification servisi Title için sana dönmeyecek!
            await _publishEndpoint.Publish(new TicketStatusUpdatedEvent
            {
                TicketId = ticket.Id,
                Title = ticket.Title,
                NewStatus = (int)ticket.Status,
                CreatedByUserId = ticket.CreatedByUserId
            });

        }
    }
}
