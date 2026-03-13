using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Messaging.Events
{
    public record TicketStatusUpdatedEvent
    {
        public Guid TicketId { get; init; }
        public string Title { get; init; } = string.Empty;
        public int NewStatus { get; init; }
        public Guid CreatedByUserId { get; init; } // Bildirimin kime gideceğini bilmek için şart
        public Guid? AssignedStaffId { get; init; } // Atanan personel değişmiş olabilir
    }
}
