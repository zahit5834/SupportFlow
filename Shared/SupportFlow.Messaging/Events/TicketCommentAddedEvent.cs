using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Messaging.Events
{
    public record TicketCommentAddedEvent
    {
        public Guid TicketId { get; init; }
        public string TicketTitle { get; init; } = string.Empty;
        public string CommenterFullName { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public Guid TicketOwnerUserId { get; init; } // Bildirim bu kişiye gidecek
    }
}
