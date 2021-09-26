using MassTransit;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

using URLShortner.Application.Common;
using URLShortner.Application.Common.Behaviours;
using URLShortner.Application.Events;

namespace URLShortner.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppConfig>(configuration.GetSection("AppConfig"));
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehaviour<,>));

            return services;
        }
    }
}
