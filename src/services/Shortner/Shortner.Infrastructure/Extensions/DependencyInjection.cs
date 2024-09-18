using Elastic.Apm.Api;
using System.Configuration;

using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Shortner.Application.Interface;
using Shortner.Infrastructure.MongoDB;
using Shortner.Infrastructure.Redis;

using static MassTransit.Logging.DiagnosticHeaders;

namespace Shortner.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        //mongo
        builder.AddMongoDBClient("MongoConnection");
        builder.Services.AddSingleton<IURLShortnerContext, MongoDbContext>();
        builder.Services.Configure<MongoSettings>(setting => { configuration.GetSection("MongoSettings").Bind(setting); });

        //cache
        builder.AddRedisDistributedCache("redis");
        builder.Services.AddSingleton<ICacheService, RedisCacheService>();

        //rabbitmq
        var rabbitMqConnectionString = builder.Configuration.GetConnectionString("rabbitmq");
        if (!string.IsNullOrEmpty(rabbitMqConnectionString))
        {
            builder.Services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(new Uri(rabbitMqConnectionString), c => { });
                    cfg.ConfigureEndpoints(ctx);
                });
            });
        }
        
        return builder;
    }
}