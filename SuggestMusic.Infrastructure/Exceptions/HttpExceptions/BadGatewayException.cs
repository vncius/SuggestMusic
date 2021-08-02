using System.Net;

namespace SuggestMusic.Infrastructure.Exceptions.HttpExceptions
{
    public class BadGatewayException : CustomHttpException
    {
        public BadGatewayException(string message) : base(message) { }

        public override int StatusCode { get => (int)HttpStatusCode.BadGateway; }
    }
}
