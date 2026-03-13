using SupportFlow.Ticket.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Ticket.Business.Dtos
{
    public class TicketDtos
    {
        public record CreateTicketDto(string Title, string Description, TicketPriority Priority);
        public record TicketListDto(Guid Id, string Title, TicketStatus Status, TicketPriority Priority, DateTime CreatedAt);

        public record UpdateTicketStatusDto(TicketStatus Status, Guid? AssignedStaffId);


        public record AddCommentDto(string Message);
        public record TicketCommentListDto(Guid Id, Guid UserId, string Message, DateTime CreatedAt);
    }
}
