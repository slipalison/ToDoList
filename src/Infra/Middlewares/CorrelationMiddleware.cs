using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.Middlewares;

public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var correlationContext = httpContext.RequestServices.GetRequiredService<ICorrelationContextService>();
        var crId = httpContext.Request.Headers.TryGetValue("X-Correlation-ID", out var value);
        correlationContext.SetCorrelationId(crId && Guid.TryParse(value.FirstOrDefault(), out var correlationGuid) ? correlationGuid : Guid.NewGuid() );
        
        if(!crId)
            httpContext.Request.Headers.Add("X-Correlation-ID", correlationContext.GetCorrelationId().ToString());

        await _next(httpContext);
    }
}