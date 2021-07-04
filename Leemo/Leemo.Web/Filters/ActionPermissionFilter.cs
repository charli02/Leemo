using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TPSS.Common;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;
using Leemo.Service.Interface;
using Leemo.Web.HttpClient;
using Leemo.Model.InputModels;

namespace Leemo.Web.Filters
{
    public class ActionPermissionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// permission data send from controller inicluding PermissionName and access requested.
        /// </summary>
        public readonly string _apiBaseUrl = string.Empty;
        /// <summary>
        ///
        /// </summary>

        public ActionPermissionFilter()
        {
            #region get appSetting Values without Parameters of Constructors.
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            _apiBaseUrl = root.GetSection("AppSettings").GetSection("Leemo_API_Config:BaseUrl").Value;
            var appSetting = root.GetSection("AppSettings");
            #endregion
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context,
                                        ActionExecutionDelegate next)
        {
            bool valid = true;
            try
            {
                var identity = context.HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    if (claims != null && claims.Count() > 0)
                    {
                        string Auth_Role = context.HttpContext.Session.GetString("Auth_Role");
                        string userID = context.HttpContext.Session.GetString("_ID");
                        string companyLocationID = context.HttpContext.Session.GetString("CompanyLocationID");
                        if (userID == null || userID == string.Empty)
                        {
                            context.HttpContext.Response.Redirect("/Account/Login?check=1");
                            valid = false;
                        }
                        else
                        {
                            var response1 = await HttpRequestFactory.Get(string.Format("{0}{1}?UserId={2}&CompanyLocationId={3}", _apiBaseUrl,
                                                       Constants.WebConstants.Urls.API_GetExistingUserData, userID, companyLocationID), context.HttpContext.Session.GetString("token_value"));
                            var result1 = response1.ContentAsType<InputUser>();
                            if (result1 != null)
                            {
                                if (result1[Constants.WebConstants.Data] != null)
                                {
                                    InputUser resultUser = result1[Constants.WebConstants.Data].ToObject<InputUser>();
                                    if (resultUser.IsActive)
                                    {
                                        if (Auth_Role != null)
                                        {
                                            if (Auth_Role.ToLower() == Constants.WebConstants.Owner)
                                            {
                                                Auth_FeatureListWithGeneralCodeByUserIdResult owner = new Auth_FeatureListWithGeneralCodeByUserIdResult();
                                                owner.RoleName = Auth_Role;
                                                var Owner = new List<Auth_FeatureListWithGeneralCodeByUserIdResult>() { owner };
                                                context.HttpContext.Items.Add("PermissionData", Owner);
                                            }
                                            else
                                            {
                                                Guid userId = Guid.Parse(userID);
                                                Guid CompanyLocationId = Guid.Parse(companyLocationID);

                                                var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}?CompanyLocationId={3}", _apiBaseUrl,
                                                                   Constants.WebConstants.Urls.API_GetProfilePermissionsByAuth_UserId, userId, CompanyLocationId), context.HttpContext.Session.GetString("token_value"));
                                                if (!response.IsSuccessStatusCode)
                                                {
                                                    context.HttpContext.Items.Add("PermissionData", null);
                                                }
                                                else
                                                {
                                                    var result = response.ContentAsType<List<Auth_FeatureListWithGeneralCodeByUserIdResult>>();
                                                    var ResultData = result["data"].ToObject<List<Auth_FeatureListWithGeneralCodeByUserIdResult>>();
                                                    context.HttpContext.Items.Add("PermissionData", ResultData);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            context.HttpContext.Response.Redirect("/Account/Login?check=1");
                                            valid = false;
                                        }
                                    }
                                    else
                                    {
                                        context.HttpContext.Response.Redirect("/Account/Logout?isForceLogout=1");
                                        valid = false;
                                    }
                                }
                            }
                        }
                    }
                }
                if (valid)
                    await next(); // the actual action

                // logic after the action goes here
            }
            catch (Exception ex)
            {
                context.HttpContext.Response.Redirect("/Account/Login?check=1");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
