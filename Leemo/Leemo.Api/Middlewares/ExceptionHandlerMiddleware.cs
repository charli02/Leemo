using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using TPSS.Common;

namespace Leemo.Api.Middlewares
{
    /// <summary>
    /// exception middleware
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// constructor for exception middleware
        /// </summary>
        /// <param name="next"></param>
        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke with context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = Constants.ContentType.ApplicationJson;
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new
            {
                Succeeded = false,
                ResponseCode = statusCode,
                Message = string.Concat(Constants.Messages.InternalServerError, " : ", exception.Message), //TODO: Display generic message when deploy Constants.Messages.InternalServerError,
                Data = string.Empty
            });
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(result);
        }

    }
}
