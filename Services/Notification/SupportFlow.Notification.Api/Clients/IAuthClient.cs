using Refit;
namespace SupportFlow.Notification.Api.Clients
{
    public interface IAuthClient
    {
        [Get("/api/auth/users/{id}")]
        Task<UserDto> GetUserByIdAsync(Guid id);
    }

    public record UserDto(Guid Id, string FullName, string Email);
}
