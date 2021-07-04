using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Leemo.Api.ActionFilters;
using Leemo.Api.Filters;
using TPSS.Common;
using TPSS.Common.Wrappers;
using Leemo.Model;
using Leemo.Model.ResultModels;
using Leemo.Service.Interface;
using Leemo.Model.Domain;

namespace Leemo.Api.Controllers
{
    /// <summary>
    /// Pofile controller class contains all the methods related to proflie entity.
    /// </summary>
    [Authorize]
    [Route(Constants.Attrributes.ApiDefaultRoute)]
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly IAuth_RoleService _roleService;
        private readonly AppSettings _appSettings;
        private readonly ICommonService _commonService;
        private List<object> _paraList = new List<object>();
        private string ErrMsg = "";
        private bool response = false;

        /// <summary>
        /// Constructor of pofile controller for initialize the required stuff.
        /// </summary>
        /// <param name="roleService">Refers to proflie service class</param>
        /// <param name="appSettings"></param>
        public RoleController(IAuth_RoleService roleService, IOptions<AppSettings> appSettings, ICommonService commonService)
        {
            _roleService = roleService;
            _appSettings = appSettings.Value;
            _commonService = commonService;
        }

        /// <summary>
        /// Return the list of Roles
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet]
        [Route(Constants.Attrributes.ListApiName)]
        public PagedResponse<Auth_Role> GetRoles([FromQuery] PaginationFilter filter,Guid CompanyLocationId)
        {
            IEnumerable<Auth_Role> roles = _roleService.GetAuth_Roles(CompanyLocationId);
            //profiles = profiles.Where(x => x.IsDeleted == false);
            try
            {
                if (roles != null && roles.Count() > 0)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(filter), ErrMsg);
                    return PagedResponse<Auth_Role>.PagedList(
                        roles,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK,
                        filter.PageNumber,
                        filter.PageSize);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(filter), ErrMsg);
                    return PagedResponse<Auth_Role>.PagedList(
                        roles,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK,
                        filter.PageNumber,
                        filter.PageSize);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<Auth_Role>.PagedList(
                    roles,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError,
                        filter.PageNumber,
                        filter.PageSize);
            }
        }

        /// <summary>
        /// Get proflie method for fetching
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Attrributes.GetByIdApiName)]
        public ApiResponse<Auth_Role> GetRole(Guid id)
        {
            try
            {
                if (id == null)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<Auth_Role>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }

                Auth_Role role = _roleService.GetAuth_Role(id);
                if (role != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<Auth_Role>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = role
                    };
                }
                ErrMsg = Constants.Messages.RecordNotFound;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                return new ApiResponse<Auth_Role>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.RecordNotFound,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Auth_Role>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// For inserting a new role record.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.Add)]
        [HttpPost(Constants.Attrributes.InsertApiName)]
        public ApiResponse<Auth_Role> CreateRole(Auth_Role role)
        {
            try
            {
                
                var roles = GetRoleByName(role.Name, role.CompanyLocationId ?? Guid.Empty);
                if (roles.Data != null)
                {
                    return new ApiResponse<Auth_Role>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.InternalServerError,
                        Message = Constants.Messages.RoleAlreadyExist,
                        Data = null
                    };
                }
                else 
                {
                    _roleService.CreateAuth_Role(role);
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(role), ErrMsg);
                    return new ApiResponse<Auth_Role>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = role
                    };
                }
              
            }
            catch (Exception ex)
            {
                return new ApiResponse<Auth_Role>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// Update role record against the role id passed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.Update)]
        [HttpPut(Constants.Attrributes.UpdateApiName)]
        public ApiResponse<Auth_Role> EditRole(Guid id, Auth_Role role)
        {
            try
            {
                _paraList.Add(id);
                _paraList.Add(role);
                if (id != role.Id)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<Auth_Role>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }

                if (ModelState.IsValid)
                {
                    Auth_Role currentRole = _roleService.GetAuth_Role(id);
                    if (currentRole != null)
                    {
                        _roleService.EditAuth_Role(role, currentRole);
                        response = Constants.ApiRequestResponse.ResponseSuccess;
                        _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                        return new ApiResponse<Auth_Role>
                        {
                            Succeeded = true,
                            ResponseCode = Enums.HttpStatusCode.OK,
                            Message = Constants.Messages.Success,
                            Data = role
                        };
                    }
                    ErrMsg = Constants.Messages.RecordNotFound;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<Auth_Role>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.NotFound,
                        Message = Constants.Messages.RecordNotFound,
                        Data = role
                    };
                }
                ErrMsg = Constants.Messages.InternalServerError;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                return new ApiResponse<Auth_Role>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Auth_Role>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// Return the list of role users
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet]
        [Route(Constants.Routes.GetRoleUsers)]
        public PagedResponse<ResultRoleUser> GetRoleusers(Guid roleId)
        {
            IEnumerable<ResultRoleUser> rolesUsers = _roleService.GetAuth_RoleUsersByAuth_RoleId(roleId);
            try
            {
                if (rolesUsers != null && rolesUsers.Count() > 0)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(roleId), ErrMsg);
                    return PagedResponse<ResultRoleUser>.PagedList(
                        rolesUsers,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(roleId), ErrMsg);
                    return PagedResponse<ResultRoleUser>.PagedList(
                        rolesUsers,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<ResultRoleUser>.PagedList(
                    rolesUsers,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Delete role record against the role id passed.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.Delete)]
        [HttpDelete(Constants.Attrributes.DeleteApiName)]
        public ApiResponse<Auth_Role> DeleteRole(Guid id)
        {
            if (id == null)
            {
                ErrMsg = Constants.Messages.BadRequest;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                return new ApiResponse<Auth_Role>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.BadRequest,
                    Data = null
                };
            }

            try
            {
                Auth_Role role = _roleService.GetAuth_Role(id);
                if (role != null)
                {
                    int ReturnValue = 0;
                    string ErrorMessage = string.Empty;
                    _roleService.DeleteAuth_Role(role, out ReturnValue, out ErrorMessage);
                    object _data = new { ReturnValue = ReturnValue, ErrorMessage = ErrorMessage };
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<Auth_Role>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = _data
                        //Data = role
                    };
                }
                else
                {
                    ErrMsg = Constants.Messages.RecordNotFound;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<Auth_Role>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.NotFound,
                        Message = Constants.Messages.RecordNotFound,
                        Data = role
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<Auth_Role>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// Return the list of Auth_Role Join Users
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet]
        [Route(Constants.Routes.GetAuth_RoleJoinedUsers)]
        public PagedResponse<ResultRole> GetAuth_RoleJoinedUsers(Guid CompanyLocationId, Guid CompanyId)
        {
            IEnumerable<ResultRole> Auth_RoleJoinUsers = _roleService.GetAuth_RoleJoinUsers(CompanyLocationId, CompanyId);
            try
            {
                if (Auth_RoleJoinUsers != null && Auth_RoleJoinUsers.Count() > 0)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<ResultRole>.PagedList(
                        Auth_RoleJoinUsers,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<ResultRole>.PagedList(
                        Auth_RoleJoinUsers,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<ResultRole>.PagedList(
                    Auth_RoleJoinUsers,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route(Constants.Routes.GetRolesForPersonalUser)]
        public PagedResponse<Auth_Role> GetRolesForPersonalUser([FromQuery] PaginationFilter filter, Guid CompanyLocationId)
        {
            IEnumerable<Auth_Role> roles = _roleService.GetAuth_Roles(CompanyLocationId);
            //profiles = profiles.Where(x => x.IsDeleted == false);
            try
            {
                if (roles != null && roles.Count() > 0)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(filter), ErrMsg);
                    return PagedResponse<Auth_Role>.PagedList(
                        roles,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK,
                        filter.PageNumber,
                        filter.PageSize);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(filter), ErrMsg);
                    return PagedResponse<Auth_Role>.PagedList(
                        roles,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK,
                        filter.PageNumber,
                        filter.PageSize);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<Auth_Role>.PagedList(
                    roles,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError,
                        filter.PageNumber,
                        filter.PageSize);
            }
        }


        /// <summary>
        /// Get role method for fetching by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Routes.GetRoleByName)]
        public ApiResponse<Auth_Role> GetRoleByName(string name,Guid companyLocationId)
        {
            try
            {
                if (name == null || name == string.Empty)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(name), ErrMsg);
                    return new ApiResponse<Auth_Role>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }

                Auth_Role role = _roleService.GetAuth_RoleByName(name, companyLocationId);
                if (role != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(name), ErrMsg);
                    return new ApiResponse<Auth_Role>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = role
                    };
                }
                ErrMsg = Constants.Messages.RecordNotFound;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(name), ErrMsg);
                return new ApiResponse<Auth_Role>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.RecordNotFound,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Auth_Role>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

    }
}