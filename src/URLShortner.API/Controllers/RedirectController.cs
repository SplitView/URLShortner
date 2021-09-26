using MediatR;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using URLShortner.Application.CustomURL.Query;

namespace URLShortner.API.Controllers
{
    public class RedirectController : Controller
    {
        private readonly IMediator _mediator;

        public RedirectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //Just controller for redirecting. Not a api controller
        [HttpGet("/{uniquePath}")]
        public async Task<IActionResult> RedirectToOriginal(string uniquePath)
        {
            var route = await _mediator.Send(new GetRedirectUrlQuery(uniquePath));
            return Redirect(route);
        }
    }
}
