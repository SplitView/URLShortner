using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using URLShortner.Application.Common.Interface;
using URLShortner.Application.Events;
using URLShortner.Infrastructure.MongoDB;
using URLShortner.Infrastructure.RabbitMQ;
using URLShortner.Infrastructure.Redis;

namespace URLShortner.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //mongo
            services.AddSingleton<IURLShortnerContext, MongoDbContext>();
            services.Configure<MongoSettings>(configuration.GetSection("MongoSettings"));

            //cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetSection("Redis")["ConnectionString"];
            });
            services.AddSingleton<ICacheService, RedisCacheService>();

            //rabbitmq
            services.AddMassTransit(config =>
            {
                config.AddConsumer<UrlRedirectedEventHandler>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["RabbitMq:ConnectionString"]);

                    cfg.ReceiveEndpoint("url-redirected-queue", c =>
                    {
                        c.ConfigureConsumer<UrlRedirectedEventHandler>(ctx);
                    });
                });
            });
            services.AddMassTransitHostedService();
            services.AddScoped<IEventService, EventService>();

            return services;
        }
    }
}
