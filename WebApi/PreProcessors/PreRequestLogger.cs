namespace WebApi.PreProcessors
{
    using FastEndpoints;
    using FastEndpoints.Validation;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class PreRequestLogger<TRequest> : IPreProcessor<TRequest>
    {
        public Task PreProcessAsync(TRequest req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
        {
            var logger = ctx.RequestServices.GetRequiredService<ILogger<TRequest>>();

            logger.LogInformation($"request:{req?.GetType().FullName} path: {ctx.Request.Path}");

            return Task.CompletedTask;
        }
    }
}