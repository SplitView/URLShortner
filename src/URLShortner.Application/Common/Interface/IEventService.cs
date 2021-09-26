using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using URLShortner.Application.Events;

namespace URLShortner.Application.Common.Interface
{
    public interface IEventService
    {
        Task Publish<T>(T @event);
    }
}
