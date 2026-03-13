using SupportFlow.Ticket.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Ticket.Entity.Models
{
    public class SupportTicket
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Durum: 0:Açık, 1:İşlemde, 2:Çözüldü, 3:Kapalı
        public TicketStatus Status { get; set; } = TicketStatus.Open;

        // Öncelik: 0:Düşük, 1:Orta, 2:Yüksek, 3:Acil
        public TicketPriority Priority { get; set; } = TicketPriority.Medium;

        // Diğer mikroservislerle olan bağlar (Sadece ID bazlı)
        public Guid CompanyId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid? AssignedStaffId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
