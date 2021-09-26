using MediatR;

using MongoDB.Driver;

using System.Threading;
using System.Threading.Tasks;

using URLShortner.Application.Common.Interface;
using URLShortner.Application.Exceptions;

namespace URLShortner.Application.Redirection
{
    public class GetRedirectionsQuery : IRequest<RedirectionViewModel>
    {
        public GetRedirectionsQuery(string uniqueKey)
        {
            UniqueKey = uniqueKey;
        }

        public string UniqueKey { get; set; }
    }

    public class GetRedirectionsQueryHandler : IRequestHandler<GetRedirectionsQuery, RedirectionViewModel>
    {
        private readonly IURLShortnerContext _uRLShortnerContext;

        public GetRedirectionsQueryHandler(IURLShortnerContext uRLShortnerContext)
        {
            _uRLShortnerContext = uRLShortnerContext;
        }

        public async Task<RedirectionViewModel> Handle(GetRedirectionsQuery request, CancellationToken cancellationToken)
        {
            var customUrl = await _uRLShortnerContext.CustomURLs
                                                     .Find(x => x.UniqueKey == request.UniqueKey)
                                                     .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (customUrl == null)
            {
                throw new EntityNotFoundException($"Custom url with {request.UniqueKey} not found.");
            }

            var urlRedirections = await _uRLShortnerContext.RedirectionLog
                                                           .Find(x => x.CustomUrlId == customUrl.Id)
                                                           .ToListAsync(cancellationToken: cancellationToken);
            RedirectionViewModel result = new RedirectionViewModel();
            foreach (var redirection in urlRedirections)
            {
                result.TimeStamps.Add(redirection.TimeStamp);
            }

            return result;
        }
    }
}
