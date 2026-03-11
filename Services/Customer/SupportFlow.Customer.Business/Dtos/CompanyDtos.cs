using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportFlow.Customer.Business.Dtos
{

    public record CreateCompanyDto(string Name, string? TaxNumber, string? Email, string? Address);
    public record CompanyListDto(Guid Id, string Name, string? Email, bool IsActive);

}
