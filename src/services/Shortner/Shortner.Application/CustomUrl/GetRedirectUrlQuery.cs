using MassTransit;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using Shortner.Application.Exceptions;
using Shortner.Application.Interface;
using URLShortner.Common.Messages;

namespace Shortner.Application.CustomUrl;

public class GetRedirectUrlQuery(string uniqueKey) : IRequest<GetRedirectUrlViewModel>
{
    public string UniqueKey { get; set; } = uniqueKey;
}

public class GetRedirectUrlQueryHandler(
    IURLShortnerContext uRLShortnerContext,
    ICacheService cacheService,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<GetRedirectUrlQuery, GetRedirectUrlViewModel>
{
    public async Task<GetRedirectUrlViewModel> Handle(GetRedirectUrlQuery request, CancellationToken cancellationToken)
    {
        CustomUrlViewModel viewModel = null;

        var cachedValue = await cacheService.GetAsync<CustomUrlViewModel>(request.UniqueKey);
        if (cachedValue == null)
        {
            var customUrl = await uRLShortnerContext.CustomURLs.Find(x => x.UniqueKey == request.UniqueKey)
                .FirstOrDefaultAsync(cancellationToken);
            if (customUrl == null) throw new EntityNotFoundException($"Redirect url for {request.UniqueKey} not found");

            viewModel = CustomUrlViewModel.FromModel(customUrl);
            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(1))
                .SetSlidingExpiration(TimeSpan.FromHours(8));

            await cacheService.SetAsync(request.UniqueKey, viewModel, cacheOptions);
        }
        else
        {
            viewModel = cachedValue;
        }

        if (viewModel.ExpiryDate < DateTime.UtcNow)
        {
            await cacheService.ClearAsync(request.UniqueKey);
            throw new UrlExpiredException(request.UniqueKey);
        }

        await publishEndpoint.Publish(new UrlRedirectedEvent(viewModel.Id), cancellationToken);

        return new GetRedirectUrlViewModel(viewModel.OriginalURL);
    }
}