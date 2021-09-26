using MediatR;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using URLShortner.Application.CustomURL.Command;
using URLShortner.Application.CustomURL.ViewModels;
using URLShortner.Application.Redirection;

namespace URLShortner.API.Controllers
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UrlShortnerController : Controller
    {
        private readonly IMediator _mediator;

        public UrlShortnerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Generate the short url for an original url
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<CustomUrlViewModel>> GenerateUrl(GenerateCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Get the count and timestamps of rediecrion according to given key
        /// </summary>
        /// <param name="uniqueKey"></param>
        /// <returns></returns>
        [HttpGet("{uniqueKey}")]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<RedirectionViewModel>> GetRedirections(string uniqueKey)
        {
            var result = await _mediator.Send(new GetRedirectionsQuery(uniqueKey));
            return Ok(result);
        }
    }
}
