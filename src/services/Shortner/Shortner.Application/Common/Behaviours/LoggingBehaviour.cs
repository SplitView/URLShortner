using DnsClient.Internal;

using MediatR.Pipeline;

using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

namespace Shortner.Application.Common.Behaviours;

public class LoggingBehaviour<T>(ILogger<T> logger) : IRequestPreProcessor<T>
{
    public Task Process(T request, CancellationToken cancellationToken)
    {
            logger.LogInformation("Request: {0} {1}", typeof(T).Name, request);
            return Task.CompletedTask;
        }
}