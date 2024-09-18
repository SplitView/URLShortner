using MassTransit;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shortner.Application.Exceptions;
using Shortner.Application.Helpers;
using Shortner.Application.Interface;
using URLShortner.Application.Common;
using URLShortner.Common.Messages;

namespace Shortner.Application.CustomUrl;

public class GenerateCommand : IRequest<CustomUrlViewModel>
{
    public string OriginalURL { get; set; }
    public bool UseCustomKey { get; set; }
    public string CustomUniqueKey { get; set; }
    public bool UseCustomExpiry { get; set; }
    public int ExpiryTimeInSeconds { get; set; }
}

public class GenerateCommandHandler(
    IURLShortnerContext uRLShortnerContext,
    IOptions<AppConfig> appConfig,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<GenerateCommand, CustomUrlViewModel>
{
    public async Task<CustomUrlViewModel> Handle(GenerateCommand request, CancellationToken cancellationToken)
    {
        var uniqueKey = await GetUniqueKey(request, cancellationToken);
        var expiryTimeInSeconds = GetExpiryTime(request);

        var customUrl = new Domain.Entities.CustomURL()
        {
            UniqueKey = uniqueKey,
            ExpiryDate = DateTime.UtcNow.AddSeconds(expiryTimeInSeconds),
            OriginalURL = request.OriginalURL
        };

        await uRLShortnerContext.CustomURLs.InsertOneAsync(customUrl, cancellationToken: cancellationToken);

        await publishEndpoint.Publish(new CustomUrlCreatedEvent(customUrl.Id, customUrl.OriginalURL, customUrl.UniqueKey, customUrl.ExpiryDate), cancellationToken);

        return CustomUrlViewModel.FromModel(customUrl);
    }

    private async Task<string> GetUniqueKey(GenerateCommand request, CancellationToken cancellationToken)
    {
        if (request.UseCustomKey && !string.IsNullOrEmpty(request.CustomUniqueKey))
        {
            await GuardDuplicateCustomKey(request, cancellationToken);
            return request.CustomUniqueKey;
        }
        else
        {
            return await GenerateAndGetUniqueKeyAsync(cancellationToken);
        }
    }

    private int GetExpiryTime(GenerateCommand request)
    {
        if (request.UseCustomExpiry && request.ExpiryTimeInSeconds > 0)
        {
            return request.ExpiryTimeInSeconds;
        }
        else
        {
            return appConfig.Value.ExpiryTimeInSeconds;
        }
    }

    private async Task<string> GenerateAndGetUniqueKeyAsync(CancellationToken cancellationToken)
    {
        string uniqueKey = string.Empty;

        bool hasDuplicateKey = true;
        while (hasDuplicateKey)
        {
            uniqueKey = UrlShortnerHelper.GetUniqueKey();

            hasDuplicateKey = await uRLShortnerContext.CustomURLs
                .Find(x => x.UniqueKey == uniqueKey)
                .AnyAsync(cancellationToken);
        }

        return uniqueKey;
    }

    private async Task GuardDuplicateCustomKey(GenerateCommand request, CancellationToken cancellationToken)
    {
        var hasDuplicateUniqueKey = await uRLShortnerContext.CustomURLs.Find(x => x.UniqueKey == request.CustomUniqueKey).AnyAsync(cancellationToken: cancellationToken);
        if (hasDuplicateUniqueKey)
        {
            throw new DuplicateCustomKeyException(request.CustomUniqueKey);
        }
    }
}