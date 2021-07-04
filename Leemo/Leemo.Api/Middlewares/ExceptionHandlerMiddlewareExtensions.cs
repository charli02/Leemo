using Microsoft.AspNetCore.Builder;

namespace Leemo.Api.Middlewares
{
    /// <summary>
    /// Exception handler middleware extension
    /// </summary>
    public static class ExceptionHandlerMiddlewareExtensions
    {
        /// <summary>
        /// Exception middleware extension
        /// </summary>
        /// <param name="app"></param>
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
