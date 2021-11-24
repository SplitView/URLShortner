using MediatR;
using Microsoft.EntityFrameworkCore;
using RedirectLog.Application.Common.Interface;
using RedirectLog.Exceptions;

namespace RedirectLog.Application.RedirectLog
{
    public class GetRedirectLogQuery : IRequest<RedirectLogViewModel>
    {
        public GetRedirectLogQuery(string uniqueKey)
        {
            UniqueKey = uniqueKey;
        }

        public string UniqueKey { get; set; }
    }

    public class GetRedirectLogHandler : IRequestHandler<GetRedirectLogQuery, RedirectLogViewModel>
    {
        private readonly IRedirectLogContext _redirectLogContext;

        public GetRedirectLogHandler(IRedirectLogContext redirectLogContext)
        {
            _redirectLogContext = redirectLogContext;
        }

        public async Task<RedirectLogViewModel> Handle(GetRedirectLogQuery request, CancellationToken cancellationToken)
        {
            var customUrl = await _redirectLogContext.CustomUrls
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
}
