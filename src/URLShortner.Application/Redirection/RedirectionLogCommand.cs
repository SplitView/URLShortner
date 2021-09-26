using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

using URLShortner.Application.Common.Interface;

namespace URLShortner.Application.Redirection
{
    public class RedirectionLogCommand : IRequest<Unit>
    {
        public RedirectionLogCommand(string customUrlId, DateTime timeStamp)
        {
            CustomUrlId = customUrlId;
            TimeStamp = timeStamp;
        }

        public string CustomUrlId { get; private set; }
        public DateTime TimeStamp { get; private set; }
    }

    public class RedirectionLogCommandHandler : IRequestHandler<RedirectionLogCommand, Unit>
    {
        private readonly IURLShortnerContext _uRLShortnerContext;

        public RedirectionLogCommandHandler(IURLShortnerContext uRLShortnerContext)
        {
            _uRLShortnerContext = uRLShortnerContext;
        }

        public async Task<Unit> Handle(RedirectionLogCommand request, CancellationToken cancellationToken)
        {
            var redirection = new URLShortner.Domain.Entities.Redirection
            {
                CustomUrlId = request.CustomUrlId,
                TimeStamp = request.TimeStamp
            };

            await _uRLShortnerContext.RedirectionLog.InsertOneAsync(redirection, cancellationToken: cancellationToken);

            return Unit.Value;
        }
    }
}
