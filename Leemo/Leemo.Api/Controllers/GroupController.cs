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
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Service.Interface;
using Leemo.Model.Domain;

namespace Leemo.Api.Controllers
{
    /// <summary>
    /// Group controller class contains all the methods related to Group entity.
    /// </summary>
    [Authorize]
    [Route(Constants.Attrributes.ApiDefaultRoute)]
    [ApiController]
    public class GroupController : BaseController
    {
        private readonly IGroupService _groupService;
        private readonly AppSettings _appSettings;
        private readonly ICommonService _commonService;
        private List<object> _paraList = new List<object>();
        private string ErrMsg = "";
        private bool response = false;

        /// <summary>
        /// Constructor of Group controller for initialize the required stuff.
        /// </summary>
        /// <param name="groupService">Refers to Group service class</param>
        /// <param name="appSettings"></param>
        /// <param name="commonService"></param>
        public GroupController(IGroupService groupService, IOptions<AppSettings> appSettings, ICommonService commonService)
        {
            _groupService = groupService;
            _appSettings = appSettings.Value;
            _commonService = commonService;
        }

        /// <summary>
        /// Return the list of grouops
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Groups, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet]
        //[Route(Constants.Attrributes.ListApiName)]
        [Route(Constants.Routes.GetGroupsByLocation)]
        public PagedResponse<Group> GetGroups([FromQuery] PaginationFilter filter, Guid companyLocationId)
        {
            var groups = new List<Group>();

            if (filter.GetActiveGroups == 0)
            {
                groups = _groupService.GetGroups().Where(y => y.CompanyLocationId == companyLocationId)
                                                                     .OrderByDescending(x => x.ModifiedOn != null ? x.ModifiedOn : x.CreatedOn).ToList();
            }
            else if (filter.GetActiveGroups == 1)
            {
                groups = _groupService.GetActiveGroup(companyLocationId).OrderByDescending(x => x.ModifiedOn != null ? x.ModifiedOn : x.CreatedOn).ToList();
            }
            else
            {
                groups = _groupService.GetInActiveGroup(companyLocationId).OrderByDescending(x => x.ModifiedOn != null ? x.ModifiedOn : x.CreatedOn).ToList();
            }

            try
            {
                if (groups != null && groups.Count() > 0)
                {
                    //CHECK FOR ACTIVE AND ALL USERS BESED ON PAGINATION PARAMETER
                    //if (filter.GetActiveGroups == 1)
                    //    groups = groups.Where(x => x.IsActive);

                    //if (filter.GetActiveGroups == 2)
                    //    groups = groups.Where(x => x.IsActive != true);

                    if (!string.IsNullOrEmpty(filter.QuerySearch))
                    {
                        filter.QuerySearch = filter.QuerySearch.ToLower();
                        groups = groups.Where(a => (a.Name.ToLower().Contains(filter.QuerySearch))).ToList();

                    }
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(filter), ErrMsg);
                    return PagedResponse<Group>.PagedList(
                        groups,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(filter), ErrMsg);
                    return PagedResponse<Group>.PagedList(
                        groups,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<Group>.PagedList(
                    groups,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// For inserting a new Group record.
        /// </summary>
        /// <param name="inputGroup"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Groups, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Groups, Constants.PermissionConstants.CodeValue.Add)]
        [HttpPost(Constants.Attrributes.InsertApiName)]
        public ApiResponse<ResultGroup> CreateGroup(InputGroup inputGroup)
        {
            try
            {
                ResultGroup resultGroup = _groupService.CreateGroup(inputGroup);
                response = Constants.ApiRequestResponse.ResponseSuccess;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(inputGroup), ErrMsg);
                return new ApiResponse<ResultGroup>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.Success,
                    Data = inputGroup
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultGroup>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// Get group method for fetching
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Groups, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Attrributes.GetByIdApiName)]
        public ApiResponse<ResultGroup> GetGroup(Guid id)
        {
            try
            {
                if (id == null)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<ResultGroup>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }

                ResultGroup resultGroup = _groupService.GetGroup(id);
                if (resultGroup != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<ResultGroup>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = resultGroup
                    };
                }
                ErrMsg = Constants.Messages.RecordNotFound;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                return new ApiResponse<ResultGroup>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.RecordNotFound,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultGroup>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// Update group record against the group id passed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inputGroup"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Groups, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Groups, Constants.PermissionConstants.CodeValue.Update)]
        [HttpPut(Constants.Attrributes.UpdateApiName)]
        public ApiResponse<ResultGroup> PutGroup(Guid id, InputGroup inputGroup)
        {
            _paraList.Add(id);
            _paraList.Add(inputGroup);
            try
            {
                if (id != inputGroup.Id)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<ResultGroup>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }
                if (ModelState.IsValid)
                {
                    ResultGroup resultData = _groupService.EditGroup(inputGroup);
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<ResultGroup>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = resultData
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultGroup>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
            ErrMsg = Constants.Messages.InternalServerError;
            _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
            return new ApiResponse<ResultGroup>
            {
                Succeeded = false,
                ResponseCode = Enums.HttpStatusCode.InternalServerError,
                Message = Constants.Messages.InternalServerError,
                Data = null
            };
        }
        /// <summary>
        /// return group record against the group name passed.
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>        
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Groups, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Routes.GetGroupByName)]
        public ApiResponse<ResultGroup> GetGroupByName(string groupName, Guid companyLocationId)
        {
            try
            {
                if (groupName == null || groupName == "")
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(groupName), ErrMsg);
                    return new ApiResponse<ResultGroup>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }

                Group resultGroup = _groupService.GetGroupByName(groupName, companyLocationId);
                if (resultGroup != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(groupName), ErrMsg);
                    return new ApiResponse<ResultGroup>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = resultGroup
                    };
                }
                ErrMsg = Constants.Messages.GroupNotExist;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(groupName), ErrMsg);
                return new ApiResponse<ResultGroup>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.GroupNotExist,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultGroup>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// Update Group profile image against the group id passed.
        /// </summary>
        /// <param name="updateGroupImage"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Groups, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Groups, Constants.PermissionConstants.CodeValue.Update)]
        [HttpPut(Constants.Routes.UpdateGroupImage)]
        public ApiResponse<Group> UpdateGroupImage(InputUpdateGroupImage updateGroupImage)
        {
            try
            {
                Group resultGroup = _groupService.UpdateGroupImage(updateGroupImage);
                if (resultGroup != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(updateGroupImage), ErrMsg);
                    return new ApiResponse<Group>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.GroupImageUpdated,
                        Data = resultGroup
                    };
                }
                ErrMsg = Constants.Messages.GroupNotExist;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(updateGroupImage), ErrMsg);
                return new ApiResponse<Group>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.GroupNotExist,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Group>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }


        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Groups, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Routes.GetGroupCountsByLocation)]
        public ApiResponse<Dictionary<string , int>> GetGroupCounts(Guid companyLocationid)
        {
            try
            {
                var resultGroup = _groupService.GetGroupCounts(companyLocationid);
                if (resultGroup != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(),
                        CommonFunction.returnJsontoString(companyLocationid), ErrMsg);
                    return new ApiResponse<Dictionary<string, int>>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.GroupImageUpdated,
                        Data = resultGroup
                    };
                }
                ErrMsg = Constants.Messages.GroupNotExist;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(),
                    CommonFunction.returnJsontoString(companyLocationid), ErrMsg);
                return new ApiResponse<Dictionary<string, int>>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.GroupNotExist,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Dictionary<string, int>>
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
