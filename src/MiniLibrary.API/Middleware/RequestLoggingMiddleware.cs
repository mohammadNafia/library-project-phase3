using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MiniLibrary.API.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;


    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path;
        var time = DateTime.UtcNow;

    var userId = context.User?.FindFirst("UserId")?.Value;
        Console.WriteLine($"[{time}] Request to {path} by User ID: {userId ?? "Anonymous"}");
     context.Response.Headers["X-App-Name"] = "MiniLibrary";
        await _next(context);
    }






}