
using MassTransit;

using MediatR;

using System.Threading.Tasks;

using URLShortner.Application.CustomURL.ViewModels;
using URLShortner.Application.Redirection;

namespace URLShortner.Application.Events
{
    public class UrlRedirectedEvent : EventBase
    {
        public string CustomUrlId { get; set; }
        public UrlRedirectedEvent()
        {

        }

        public UrlRedirectedEvent(CustomUrlViewModel customUrlViewModel) : base()
        {
            CustomUrlId = customUrlViewModel.Id;
        }
    }

    public class UrlRedirectedEventHandler : IConsumer<UrlRedirectedEvent>
    {
        private readonly IMediator _mediator;

        public UrlRedirectedEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<UrlRedirectedEvent> context)
        {
            await _mediator.Send(new RedirectionLogCommand(context.Message.CustomUrlId, context.Message.TimeStamp));
        }
    }
}
