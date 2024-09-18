using MassTransit;
using MediatR;
using RedirectLog.Application.RedirectLog;
using URLShortner.Common.Messages;

namespace RedirectLog.Application.Consumers;

public class UrlRedirectedConsumer(IMediator mediator) : IConsumer<UrlRedirectedEvent>
{
    public async Task Consume(ConsumeContext<UrlRedirectedEvent> context)
    {
            await mediator.Send(new SaveRedirectLogCommand(context.Message.CustomUrlId, context.Message.TimeStamp));
        }
}