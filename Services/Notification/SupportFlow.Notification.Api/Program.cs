using MassTransit;
using Refit;
using SupportFlow.Notification.Api.Clients;
using SupportFlow.Notification.Api.Consumers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TicketCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h => {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddRefitClient<IAuthClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:6001"));

builder.Services.AddRefitClient<ICustomerClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:6101"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
