using MediatR;
using RedirectLog.Application.Common.Interface;
using RedirectLog.Domain.Entities;

namespace RedirectLog.Application.RedirectLog;

public class SaveRedirectLogCommand : IRequest<Unit>
{
    public SaveRedirectLogCommand(string customUrlId, DateTime timeStamp)
    {
            CustomUrlId = customUrlId;
            TimeStamp = timeStamp;
        }

    public string CustomUrlId { get; private set; }
    public DateTime TimeStamp { get; private set; }
}

public class SaveRedirectLogCommandHandler : IRequestHandler<SaveRedirectLogCommand, Unit>
{
    private readonly IRedirectLogContext _redirectLogContext;

    public SaveRedirectLogCommandHandler(IRedirectLogContext redirectLogContext)
    {
            _redirectLogContext = redirectLogContext;
        }

    public async Task<Unit> Handle(SaveRedirectLogCommand request, CancellationToken cancellationToken)
    {
            var redirection = new Redirection
            {
                Id = Guid.NewGuid().ToString(),
                CustomUrlId = request.CustomUrlId,
                TimeStamp = request.TimeStamp
            };

            await _redirectLogContext.Redirections.AddAsync(redirection, cancellationToken: cancellationToken);
            await _redirectLogContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
}