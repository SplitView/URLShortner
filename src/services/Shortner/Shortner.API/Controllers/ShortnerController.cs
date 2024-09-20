using Asp.Versioning;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Shortner.Application.CustomUrl;

namespace Shortner.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ShortnerController(IMediator mediator) : ControllerBase
{
    /// <summary>
    ///     Generate the short url for an original url
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("generate")]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<CustomUrlViewModel>> GenerateUrl([FromBody] GenerateCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    ///     Gets the redirection model according to unique key
    /// </summary>
    /// <param name="uniqueKey"></param>
    /// <returns></returns>
    [HttpGet("{uniqueKey}")]
    [ProducesDefaultResponseType(typeof(GetRedirectUrlViewModel))]
    public async Task<ActionResult<GetRedirectUrlViewModel>> GetRedirectUrl([FromRoute] string uniqueKey)
    {
        return Ok(await mediator.Send(new GetRedirectUrlQuery(uniqueKey)));
    }
}