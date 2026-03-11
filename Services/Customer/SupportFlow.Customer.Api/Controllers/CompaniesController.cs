using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SupportFlow.Customer.Business.Dtos;
using SupportFlow.Customer.Business.Interfaces;

namespace SupportFlow.Customer.Api.Controllers
{
    [Route("api/customer/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _companyService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyDto createCompanyDto)
        {
            try
            {
                var companyId = await _companyService.CreateAsync(createCompanyDto);
                return Ok(new { id = companyId, message = "Şirket başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
