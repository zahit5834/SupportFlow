using MassTransit;
using SupportFlow.Messaging.Events;
using SupportFlow.Notification.Api.Clients;

namespace SupportFlow.Notification.Api.Consumers
{
    public class TicketCreatedConsumer : IConsumer<TicketCreatedEvent>
    {
        private readonly IAuthClient _authClient;
        private readonly ICustomerClient _customerClient;
        private readonly ILogger<TicketCreatedConsumer> _logger;

        public TicketCreatedConsumer(ILogger<TicketCreatedConsumer> logger, IAuthClient authClient, ICustomerClient customerClient)
        {
            _logger = logger;
            _authClient = authClient;
            _customerClient = customerClient;
        }

        public async Task Consume(ConsumeContext<TicketCreatedEvent> context)
        {
            var user = await _authClient.GetUserByIdAsync(context.Message.CreatedByUserId);
            var company = await _customerClient.GetCompanyByIdAsync(context.Message.CompanyId);


            _logger.LogInformation("--- ZENGİNLEŞTİRİLMİŞ BİLDİRİM ---");
            _logger.LogInformation("Sayın {UserName}, {CompanyName} adına açtığınız '{Title}' başlıklı talebiniz alınmıştır.",
                user.FullName, company.Name, context.Message.Title);
            _logger.LogInformation("Bildirim gönderilecek adres: {Email}", user.Email);

        }
    }
}
