
using MassTransit;

using MediatR;
using System;
using System.Threading.Tasks;

using URLShortner.Application.CustomURL.ViewModels;
using URLShortner.Application.Redirection;

namespace URLShortner.Application.Events
{
    public class UrlRedirectedEvent
    {
        public Guid Id { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public string CustomUrlId { get; set; }
        public UrlRedirectedEvent()
        {
        }

        public UrlRedirectedEvent(CustomUrlViewModel customUrlViewModel)
        {
            Id = Guid.NewGuid();
            TimeStamp = DateTime.UtcNow;
            CustomUrlId = customUrlViewModel.Id;
        }
    }

    public class UrlRedirectedConsumer : IConsumer<UrlRedirectedEvent>
    {
        private readonly IMediator _mediator;

        public UrlRedirectedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<UrlRedirectedEvent> context)
        {
            await _mediator.Send(new RedirectionLogCommand(context.Message.CustomUrlId, context.Message.TimeStamp));
        }
    }
}
