using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupportFlow.Ticket.Business.Interfaces;
using System.Security.Claims;
using static SupportFlow.Ticket.Business.Dtos.TicketDtos;

namespace SupportFlow.Ticket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTicketDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var companyId = Guid.Parse(User.FindFirst(x => x.Type == "CompanyId").Value);

            var result = await _ticketService.CreateTicketAsync(dto, userId, companyId);
            return Ok(new { id = result, message = "Destek Talebiniz Başarıyla Alındı" });
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCompanyTickets()
        {
            var companyId = Guid.Parse(User.FindFirst(x => x.Type == "CompanyId").Value);
            var result = await _ticketService.GetCompanyTicketsAsync(companyId);

            return Ok(result);
        }

    }
}
