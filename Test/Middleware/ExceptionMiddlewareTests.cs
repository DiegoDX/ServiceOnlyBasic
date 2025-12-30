using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using WebAPI.Middleware;

namespace Test.Middleware
{
    public class ExceptionMiddlewareTests
    {
        [Fact]
        public async Task Middleware_ShouldReturn500_OnUnhandledException()
        {
            var mockLogger = new Mock<ILogger<ExceptionMiddleware>>();
            var mockWebHostEnv = new Mock<IWebHostEnvironment>();

            var middleware =new ExceptionMiddleware(ctx =>
            {
                throw new Exception("Test exception");

            }, mockLogger.Object, mockWebHostEnv.Object);

            var context = new DefaultHttpContext();

            await middleware.Invoke(context);
            context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
