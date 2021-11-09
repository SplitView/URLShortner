using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using URLShortner.API.Controllers;
using URLShortner.Application.CustomURL.Query;
using Xunit;

namespace Test.URLShortner.API
{
    public class RedirectControllerTest
    {
        private const string OriginalUrl = "https://google.com/";

        [Fact]
        public void RedirectToOriginal_Success()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetRedirectUrlQuery>(), System.Threading.CancellationToken.None)).Returns(Task.FromResult(OriginalUrl));

            var redirectController = new RedirectController(mediatorMock.Object);

            var redirectResponse = (RedirectResult)redirectController.RedirectToOriginal(It.IsAny<string>()).Result;
            Assert.Equal(OriginalUrl, redirectResponse.Url);
            Assert.False(redirectResponse.Permanent);
            Assert.False(redirectResponse.PreserveMethod);
        }
    }
}
