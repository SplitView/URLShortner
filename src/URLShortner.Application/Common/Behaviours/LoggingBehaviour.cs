using DnsClient.Internal;

using MediatR.Pipeline;

using Microsoft.Extensions.Logging;

using System.Threading;
using System.Threading.Tasks;

namespace URLShortner.Application.Common.Behaviours
{
    public class LoggingBehaviour<T> : IRequestPreProcessor<T>
    {
        private readonly ILogger<T> _logger;

        public LoggingBehaviour(ILogger<T> logger)
        {
            _logger = logger;
        }

        public Task Process(T request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Request: {0} {1}", typeof(T).Name, request);
            return Task.CompletedTask;
        }
    }
}
