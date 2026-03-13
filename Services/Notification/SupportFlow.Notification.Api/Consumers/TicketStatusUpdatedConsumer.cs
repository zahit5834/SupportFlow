using MassTransit;
using SupportFlow.Messaging.Events;
using SupportFlow.Notification.Api.Clients;

namespace SupportFlow.Notification.Api.Consumers
{
    public class TicketStatusUpdatedConsumer : IConsumer<TicketStatusUpdatedEvent>
    {

        private readonly IAuthClient _authClient;
        private readonly ILogger<TicketStatusUpdatedConsumer> _logger;

        public TicketStatusUpdatedConsumer(IAuthClient authClient, ILogger<TicketStatusUpdatedConsumer> logger)
        {
            _authClient = authClient;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<TicketStatusUpdatedEvent> context)
        {
            var user = await _authClient.GetUserByIdAsync(context.Message.CreatedByUserId);

            _logger.LogInformation("--- BAĞIMSIZ BİLDİRİM ---");
            _logger.LogInformation("Sayın {Name}, '{Title}' başlıklı talebinizin durumu güncellendi. Yeni Durum: {Status}",
                user.FullName, context.Message.Title, context.Message.NewStatus);
        }
    }
}
