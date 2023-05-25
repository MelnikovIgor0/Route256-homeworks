using System.Net;
using System.Text;

namespace Route256.Week1.Homework.PriceCalculator.Api.Middlewaries;

internal sealed class LogRequestOfTotalPriceOfGoodMiddleware
{
    private readonly RequestDelegate _next;
    private ILogger<LogRequestOfTotalPriceOfGoodMiddleware> _logger;

    public LogRequestOfTotalPriceOfGoodMiddleware(RequestDelegate next,
        ILogger<LogRequestOfTotalPriceOfGoodMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    private string BaseUrl(HttpRequest req)
    {
        var uriBuilder = new UriBuilder(req.Scheme, req.Host.Host, req.Host.Port ?? -1);
        if (uriBuilder.Uri.IsDefaultPort)
        {
            uriBuilder.Port = -1;
        }
        return uriBuilder.Uri.AbsoluteUri;
    }

    private string HeadersToString(IHeaderDictionary headers)
    {
        StringBuilder answer = new StringBuilder($"{{{Environment.NewLine}");
        foreach (var header in headers)
        {
            answer.Append("\t\t" + header.Key + ": " + header.Value + Environment.NewLine);
        }
        return answer.ToString()[0..(answer.ToString().Length - 1)] + "}";
    }

    private string GetRequestBody(HttpContext context)
    {
        if (context.Request.Body.Length == 0)
        {
            return "{}";
        }
        context.Request.Body.Position = 0;
        return new StreamReader(context.Request.Body).ReadToEnd();
    }

    private async Task<string> GetResponseBody(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        try
        {
            var memoryBodyStream = new MemoryStream();
            context.Response.Body = memoryBodyStream;

            await _next.Invoke(context);

            memoryBodyStream.Seek(0, SeekOrigin.Begin);
            string body = await new StreamReader(memoryBodyStream).ReadToEndAsync();
            memoryBodyStream.Seek(0, SeekOrigin.Begin);
            await memoryBodyStream.CopyToAsync(originalBodyStream);
            return body;
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.ToString().Split('/').Last<string>() == "calculate-total-price")
        {
            _logger.LogInformation(
                $"{Environment.NewLine}Request:{Environment.NewLine}\tTimestamp: "
                + DateTime.UtcNow + $"{Environment.NewLine}\t" +
                "Url: " + BaseUrl(context.Request) +
                context.Request.Path.ToString()[1..(context.Request.Path.ToString().Length - 1)] +
                $"{Environment.NewLine}\tHeaders: " +
                HeadersToString(context.Request.Headers) + $"{Environment.NewLine}\t" +
                "Body: " + GetRequestBody(context) + $"{Environment.NewLine}Response:{Environment.NewLine}\tBody: " +
                GetResponseBody(context).Result
                );
            _next.Invoke(context);
        }
    }
}