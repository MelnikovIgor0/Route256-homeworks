using Route256.Week5.Homework.PriceCalculator.Bll.Exceptions;

namespace Route256.Week5.Homework.PriceCalculator.Api.Middleware
{
    /// <summary>
    /// Обрабочик ошибок на уровне Middleware
    /// </summary>
    public class ExceptionHandlerMiddleware
    {

        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var response = context.Response;
                if (exception is OneOrManyCalculationsBelongsToAnotherUserException)
                    response.StatusCode = 403;
                else if (exception is OneOrManyCalculationsNotFoundException)
                    response.StatusCode = 400;
                else
                    response.StatusCode = 500;

                context.Response.ContentType = "text/plain";
                await response.WriteAsync($"{exception.GetType().FullName} {exception.Message}");
            }
        }
    }
}
