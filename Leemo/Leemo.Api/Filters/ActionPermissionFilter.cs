using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TPSS.Common;
using Leemo.Model.ResultModels;
using Leemo.Service.Interface;

namespace Leemo.Api.ActionFilters
{
    /// <summary>
    ///
    /// </summary>
    public class ActionPermissionFilterAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissionData"></param>
        public ActionPermissionFilterAttribute(params string[] permissionData) : base(typeof(ActionPermissionFilter))
        {
            Arguments = new object[] { permissionData };
        }


        /// <summary>
        /// 
        /// </summary>
        public class ActionPermissionFilter : ActionFilterAttribute
        {
            /// <summary>
            /// permission data send from controller inicluding PermissionName and access requested.
            /// </summary>
            public string[] PermissionData { get; set; }
            private readonly IAuth_RoleFeatureMappingService _auth_RoleFeatureMappingService;
            private readonly IUserService _userService;

            /// <summary>
            ///
            /// </summary>
            /// <param name="auth_RoleFeatureMappingService"></param>
            /// <param name="permissionData"></param>
            public ActionPermissionFilter(IUserService userService, string[] permissionData)
            {
                //_auth_RoleFeatureMappingService = auth_RoleFeatureMappingService;
                _userService = userService;
                this.PermissionData = permissionData;
            }


            public override void OnActionExecuting(ActionExecutingContext context)
            {
                var identity = context.HttpContext.User.Identity as ClaimsIdentity;
                bool IsAccessDenied = true; //Change its value to true when need to allow permissions 
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    if (claims != null && claims.Count() > 0)
                    {
                        Guid userId = Guid.Parse(identity.FindFirst(Constants.JwtTokenClaimType_UserId).Value);
                        Guid userLocationId = Guid.Parse(identity.FindFirst("UserLocationID").Value);

                        if (PermissionData != null && PermissionData[0] != null && PermissionData[1] != null)
                        {
                            var permissions = _userService.GetAuth_FeatureListWithGeneralCodeByUserId(userId,userLocationId);
                            if (permissions != null || permissions.Count() > 0) 
                            {
                                var accessPermission = permissions.Where(x => x.FeatureName == PermissionData[0] && x.CodeValue == PermissionData[1]).ToList();
                                if(accessPermission.Count() > 0)
                                    IsAccessDenied = false;
                            }
                            
                        }
                    }
                }

                if (IsAccessDenied)
                {
                    context.Result = new BadRequestObjectResult(new
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.AccessDenied,
                        Message = Constants.Messages.AccessDenied,
                        ResponseType = Constants.ResponseType.AccessDenied,
                        Data = string.Empty
                    });


                    
                    //context.Result = new BadRequestObjectResult(JsonConvert.SerializeObject(new
                    //{
                    //    Succeeded = false,
                    //    ResponseCode = Enums.HttpStatusCode.AccessDenied,
                    //    Message = Constants.Messages.AccessDenied,
                    //    Data = string.Empty
                    //}));
                    return;
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
}