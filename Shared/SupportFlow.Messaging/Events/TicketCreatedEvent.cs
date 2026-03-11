using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Messaging.Events
{
    public record TicketCreatedEvent
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public Guid CompanyId { get; init; }
        public Guid CreatedByUserId { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
