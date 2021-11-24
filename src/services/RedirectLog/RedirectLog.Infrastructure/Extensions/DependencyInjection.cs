using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedirectLog.Application.Common.Interface;
using RedirectLog.Infrastructure.Persistence;

namespace RedirectLog.Infrastructure.Extensions
{
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
            //services.AddMassTransit(config =>
            //{
            //    config.AddConsumer<UrlRedirectedConsumer>();
            //    config.UsingRabbitMq((ctx, cfg) =>
            //    {
            //        cfg.Host(configuration["RabbitMq:ConnectionString"]);

            //        cfg.ReceiveEndpoint("url-redirected-queue", c =>
            //        {
            //            c.Consumer<UrlRedirectedConsumer>(ctx);
            //        });
            //    });
            //});
            //services.AddMassTransitHostedService();
            //services.AddScoped<IEventService, EventService>();

            return services;
        }
    }
}
