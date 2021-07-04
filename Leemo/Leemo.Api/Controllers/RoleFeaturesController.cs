using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Leemo.Api.ActionFilters;
using TPSS.Common;
using TPSS.Common.Wrappers;
using Leemo.Model;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Service.Interface;
using Leemo.Model.Domain;

namespace Leemo.Api.Controllers
{
    /// <summary>
    /// Proflie Permission Controller
    /// </summary>
    [Authorize]
    [Route(Constants.Attrributes.ApiDefaultRoute)]
    [ApiController]
    public class RoleFeaturesController : BaseController
    {
        private readonly IAuth_RoleService _permissionService;
        private readonly IAuth_RoleFeatureMappingService _profilePermissionMappingService;
        private readonly AppSettings _appSettings;
        private readonly ICommonService _commonService;
        private List<object> _paraList = new List<object>();
        private string ErrMsg = "";
        private bool response = false;

        /// <summary>
        /// constructor for pforifle permissions
        /// </summary>
        /// <param name="profilePermissionService"></param>
        public RoleFeaturesController(IAuth_RoleService permissionService, IAuth_RoleFeatureMappingService profilePermissionMappingService, IOptions<AppSettings> appSettings, ICommonService commonService)
        {
            _permissionService = permissionService;
            _profilePermissionMappingService = profilePermissionMappingService;
            _appSettings = appSettings.Value;
            _commonService = commonService;
        }

        /// <summary>
        /// Return the list of proflies permissions
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.ViewPermissions)]
        [HttpGet]
        [Route(Constants.Attrributes.ListApiName)]
        public PagedResponse<Auth_FeatureListWithGeneralCodeByUserIdResult> GetAuth_RoleFeatures()
        {
            IEnumerable<Auth_FeatureListWithGeneralCodeByUserIdResult> permissions = _profilePermissionMappingService.GetAuth_RoleFeaturesByUserId(null,null,Guid.Empty);

            try
            {
                if (permissions != null && permissions.Count() > 0)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<Auth_FeatureListWithGeneralCodeByUserIdResult>.PagedList(
                        permissions,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<Auth_FeatureListWithGeneralCodeByUserIdResult>.PagedList(
                        permissions,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<Auth_FeatureListWithGeneralCodeByUserIdResult>.PagedList(
                    permissions,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Return the list of proflies permissions
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.ViewPermissions)]
        [HttpGet]
        [Route(Constants.Routes.GetProfilePermissionsByAuth_RoleId)]
        public PagedResponse<Auth_FeatureListWithGeneralCodeByUserIdResult> GetProfilePermissionsByAuth_RoleId(Guid id,Guid userId,Guid ProductId)
        {
            IEnumerable<Auth_FeatureListWithGeneralCodeByUserIdResult> profilePermissions = _profilePermissionMappingService.GetAuth_RoleFeaturesByUserId(userId, id,ProductId);

            try
            {
                if (profilePermissions != null && profilePermissions.Count() > 0)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return PagedResponse<Auth_FeatureListWithGeneralCodeByUserIdResult>.PagedList(
                        profilePermissions,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return PagedResponse<Auth_FeatureListWithGeneralCodeByUserIdResult>.PagedList(
                        profilePermissions,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<Auth_FeatureListWithGeneralCodeByUserIdResult>.PagedList(
                    profilePermissions,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// For inserting a new user for profile permission mapping.
        /// </summary>
        /// <param name="profilePermissionMapping"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.ViewPermissions)]
        //[ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.Add)]
        [HttpPost(Constants.Attrributes.InsertApiName)]
        public ApiResponse<InputAuth_RoleFeatureMapping> PostProfilePermission(InputAuth_RoleFeatureMapping profilePermissionMapping)
        {
            try
            {
                _profilePermissionMappingService.CreateAuth_RoleFeatureMapping(profilePermissionMapping);
                response = Constants.ApiRequestResponse.ResponseSuccess;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(profilePermissionMapping), ErrMsg);
                return new ApiResponse<InputAuth_RoleFeatureMapping>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.Success,
                    Data = profilePermissionMapping
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<InputAuth_RoleFeatureMapping>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// For updating record for profile permission mapping.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inputProfilePermissionMapping"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.ViewPermissions)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.UpdatePermissions)]
        [HttpPut(Constants.Attrributes.UpdateApiName)]
        public ApiResponse<Auth_RoleFeatureMapping> PutProfilePermission(Guid id, InputAuth_RoleFeatureMapping inputProfilePermissionMapping)
        {
            try
            {
                _paraList.Add(id);
                _paraList.Add(inputProfilePermissionMapping);
                if (id == null)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<Auth_RoleFeatureMapping>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }
                Auth_RoleFeatureMapping profilePermissionMapping = _profilePermissionMappingService.EditAuth_RoleFeatureMapping(id, inputProfilePermissionMapping);
                response = Constants.ApiRequestResponse.ResponseSuccess;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                return new ApiResponse<Auth_RoleFeatureMapping>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.Success,
                    Data = profilePermissionMapping
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Auth_RoleFeatureMapping>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// For updating record for profile permission mapping.
        /// </summary>
        /// <param name="inputAuth_RoleFeatureMappingTemp"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.ViewPermissions)]
        [HttpPost(Constants.Routes.InsertUpdateAuth_RoleFeatureMappingTemp)]
        //[AllowAnonymous]
        public ApiResponse<Auth_RoleFeatureMappingTemp> InsertUpdateAuth_RoleFeatureMappingTemp(InputAuth_RoleFeatureMappingTemp inputAuth_RoleFeatureMappingTemp)
        {
            try
            {
                _paraList.Add(inputAuth_RoleFeatureMappingTemp);
                if (inputAuth_RoleFeatureMappingTemp == null)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<Auth_RoleFeatureMappingTemp>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType= Constants.ResponseType.Error,
                        Data = null
                    };
                }
                IEnumerable<Auth_RoleFeatureMappingTemp> resultUpdate = _profilePermissionMappingService.UpdateAuth_FeatureCodeMappingTemp(inputAuth_RoleFeatureMappingTemp);
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<Auth_RoleFeatureMappingTemp>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.Update,
                        Data = resultUpdate
                    };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Auth_RoleFeatureMappingTemp>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    ResponseType = Constants.ResponseType.Error,
                    Data = null
                };
            }
        }

        /// <summary>
        /// For Bulk updating record for profile permission mapping.
        /// </summary>
        /// <param name="inputAuth_RoleFeatureMappingTemp"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.ViewPermissions)]
        [HttpPost(Constants.Routes.BulkUpdateAuth_RoleFeatureMappingTemp)]
        //[AllowAnonymous]
        public ApiResponse<Auth_RoleFeatureMappingTemp> BulkUpdateAuth_RoleFeatureMappingTemp(InputAuth_RoleFeatureMappingTemp inputAuth_RoleFeatureMappingTemp)
        {
            try
            {
                _paraList.Add(inputAuth_RoleFeatureMappingTemp);
                if (inputAuth_RoleFeatureMappingTemp == null)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<Auth_RoleFeatureMappingTemp>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }

                IEnumerable<Auth_RoleFeatureMappingTemp> resultUpdate = _profilePermissionMappingService.BulkUpdateAuth_FeatureCodeMappingTemp(inputAuth_RoleFeatureMappingTemp);
                response = Constants.ApiRequestResponse.ResponseSuccess;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                return new ApiResponse<Auth_RoleFeatureMappingTemp>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.Success,
                    ResponseType = Constants.ResponseType.Update,
                    Data = resultUpdate
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Auth_RoleFeatureMappingTemp>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    ResponseType = Constants.ResponseType.Error,
                    Data = null
                };
            }
        }

        /// <summary>
        /// For updating record for profile permission mapping.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.ViewPermissions)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.UpdatePermissions)]
        [HttpGet(Constants.Routes.UpdateAuth_RoleFeatureMappingChanges)]
        //[AllowAnonymous]
        public ApiResponse<int> UpdateAuth_RoleFeatureMappingChanges(Guid roleId, Guid userId)
        {
            try
            {
                _paraList.Add(roleId);
                _paraList.Add(userId);
                if (!(roleId != Guid.Empty && userId != Guid.Empty))
                {        
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<int>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                var result = _profilePermissionMappingService.UpdateAuth_RoleFeatureMappingChanges(roleId, userId );
                response = Constants.ApiRequestResponse.ResponseSuccess;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                return new ApiResponse<int>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.Success,
                    ResponseType = Constants.ResponseType.Insert,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    ResponseType = Constants.ResponseType.Error,
                    Data = null
                };
            }
        }
    }
}
