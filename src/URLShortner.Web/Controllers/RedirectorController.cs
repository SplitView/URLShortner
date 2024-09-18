using Microsoft.AspNetCore.Mvc;
using URLShortner.Web.Services;

namespace URLShortner.Web.Controllers;

[Route("/")]
public class RedirectorController : ControllerBase
{
    private readonly IApiService _apiService;

    public RedirectorController(IApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet("{uniqueKey}")]
    public async Task<IActionResult> RedirectWithKey([FromRoute] string uniqueKey)
    {
        var redirectUrl = await _apiService.GetRedirectUrlAsync(uniqueKey);
        return Redirect(redirectUrl.OriginalUrl);
    }
}