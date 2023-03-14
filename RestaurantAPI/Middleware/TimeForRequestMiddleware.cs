using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RestaurantAPI.Middleware
{
    public class TimeForRequestMiddleware : IMiddleware
    {
        private readonly ILogger<TimeForRequestMiddleware> logger;
        private readonly Stopwatch stopwatch;

        public TimeForRequestMiddleware(ILogger<TimeForRequestMiddleware> logger)
        {
            this.logger = logger;
            stopwatch = Stopwatch.StartNew();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            stopwatch.Start();
            await next.Invoke(context);
            stopwatch.Stop();

            var elapsedMiliseconds = stopwatch.ElapsedMilliseconds;
            if(elapsedMiliseconds / 1000 > 4)
            {
                string messsage = $"Request [{context.Request.Method}] at {context.Request.Path} took {elapsedMiliseconds} ms";

                logger.LogInformation(messsage);
            }
        }
    }
}
