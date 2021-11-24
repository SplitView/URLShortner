//using MediatR;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using System;
//using System.Threading.Tasks;
//using URLShortner.API.Controllers;
//using URLShortner.Application.CustomURL.Command;
//using URLShortner.Application.CustomURL.ViewModels;
//using URLShortner.Application.Redirection;
//using Xunit;

//namespace Test.URLShortner.API
//{
//    public class UrlShortnerControllerTest
//    {
//        [Fact]
//        public void GenerateUrl_Success()
//        {
//            CustomUrlViewModel expectedObject = new()
//            {
//                ExpiryDate = new DateTime(2020, 12, 12),
//                Id = Guid.NewGuid().ToString(),
//                OriginalURL = "https://google.com",
//                RedirectURL = "https://localhost:5000/asdfgh",
//                UniqueKey = "asdfgh"
//            };

//            var mediatorMock = new Mock<IMediator>();
//            mediatorMock.Setup(x => x.Send(It.IsAny<GenerateCommand>(), System.Threading.CancellationToken.None)).Returns(Task.FromResult(expectedObject));

//            var urlShortnerController = new UrlShortnerController(mediatorMock.Object);

//            var response = urlShortnerController.GenerateUrl(It.IsAny<GenerateCommand>()).Result;
//            var objectResult = response.Result as OkObjectResult;
//            Assert.NotNull(objectResult);

//            Assert.Equal(200, objectResult?.StatusCode);
//            Assert.Equal(expectedObject, objectResult?.Value);
//        }


//        [Fact]
//        public void GetRedirections_Success()
//        {
//            RedirectionViewModel expectedObject = new()
//            {
//                TimeStamps = new System.Collections.Generic.List<DateTime> {
//                    new DateTime(2020, 12, 12),
//                    new DateTime(2020, 12, 12)
//                }
//            };

//            var mediatorMock = new Mock<IMediator>();
//            mediatorMock.Setup(x => x.Send(It.IsAny<GetRedirectionsQuery>(), System.Threading.CancellationToken.None)).Returns(Task.FromResult(expectedObject));

//            var urlShortnerController = new UrlShortnerController(mediatorMock.Object);

//            var response = urlShortnerController.GetRedirections(It.IsAny<string>()).Result;
//            var objectResult = response.Result as OkObjectResult;
//            Assert.NotNull(objectResult);

//            Assert.Equal(200, objectResult?.StatusCode);
//            Assert.Equal(expectedObject, objectResult?.Value);
//        }
//    }
//}
