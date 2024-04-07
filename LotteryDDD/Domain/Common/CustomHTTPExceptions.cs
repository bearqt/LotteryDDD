using System.Net;

namespace LotteryDDD.Domain.Common;

public class BadRequestException : CustomHTTPException
{
    public BadRequestException(string message, int? code = null) : base(message, code: code)
    {

    }
}

public class ConflictException : CustomHTTPException
{
    public ConflictException(string message, int? code = null) : base(message, code: code)
    {
    }
}

public class CustomHTTPException : System.Exception
{
    public CustomHTTPException(
        string message,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        int? code = null) : base(message)
    {
        StatusCode = statusCode;
        Code = code;
    }

    public CustomHTTPException(
        string message,
        System.Exception innerException,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        int? code = null) : base(message, innerException)
    {
        StatusCode = statusCode;
        Code = code;
    }

    public CustomHTTPException(
        HttpStatusCode statusCode = HttpStatusCode.BadRequest,
        int? code = null) : base()
    {
        StatusCode = statusCode;
        Code = code;
    }

    public HttpStatusCode StatusCode { get; }

    public int? Code { get; }
}