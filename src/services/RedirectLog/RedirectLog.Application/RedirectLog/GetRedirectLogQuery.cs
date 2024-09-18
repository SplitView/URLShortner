using MediatR;
using Microsoft.EntityFrameworkCore;
using RedirectLog.Application.Common.Interface;
using RedirectLog.Exceptions;

namespace RedirectLog.Application.RedirectLog;

public class GetRedirectLogQuery(string uniqueKey) : IRequest<RedirectLogViewModel>
{
    public string UniqueKey { get; set; } = uniqueKey;
}

public class GetRedirectLogHandler(IRedirectLogContext redirectLogContext)
    : IRequestHandler<GetRedirectLogQuery, RedirectLogViewModel>
{
    public async Task<RedirectLogViewModel> Handle(GetRedirectLogQuery request, CancellationToken cancellationToken)
    {
            var customUrl = await redirectLogContext.CustomUrls
                                                     .Include(x => x.Redirections)
                                                     .FirstOrDefaultAsync(x => x.UniqueKey == request.UniqueKey, cancellationToken);
            if (customUrl == null)
            {
                throw new EntityNotFoundException($"Custom url with {request.UniqueKey} not found.");
            }

            RedirectLogViewModel result = new();
            foreach (var redirection in customUrl.Redirections)
            {
                result.TimeStamps.Add(redirection.TimeStamp);
            }

            return result;
        }
}