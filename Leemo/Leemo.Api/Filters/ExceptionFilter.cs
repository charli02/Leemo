using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using TPSS.Common;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Repository.Interface;

namespace Leemo.Api.Filters
{
    /// <summary>
    /// Exception filter
    /// </summary>
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILogReposiory _logReposiory;

        /// <summary>
        /// log repository costructor
        /// </summary>
        /// <param name="logReposiory"></param>
        public ExceptionFilter(ILogReposiory logReposiory)
        {
            _logReposiory = logReposiory;
        }

        /// <summary>
        /// It will log the exception in database.
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            var identity = (ClaimsIdentity)context.HttpContext.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string userEmail = "";
            if (claims.Count() > 0)
                userEmail = claims.Where(x => x.Type == Constants.JwtTokenClaimType_UserEmail).FirstOrDefault().Value;
            //string userEmail = claims.Where(x => x.Type == Constants.JwtTokenClaimType_UserEmail).FirstOrDefault().Value;
            ErrorLog log = new ErrorLog
            {
                TimeStamp = DateTime.UtcNow,
                ActionDescriptor = context.ActionDescriptor.DisplayName,
                //IpAddress = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                IpAddress = CommonFunction.GetIpAddress(),
                Message = context.Exception.Message,
                RequestId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier,
                RequestPath = context.HttpContext.Request.Path,
                Source = context.Exception.Source,
                StackTrace = context.Exception.StackTrace,
                Type = context.Exception.GetType().ToString(),
                //User = context.HttpContext.User.Identity.Name,
                User = userEmail,
                LogType = Constants.LogType.Exception,
                ProjectSource = Constants.ProjectSourceAPI
            };
            _logReposiory.InsertLog(log);
        }
    }
}
    