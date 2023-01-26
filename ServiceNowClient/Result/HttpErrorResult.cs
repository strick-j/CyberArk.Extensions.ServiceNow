using System.Net;

namespace CyberArk.Extensions.ServiceNow
{
    public class HttpErrorResult<T> : ErrorResult<T>
    {
        public HttpStatusCode StatusCode { get; }

        public HttpErrorResult(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpErrorResult(string message, IReadOnlyCollection<Error> errors, HttpStatusCode statusCode) : base(message, errors)
        {
            StatusCode = statusCode;
        }
    }
}