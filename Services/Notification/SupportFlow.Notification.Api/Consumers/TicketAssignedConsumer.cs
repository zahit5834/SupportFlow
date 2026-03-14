using MassTransit;
using SupportFlow.Messaging.Events;
using SupportFlow.Notification.Api.Clients;

namespace SupportFlow.Notification.Api.Consumers
{
    public class TicketAssignedConsumer:IConsumer<TicketAssignedEvent>
    {
        private readonly IAuthClient _authClient;
        private readonly ILogger<TicketAssignedConsumer> _logger;

        public TicketAssignedConsumer(ILogger<TicketAssignedConsumer> logger, IAuthClient authClient)
        {
            _logger = logger;
            _authClient = authClient;
        }

        public async Task Consume(ConsumeContext<TicketAssignedEvent> context)
        {
            var user = await _authClient.GetUserByIdAsync(context.Message.TicketOwnerUserId);

            _logger.LogInformation("--- ATAMA BİLDİRİMİ ---");
            _logger.LogInformation("Sayın {Name}, '{Title}' başlıklı talebiniz {Staff} tarafından devralınmıştır.",
                user.FullName, context.Message.TicketTitle, context.Message.StaffFullName);

        }
    }
}
