using System;

namespace URLShortner.Application.Events
{
    public abstract class EventBase
    {
        public EventBase()
        {
            Id = Guid.NewGuid();
            TimeStamp = DateTime.UtcNow;
        }

        public Guid Id { get; private set; }

        public DateTime TimeStamp { get; private set; }
    }
}
