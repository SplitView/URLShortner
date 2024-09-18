using MassTransit;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedirectLog.Application.Common.Interface;
using RedirectLog.Application.Consumers;
using RedirectLog.Infrastructure.Persistence;

using URLShortner.Common;

namespace RedirectLog.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<RedirectLogContext>(options =>
        {
            options.UseSqlServer(
                connectionString, x => x.MigrationsAssembly(typeof(RedirectLogContext).Assembly.FullName));
        });

        services.AddScoped<IRedirectLogContext>(provider => provider.GetRequiredService<RedirectLogContext>());

        //rabbitmq
        services.AddMassTransit(config =>
        {
            config.AddConsumer<CustomUrlCreatedConsumer>();
            config.AddConsumer<UrlRedirectedConsumer>();

            config.SetKebabCaseEndpointNameFormatter();

            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration["RabbitMq:ConnectionString"]);

                cfg.ReceiveEndpoint(Constants.QueueEndpoints.URL_REDIRECTED, c =>
                {
                    c.UseMessageRetry(r => r.Interval(2, 100));
                    c.ConfigureConsumer<UrlRedirectedConsumer>(ctx);
                });

                cfg.ReceiveEndpoint(Constants.QueueEndpoints.CUSTOM_URL_CREATED, c =>
                {
                    c.UseMessageRetry(r => r.Interval(2, 100));
                    c.ConfigureConsumer<CustomUrlCreatedConsumer>(ctx);
                });
            });
        });

        return services;
    }
}