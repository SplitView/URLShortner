using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shortner.Application.CustomUrl;

namespace Shortner.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ShortnerController : Controller
    {
        private readonly IMediator _mediator;

        public ShortnerController(IMediator mediator)
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
    }
}
