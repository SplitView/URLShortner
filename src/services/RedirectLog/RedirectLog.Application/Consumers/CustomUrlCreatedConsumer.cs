using MassTransit;
using MediatR;
using RedirectLog.Application.CustomUrls;
using URLShortner.Common.Messages;

namespace RedirectLog.Application.Consumers;

public class CustomUrlCreatedConsumer(IMediator mediator) : IConsumer<CustomUrlCreatedEvent>
{
    public async Task Consume(ConsumeContext<CustomUrlCreatedEvent> context)
    {
            await mediator.Send(new SaveCustomUrlCommand(context.Message.CustomUrlId, context.Message.OriginalURL, context.Message.UniqueKey, context.Message.ExpiryDate));
        }
}