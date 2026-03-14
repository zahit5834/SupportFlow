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
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateTicketStatusDto dto)
        {
            try
            {
                await _ticketService.UpdateStatusAsync(id, dto);
                return Ok(new { message = "Ticket durumu ve atama bilgisi güncellendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(Guid id, [FromBody] AddCommentDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var fullName = User.FindFirst("FullName")?.Value ?? "Bilinmeyen Kullanıcı"; // Claims'ten ismi çekiyoruz

            await _ticketService.AddCommentAsync(id, userId, fullName, dto);
            return Ok(new { message = "Yorum başarıyla eklendi." });
        }

        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetComments(Guid id)
        {
            var result = await _ticketService.GetCommentsAsync(id);
            return Ok(result);
        }

        public async Task<IActionResult> GetMyTasks()
        {
            var staffId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _ticketService.GetAssignedTicketsAsync(staffId);
            return Ok(result);
        }

        [HttpGet("pool")]
        public async Task<IActionResult> GetUnassignedPool()
        {
            var result = await _ticketService.GetUnassignedTicketsAsync();
            return Ok(result);
        }

        [HttpPatch("{id}/assign")]
        public async Task<IActionResult> AssignToMe(Guid id)
        {
            // Token'dan personelin bilgilerini alıyoruz
            var staffId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var staffName = User.FindFirst("FullName")?.Value ?? "Destek Personeli";

            try
            {
                await _ticketService.AssignTicketAsync(id, staffId, staffName);
                return Ok(new { message = "Bilet başarıyla üzerinize atandı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
