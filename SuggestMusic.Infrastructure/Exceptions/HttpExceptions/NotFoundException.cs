using System.Net;

namespace SuggestMusic.Infrastructure.Exceptions.HttpExceptions
{
    public class NotFoundException : CustomHttpException
    {
        public NotFoundException(string message) : base(message) { }

        public override int StatusCode { get => (int)HttpStatusCode.NotFound; }
    }
}
