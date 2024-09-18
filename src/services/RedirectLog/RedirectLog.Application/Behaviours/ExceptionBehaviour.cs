using MediatR;

using Microsoft.Extensions.Logging;

namespace RedirectLog.Application;

public class ExceptionBehaviour<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Request: Exception occured and is unhandled {0} {1}", typeof(TRequest).Name, request);

            throw;
        }
    }
}