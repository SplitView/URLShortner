using MediatR;

using Microsoft.Extensions.Logging;

namespace RedirectLog.Application
{
    public class ExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public ExceptionBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request: Exception occured and is unhandled {0} {1}", typeof(TRequest).Name, request);

                throw;
            }
        }
    }
}