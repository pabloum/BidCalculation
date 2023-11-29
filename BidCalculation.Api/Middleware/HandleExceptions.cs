using System.Net;
using System.Reflection;
using System.Text.Json;

namespace BidCalculation.Api.Middleware;

public class HandleExceptions
{
    private readonly RequestDelegate _next;
    private readonly Dictionary<Type, HttpStatusCode> _statusCode;

    public HandleExceptions(RequestDelegate next)
    {
        _next = next;
        _statusCode = new Dictionary<Type, HttpStatusCode>
        {
            { typeof(ApplicationException), HttpStatusCode.PreconditionFailed },
        };
    }

    public async Task Invoke(HttpContext context)
    {
        try 
        {
            await _next(context);
        } 
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex); 
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (ex is TargetInvocationException && ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            if (!_statusCode.TryGetValue(ex.GetType(), out var code))
            {
                code = HttpStatusCode.InternalServerError;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var error = ex.Message;
            var response = JsonSerializer.Serialize(error, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return context.Response.WriteAsync(response);
        }
}