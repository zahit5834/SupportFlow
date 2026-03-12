using Refit;

namespace SupportFlow.Notification.Api.Clients
{
    public interface ICustomerClient
    {
        [Get("/api/customer/companies/{id}")]
        Task<CompanyDto> GetCompanyByIdAsync(Guid id);
    }

    public record CompanyDto(Guid Id, string Name);
}
