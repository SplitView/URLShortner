using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shortner.Application.Interface;
using Shortner.Infrastructure.MongoDB;
using Shortner.Infrastructure.Redis;

namespace Shortner.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
            //mongo
            services.AddSingleton<IURLShortnerContext, MongoDbContext>();
            services.Configure<MongoSettings>(setting =>
            {
                configuration.GetSection("MongoSettings").Bind(setting);
            });

            //cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetSection("Redis")["ConnectionString"];
            });

            services.AddSingleton<ICacheService, RedisCacheService>();

            //rabbitmq
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(configuration["RabbitMq:ConnectionString"]);
                });
            });

            services.AddMassTransitHostedService();
            return services;
        }
}