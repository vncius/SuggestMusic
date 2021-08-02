using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Net.Http;

namespace SuggestMusic.UnitTests.Helpers
{
    public static class HelperTests
    {
        public static IHttpClientFactory GetMockHttpClientFactory()
        {
            var mockFactory = new Mock<IHttpClientFactory>();
            var client = new HttpClient();

            mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

            return mockFactory.Object;
        }

        public static ILogger<T> GetMockLogger<T>()
        {
            ILoggerFactory factoryILogger = new NullLoggerFactory();
            return factoryILogger.CreateLogger<T>();
        }
    }
}
