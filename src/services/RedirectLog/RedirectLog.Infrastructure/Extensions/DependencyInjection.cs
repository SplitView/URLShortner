using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using RedirectLog.Application.Common.Interface;
using RedirectLog.Application.Consumers;
using RedirectLog.Infrastructure.Persistence;

using URLShortner.Common;

namespace RedirectLog.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        builder.AddSqlServerDbContext<RedirectLogContext>("sqlDb");

        builder.Services.AddScoped<IRedirectLogContext>(provider => provider.GetRequiredService<RedirectLogContext>());

        //rabbitmq
        var rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMq");
        if (!string.IsNullOrEmpty(rabbitMqConnectionString))
        {
            builder.Services.AddMassTransit(config =>
            {
                config.AddConsumer<CustomUrlCreatedConsumer>();
                config.AddConsumer<UrlRedirectedConsumer>();

                config.SetKebabCaseEndpointNameFormatter();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri(rabbitMqConnectionString));

                    cfg.ReceiveEndpoint(Constants.QueueEndpoints.URL_REDIRECTED, c =>
                    {
                        c.UseMessageRetry(r => r.Interval(2, 100));
                        c.ConfigureConsumer<UrlRedirectedConsumer>(ctx);
                    });

                    cfg.ReceiveEndpoint(Constants.QueueEndpoints.CUSTOM_URL_CREATED, c =>
                    {
                        c.ConfigureConsumer<CustomUrlCreatedConsumer>(ctx);
                    });
                });
            });
        }

        return builder;
    }
}