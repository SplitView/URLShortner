using MediatR;
using Microsoft.AspNetCore.Mvc;
using RedirectLog.Application.RedirectLog;

namespace RedirectLog.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class RedirectLogController(IMediator mediator) : Controller
{
    /// <summary>
    /// Get the count and timestamps of rediecrion according to given key
    /// </summary>
    /// <param name="uniqueKey"></param>
    /// <returns></returns>
    [HttpGet("{uniqueKey}")]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<RedirectLogViewModel>> GetRedirections(string uniqueKey)
    {
        var result = await mediator.Send(new GetRedirectLogQuery(uniqueKey));
        return Ok(result);
    }
}