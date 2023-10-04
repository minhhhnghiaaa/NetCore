using System.Net;

namespace NetCore.Api.Common;

public class CustomErrorResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string ErrorMessage { get; set; }

    public CustomErrorResponse(HttpStatusCode statusCode, string errorMessage)
    {
        StatusCode = statusCode;
        ErrorMessage = errorMessage;
    }
}