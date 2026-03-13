using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Ticket.Entity.Models
{
    public class TicketComment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TicketId { get; set; }
        public Guid UserId { get; set; } // Mesajı yazan kişi (Müşteri veya Personel)
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property (İsteğe bağlı, EF Core ilişkisi için)
        public SupportTicket Ticket { get; set; } = null!;
    }
}
