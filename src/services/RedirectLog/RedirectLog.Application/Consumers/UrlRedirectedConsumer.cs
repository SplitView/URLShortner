using MassTransit;
using MediatR;
using RedirectLog.Application.RedirectLog;
using URLShortner.Common.Messages;

namespace RedirectLog.Application.Consumers;

public class UrlRedirectedConsumer : IConsumer<UrlRedirectedEvent>
{
    private readonly IMediator _mediator;

    public UrlRedirectedConsumer(IMediator mediator)
    {
            _mediator = mediator;
        }

    public async Task Consume(ConsumeContext<UrlRedirectedEvent> context)
    {
            await _mediator.Send(new SaveRedirectLogCommand(context.Message.CustomUrlId, context.Message.TimeStamp));
        }
}