using MassTransit;

using System.Threading.Tasks;

using URLShortner.Application.Common.Interface;
using URLShortner.Application.Events;

namespace URLShortner.Infrastructure.RabbitMQ
{
    public class EventService : IEventService
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Publish<T>(T @event)
        {
            await _publishEndpoint.Publish(@event);
        }
    }
}
