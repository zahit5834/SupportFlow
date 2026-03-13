using MassTransit;
using SupportFlow.Messaging.Events;
using SupportFlow.Notification.Api.Clients;

namespace SupportFlow.Notification.Api.Consumers
{
    public class TicketCommentAddedConsumer : IConsumer<TicketCommentAddedEvent>
    {
        private readonly IAuthClient _authClient;
        private readonly ILogger<TicketCommentAddedConsumer> _logger;

        public TicketCommentAddedConsumer(IAuthClient authClient, ILogger<TicketCommentAddedConsumer> logger)
        {
            _authClient = authClient;
            _logger = logger;
        }


        public async Task Consume(ConsumeContext<TicketCommentAddedEvent> context)
        {
            var user = await _authClient.GetUserByIdAsync(context.Message.TicketOwnerUserId);

            _logger.LogInformation("--- YENİ YORUM BİLDİRİMİ ---");
            _logger.LogInformation("Gönderilen Mail: {Email}", user.Email);
            _logger.LogInformation("Mesaj: {Commenter} adlı kullanıcı '{Title}' talebinize cevap yazdı: {Msg}",
                context.Message.CommenterFullName, context.Message.TicketTitle, context.Message.Message);

        }
    }
}
