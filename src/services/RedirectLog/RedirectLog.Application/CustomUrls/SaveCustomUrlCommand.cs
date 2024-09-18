using MediatR;
using RedirectLog.Application.Common.Interface;

namespace RedirectLog.Application.CustomUrls;

public class SaveCustomUrlCommand : IRequest<Unit>
{
    public SaveCustomUrlCommand(string customUrlId, string originalURL, string uniqueKey, DateTime expiryDate)
    {
            CustomUrlId = customUrlId;
            OriginalURL = originalURL;
            UniqueKey = uniqueKey;
            ExpiryDate = expiryDate;
        }

    public string CustomUrlId { get; set; }
    public string OriginalURL { get; set; }
    public string UniqueKey { get; set; }
    public DateTime ExpiryDate { get; set; }
}

public class SaveCustomUrlCommandHandler : IRequestHandler<SaveCustomUrlCommand, Unit>
{

    private readonly IRedirectLogContext _redirectLogContext;

    public SaveCustomUrlCommandHandler(IRedirectLogContext redirectLogContext)
    {
            _redirectLogContext = redirectLogContext;
        }

    public async Task<Unit> Handle(SaveCustomUrlCommand request, CancellationToken cancellationToken)
    {
            var customUrl = new Domain.Entities.CustomUrl
            {
                ExpiryDate = request.ExpiryDate,
                Id = request.CustomUrlId,
                OriginalURL = request.OriginalURL,
                UniqueKey = request.UniqueKey
            };

            await _redirectLogContext.CustomUrls.AddAsync(customUrl,cancellationToken);
            await _redirectLogContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
}