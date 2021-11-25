using MassTransit;
using MediatR;
using RedirectLog.Application.CustomUrls;
using URLShortner.Common.Messages;

namespace RedirectLog.Application.Consumers
{
    public class CustomUrlCreatedConsumer : IConsumer<CustomUrlCreatedEvent>
    {
        private readonly IMediator _mediator;

        public CustomUrlCreatedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<CustomUrlCreatedEvent> context)
        {
            await _mediator.Send(new SaveCustomUrlCommand(context.Message.CustomUrlId, context.Message.OriginalURL, context.Message.UniqueKey, context.Message.ExpiryDate));
        }
    }
}
