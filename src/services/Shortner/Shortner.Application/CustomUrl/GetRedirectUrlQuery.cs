using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using Shortner.Application.Exceptions;
using Shortner.Application.Interface;
using URLShortner.Common.Messages;

namespace Shortner.Application.CustomUrl
{
    public class GetRedirectUrlQuery : IRequest<GetRedirectUrlViewModel>
    {
        public string UniqueKey { get; set; }

        public GetRedirectUrlQuery(string uniqueKey)
        {
            UniqueKey = uniqueKey;
        }
    }

    public class GetRedirectUrlQueryHandler : IRequestHandler<GetRedirectUrlQuery, GetRedirectUrlViewModel>
    {
        private readonly IURLShortnerContext _uRLShortnerContext;
        private readonly ICacheService _cacheService;
        private readonly IPublishEndpoint _publishEndpoint;

        public GetRedirectUrlQueryHandler(IURLShortnerContext uRLShortnerContext,
                                          ICacheService cacheService,
                                          IPublishEndpoint publishEndpoint)
        {
            _uRLShortnerContext = uRLShortnerContext;
            _cacheService = cacheService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<GetRedirectUrlViewModel> Handle(GetRedirectUrlQuery request, CancellationToken cancellationToken)
        {
            CustomUrlViewModel viewModel = null;

            var cachedValue = await _cacheService.GetAsync<CustomUrlViewModel>(request.UniqueKey);
            if (cachedValue == null)
            {
                var customUrl = await _uRLShortnerContext.CustomURLs.Find(x => x.UniqueKey == request.UniqueKey).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                if (customUrl == null)
                {
                    throw new EntityNotFoundException($"Redirect url for {request.UniqueKey} not found");
                }

                viewModel = CustomUrlViewModel.FromModel(customUrl);
                var cacheOptions = new DistributedCacheEntryOptions()
                                        .SetAbsoluteExpiration(TimeSpan.FromDays(1))
                                        .SetSlidingExpiration(TimeSpan.FromHours(8));

                await _cacheService.SetAsync(request.UniqueKey, viewModel, cacheOptions);
            }
            else
            {
                viewModel = cachedValue;
            }

            if (viewModel.ExpiryDate < DateTime.UtcNow)
            {
                await _cacheService.ClearAsync(request.UniqueKey);
                throw new UrlExpiredException(request.UniqueKey);
            }

            await _publishEndpoint.Publish(new UrlRedirectedEvent(viewModel.Id), cancellationToken);

            return new GetRedirectUrlViewModel(viewModel.OriginalURL);
        }
    }
}