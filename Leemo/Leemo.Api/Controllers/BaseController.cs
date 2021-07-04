using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TPSS.Common;
using Leemo.Api.Filters;

namespace Leemo.Api.Controllers
{
    /// <summary>
    /// Created of applying exception filter, so inherit this basecontroller to have controller with loging in db.
    /// </summary>
    [ServiceFilter(typeof(ExceptionFilter))]
    public class BaseController : Controller
    {
        /// <summary>
        /// get current logged in user email
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public string getUserEmail()
        {
            var identity = (ClaimsIdentity)User.Identity;
            try
            {
                IEnumerable<Claim> claims = identity.Claims;
                var useremail = claims.Where(x => x.Type == Constants.JwtTokenClaimType_UserEmail).FirstOrDefault().Value;
                return useremail;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public Guid getCompanyLocationId()
        {
            var identity = (ClaimsIdentity)User.Identity;
            try
            {
                IEnumerable<Claim> claims = identity.Claims;
                var UserLocationID = claims.Where(x => x.Type == Constants.JwtTokenClaimType_UserLocationID).FirstOrDefault().Value;
                return Guid.Parse(UserLocationID);
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// get current logged in user data
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public string getUserData()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userdata = claims.Where(x => x.Type == Constants.JwtTokenClaimType_UserRespJson).FirstOrDefault().Value;
            return userdata;
        }
    }
}
