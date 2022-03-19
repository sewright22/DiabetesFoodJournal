using FastEndpoints;
using FastEndpoints.Validation;

namespace WebApi.PostProcessors
{
    public class PostRequestLogger<TRequest, TResponse> : IPostProcessor<TRequest, TResponse>
    {
        public Task PostProcessAsync(TRequest req, TResponse res, HttpContext ctx, IReadOnlyCollection<ValidationFailure> failures, CancellationToken ct)
        {
            var logger = ctx.RequestServices.GetRequiredService<ILogger<TResponse>>();

            logger.LogInformation($"request:{res?.GetType().FullName} path: {ctx.Request.Path}");

            return Task.CompletedTask;
        }
    }
}