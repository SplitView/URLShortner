using MediatR;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using MongoDB.Driver;

using System;
using System.Threading;
using System.Threading.Tasks;

using URLShortner.Application.Common;
using URLShortner.Application.Common.Interface;
using URLShortner.Application.CustomURL.ViewModels;
using URLShortner.Application.Exceptions;
using URLShortner.Application.Helpers;

namespace URLShortner.Application.CustomURL.Command
{
    public class GenerateCommand : IRequest<CustomUrlViewModel>
    {
        public string OriginalURL { get; set; }
        public bool UseCustomKey { get; set; }
        public string CustomUniqueKey { get; set; }
        public bool UseCustomExpiry { get; set; }
        public int ExpiryTimeInSeconds { get; set; }
    }

    public class GenerateCommandHandler : IRequestHandler<GenerateCommand, CustomUrlViewModel>
    {
        private readonly IURLShortnerContext _uRLShortnerContext;
        private readonly IOptions<AppConfig> _appConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GenerateCommandHandler(IURLShortnerContext uRLShortnerContext,
                                      IOptions<AppConfig> appConfig,
                                      IHttpContextAccessor httpContextAccessor)
        {
            _uRLShortnerContext = uRLShortnerContext;
            _appConfig = appConfig;
            _httpContextAccessor = httpContextAccessor;
        }

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

            await _uRLShortnerContext.CustomURLs.InsertOneAsync(customUrl, cancellationToken: cancellationToken);

            return CustomUrlViewModel.FromModel(customUrl, _httpContextAccessor);
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
                return _appConfig.Value.ExpiryTimeInSeconds;
            }
        }

        private async Task<string> GenerateAndGetUniqueKeyAsync(CancellationToken cancellationToken)
        {
            string uniqueKey = string.Empty;

            bool hasDuplicateKey = true;
            while (hasDuplicateKey)
            {
                uniqueKey = UrlShortnerHelper.GetUniqueKey();

                hasDuplicateKey = await _uRLShortnerContext.CustomURLs
                                                           .Find(x => x.UniqueKey == uniqueKey)
                                                           .AnyAsync(cancellationToken);
            }

            return uniqueKey;
        }

        private async Task GuardDuplicateCustomKey(GenerateCommand request, CancellationToken cancellationToken)
        {
            var hasDuplicateUniqueKey = await _uRLShortnerContext.CustomURLs.Find(x => x.UniqueKey == request.CustomUniqueKey).AnyAsync(cancellationToken: cancellationToken);
            if (hasDuplicateUniqueKey)
            {
                throw new DuplicateCustomKeyException(request.CustomUniqueKey);
            }
        }
    }
}
