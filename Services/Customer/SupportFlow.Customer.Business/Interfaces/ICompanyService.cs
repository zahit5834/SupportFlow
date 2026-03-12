using SupportFlow.Customer.Business.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Customer.Business.Interfaces
{
    public interface ICompanyService
    {
        Task<List<CompanyListDto>> GetAllAsync();
        Task<Guid> CreateAsync(CreateCompanyDto dto);

        Task<CompanyResponseDto> GetCompanyByIdAsync(Guid id);
    }
}
