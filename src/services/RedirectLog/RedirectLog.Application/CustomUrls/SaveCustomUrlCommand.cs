using MediatR;
using RedirectLog.Application.Common.Interface;
using RedirectLog.Domain.Entities;

namespace RedirectLog.Application.CustomUrls;

public class SaveCustomUrlCommand(string customUrlId, string originalURL, string uniqueKey, DateTime expiryDate)
    : IRequest<Unit>
{
    public string CustomUrlId { get; set; } = customUrlId;
    public string OriginalURL { get; set; } = originalURL;
    public string UniqueKey { get; set; } = uniqueKey;
    public DateTime ExpiryDate { get; set; } = expiryDate;
}

public class SaveCustomUrlCommandHandler(IRedirectLogContext redirectLogContext)
    : IRequestHandler<SaveCustomUrlCommand, Unit>
{
    public async Task<Unit> Handle(SaveCustomUrlCommand request, CancellationToken cancellationToken)
    {
        var customUrl = new CustomUrl
        {
            ExpiryDate = request.ExpiryDate,
            Id = request.CustomUrlId,
            OriginalURL = request.OriginalURL,
            UniqueKey = request.UniqueKey
        };

        await redirectLogContext.CustomUrls.AddAsync(customUrl, cancellationToken);
        await redirectLogContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}