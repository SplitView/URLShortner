using System.Reflection;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Shortner.Application.Common.Behaviours;

using URLShortner.Application.Common;

namespace Shortner.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppConfig>(setting => { configuration.GetSection("AppConfig").Bind(setting); });

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehaviour<,>));

        return services;
    }
}