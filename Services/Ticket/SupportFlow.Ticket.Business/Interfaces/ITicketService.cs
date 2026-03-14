using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SupportFlow.Ticket.Business.Dtos.TicketDtos;

namespace SupportFlow.Ticket.Business.Interfaces
{
    public interface ITicketService
    {
        Task<Guid> CreateTicketAsync(CreateTicketDto dto, Guid userId, Guid companyId);
        Task<List<TicketListDto>> GetCompanyTicketsAsync(Guid companyId);

        Task UpdateStatusAsync(Guid ticketId, UpdateTicketStatusDto dto);

        Task AddCommentAsync(Guid ticketId, Guid userId, string fullName, AddCommentDto dto);
        Task<List<TicketCommentListDto>> GetCommentsAsync(Guid ticketId);


        Task AssignTicketAsync(Guid ticketId, Guid staffId, string staffFullName);


        Task<List<TicketListDto>> GetAssignedTicketsAsync(Guid staffId);
        Task<List<TicketListDto>> GetUnassignedTicketsAsync();

    }
}
