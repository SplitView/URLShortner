using MediatR;
using RedirectLog.Application.Common.Interface;
using RedirectLog.Domain.Entities;

namespace RedirectLog.Application.RedirectLog;

public class SaveRedirectLogCommand(string customUrlId, DateTime timeStamp) : IRequest<Unit>
{
    public string CustomUrlId { get; private set; } = customUrlId;
    public DateTime TimeStamp { get; private set; } = timeStamp;
}

public class SaveRedirectLogCommandHandler(IRedirectLogContext redirectLogContext)
    : IRequestHandler<SaveRedirectLogCommand, Unit>
{
    public async Task<Unit> Handle(SaveRedirectLogCommand request, CancellationToken cancellationToken)
    {
            var redirection = new Redirection
            {
                Id = Guid.NewGuid().ToString(),
                CustomUrlId = request.CustomUrlId,
                TimeStamp = request.TimeStamp
            };

            await redirectLogContext.Redirections.AddAsync(redirection, cancellationToken: cancellationToken);
            await redirectLogContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
}