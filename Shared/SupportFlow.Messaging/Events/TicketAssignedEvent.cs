using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Messaging.Events
{
    public record TicketAssignedEvent
    {
        public Guid TicketId { get; init; }
        public string TicketTitle { get; init; } = string.Empty;
        public Guid AssignedStaffId { get; init; }
        public string StaffFullName { get; init; } = string.Empty;
        public Guid TicketOwnerUserId { get; init; }
    }
}
