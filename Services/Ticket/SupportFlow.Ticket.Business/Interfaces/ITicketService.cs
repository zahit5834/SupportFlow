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
    }
}
