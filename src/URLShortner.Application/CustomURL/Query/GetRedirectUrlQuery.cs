using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

using MongoDB.Driver;

using System;
using System.Threading;
using System.Threading.Tasks;

using URLShortner.Application.Common.Interface;
using URLShortner.Application.CustomURL.ViewModels;
using URLShortner.Application.Events;
using URLShortner.Application.Exceptions;

namespace URLShortner.Application.CustomURL.Query
{
    public class GetRedirectUrlQuery : IRequest<string>
    {
        public string UniqueKey { get; set; }

        public GetRedirectUrlQuery(string uniqueKey)
        {
            UniqueKey = uniqueKey;
        }
    }

    public class GetRedirectUrlQueryHandler : IRequestHandler<GetRedirectUrlQuery, string>
    {
        private readonly IURLShortnerContext _uRLShortnerContext;
        private readonly ICacheService _cacheService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEventService _eventService;

        public GetRedirectUrlQueryHandler(IURLShortnerContext uRLShortnerContext,
                                          ICacheService cacheService,
                                          IHttpContextAccessor httpContextAccessor,
                                          IEventService eventService)
        {
            _uRLShortnerContext = uRLShortnerContext;
            _cacheService = cacheService;
            _httpContextAccessor = httpContextAccessor;
            _eventService = eventService;
        }

        public async Task<string> Handle(GetRedirectUrlQuery request, CancellationToken cancellationToken)
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

                viewModel = CustomUrlViewModel.FromModel(customUrl, _httpContextAccessor);
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

            await _eventService.Publish(new UrlRedirectedEvent(viewModel));

            return viewModel.OriginalURL;
        }
    }


}
