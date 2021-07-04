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
using Leemo.Service.Interface;
using Leemo.Model.Domain;

namespace Leemo.Api.Controllers
{
    /// <summary>
    /// Designation controller class contains all the methods related to Designation entity.
    /// </summary>
    [Authorize]
    [Route(Constants.Attrributes.ApiDefaultRoute)]
    [ApiController]
    public class DesignationController : BaseController
    {
        private readonly IDesignationService _designationService;
        private readonly IDesignationHierarchyService _designationHierarchyService;
        //private readonly IProfilePermissionMappingService _profilePermissionMappingService;
        private readonly AppSettings _appSettings;
        private readonly ICommonService _commonService;
        private readonly IUserService _userService;
        private List<object> _paraList = new List<object>();
        private string ErrMsg = "";
        private bool response = false;

        /// <summary>
        /// Constructor of designation controller for initialize the required stuff.
        /// </summary>
        /// <param name="designationService">Refers to designation service class</param>
        /// <param name="designationHierarchyService"></param>
        /// <param name="appSettings"></param>
        /// <param name="commonService"></param>
        /// <param name="userService"></param>
        public DesignationController(IDesignationService designationService, IDesignationHierarchyService designationHierarchyService, IOptions<AppSettings> appSettings, ICommonService commonService, IUserService userService)
        {
            _designationService = designationService;
            _designationHierarchyService = designationHierarchyService;
            _appSettings = appSettings.Value;
            _commonService = commonService;
            _userService = userService;
            
        }

        /// <summary>
        /// Return the list of designation
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet]
        //[Route(Constants.Attrributes.ListApiName)]
        [Route(Constants.Routes.GetDesignationsByLocation)]
        public PagedResponse<Designation> GetDesignations(Guid companyLocationId)
        {
            try
            {
                IEnumerable<Designation> designations = _designationService.GetDesignations().Where(x=>x.CompanyLocationId == companyLocationId).ToList();
                if (designations != null && designations.Count() > 0)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<Designation>.PagedList(
                        designations,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<Designation>.PagedList(
                        designations,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<Designation>.PagedList(
                    null,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get designation method for fetching
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Attrributes.GetByIdApiName)]
        public ApiResponse<Designation> GetDesignation(Guid id)
        {
            try
            {
                if (id == null)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<Designation>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }

                Designation designation = _designationService.GetDesignation(id);
                if (designation != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<Designation>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.Insert,
                        Data = designation
                    };
                }
                ErrMsg = Constants.Messages.RecordNotFound;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                return new ApiResponse<Designation>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.RecordNotFound,
                    ResponseType=Constants.ResponseType.Error,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return PagedResponse<Designation>.PagedList(
                    null,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// For inserting a new designation record.
        /// </summary>
        /// <param name="designation"></param>
        /// <returns></returns> 
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.Add)]
        [HttpPost(Constants.Attrributes.InsertApiName)]
        public ApiResponse<Designation> CreateDesignation(Designation designation,Guid companyLocationId)
        {
            try
            {
                //designation.CreatedOn = DateTime.UtcNow;
                //designation.IsActive = true;
                designation.IsRoot = false;
                Guid guidCompanyLocationId = designation.CompanyLocationId;
                var checkDuplicates = GetDesignationByName(designation.Name, guidCompanyLocationId);

                if(designation.Id!=null || designation.ParentDesignationId != null)
                {
                    if (checkDuplicates.Data == null)
                    {
                        _designationService.CreateDesignation(designation);
                        response = Constants.ApiRequestResponse.ResponseSuccess;
                        _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(designation), ErrMsg);
                        return new ApiResponse<Designation>
                        {
                            Succeeded = true,
                            ResponseCode = Enums.HttpStatusCode.OK,
                            Message = Constants.Messages.Success,
                            ResponseType = Constants.ResponseType.Insert,
                            Data = designation
                        };
                    }
                    else
                    {
                        return new ApiResponse<Designation>
                        {
                            Succeeded = true,
                            ResponseCode = Enums.HttpStatusCode.OK,
                            Message = Constants.Messages.DesignationAlreadyExist,
                            ResponseType = Constants.ResponseType.AlreadyExists,
                            Data = null
                        };
                    }
                }
                else
                {
                    return new ApiResponse<Designation>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }
                
            }
            catch (Exception ex)
            {
                return new ApiResponse<Designation>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    ResponseType = Constants.ResponseType.Error,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// For set designation position in structure
        /// </summary>
        /// <param name="designationHierarchy"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.Update)]
        [HttpPost(Constants.Routes.SetPosition)]
        public ApiResponse<DesignationHierarchy> SetPosition(DesignationHierarchy designationHierarchy)
        {
            try
            {
                _designationHierarchyService.SetPosition(designationHierarchy);
                response = Constants.ApiRequestResponse.ResponseSuccess;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(designationHierarchy), ErrMsg);
                return new ApiResponse<DesignationHierarchy>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.Success,
                    Data = designationHierarchy
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<DesignationHierarchy>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// For get designation structure
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.ViewPermissions)]
        [HttpGet(Constants.Routes.GetDesignationStructure)]
        public PagedResponse<DesignationHierarchy> GetDesignationStructure(Guid CompanyLocationId)
        {
            try
            {
                IEnumerable<DesignationHierarchy> designationHierarchyList = _designationHierarchyService.GetDesignationHierarchyList(CompanyLocationId);

                if (designationHierarchyList != null && designationHierarchyList.Count() > 0)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<DesignationHierarchy>.PagedList(
                        designationHierarchyList,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<DesignationHierarchy>.PagedList(
                        designationHierarchyList,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<DesignationHierarchy>.PagedList(
                    null,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// For getting designation parent associated users
        /// </summary>
        /// <param name="DesignationId"></param>
        /// <returns></returns>        
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Routes.GetAssociatedUsersWithDesignation)]
        public ApiResponse<User> GetAssociatedUsersWithDesignation(Guid DesignationId, Guid CompanyId)
        {
            try
            {
                Guid? ParentDesignationId = _designationHierarchyService.GetParentDesignationId(DesignationId);
                if (ParentDesignationId != Guid.Empty)
                {
                    var ReportingUsers = _userService.GetUserByDesignationId(ParentDesignationId,CompanyId);
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(DesignationId), ErrMsg);
                    return PagedResponse<User>.PagedList(
                            ReportingUsers,
                            Constants.Messages.Success,
                            true,
                            Enums.HttpStatusCode.OK);
                }
                else
                {
                    response = Constants.ApiRequestResponse.ResponseFailed;
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(DesignationId), ErrMsg);
                    return PagedResponse<User>.PagedList(
                            null,
                            Constants.Messages.Success,
                            true,
                            Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<User>.PagedList(
                    null,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Update group record against the group id passed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="designation"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.Update)]
        [HttpPut(Constants.Attrributes.UpdateApiName)]
        public ApiResponse<Designation> PutDesignation(Guid id, Designation designation)
        {
            _paraList.Add(id);
            _paraList.Add(designation);
            try
            {
                if (id != designation.Id)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<Designation>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }
                if (ModelState.IsValid)
                {
                     _designationService.EditDesignation(designation);
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<Designation>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = designation
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<Designation>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
            ErrMsg = Constants.Messages.InternalServerError;
            _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
            return new ApiResponse<Designation>
            {
                Succeeded = false,
                ResponseCode = Enums.HttpStatusCode.InternalServerError,
                Message = Constants.Messages.InternalServerError,
                Data = null
            };
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route(Constants.Routes.GetPersonalDesignation)]
        public PagedResponse<Designation> GetPersonalDesignation(Guid companyLocationId)
        {
            try
            {
                IEnumerable<Designation> designations = _designationService.GetDesignations().Where(x=>x.CompanyLocationId == companyLocationId && x.IsActive == true).ToList();
                if (designations != null && designations.Count() > 0)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<Designation>.PagedList(
                        designations,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<Designation>.PagedList(
                        designations,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<Designation>.PagedList(
                    null,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet(Constants.Routes.GetAssociatedUsersWithDesignationforPersonal)]
        public ApiResponse<User> GetAssociatedUsersWithDesignationforPersonal(Guid DesignationId, Guid CompanyId)
        {
            try
            {
                Guid? ParentDesignationId = _designationHierarchyService.GetParentDesignationId(DesignationId);
                if (ParentDesignationId != Guid.Empty)
                {
                    var ReportingUsers = _userService.GetUserByDesignationId(ParentDesignationId,CompanyId);
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(DesignationId), ErrMsg);
                    return PagedResponse<User>.PagedList(
                            ReportingUsers,
                            Constants.Messages.Success,
                            true,
                            Enums.HttpStatusCode.OK);
                }
                else
                {
                    response = Constants.ApiRequestResponse.ResponseFailed;
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(DesignationId), ErrMsg);
                    return PagedResponse<User>.PagedList(
                            null,
                            Constants.Messages.Success,
                            true,
                            Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<User>.PagedList(
                    null,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.Delete)]
        [HttpDelete(Constants.Attrributes.DeleteApiName)]
        public ApiResponse<Designation> DeleteDesignation(Guid id)
        {
            if (id == null)
            {
                ErrMsg = Constants.Messages.BadRequest;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                return new ApiResponse<Designation>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.BadRequest,
                    Data = null
                };
            }

            try
            {
                Designation designation = _designationService.GetDesignation(id);
                if (designation != null)
                {
                    int ReturnValue=0;
                    string ErrorMessage = string.Empty;
                    _designationService.DeleteDesignation(id, out ReturnValue, out ErrorMessage);
                    object _data = new { ReturnValue = ReturnValue, ErrorMessage = ErrorMessage };
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<Designation>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = _data
                    };
                }
                else
                {
                    ErrMsg = Constants.Messages.RecordNotFound;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<Designation>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.NotFound,
                        Message = Constants.Messages.RecordNotFound,
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<Designation>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Routes.GetDesignationByName)]
        public ApiResponse<Designation> GetDesignationByName(string name,Guid companyLocationId)
        {
            try
            {
                if (name == null || name == "")
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(name), ErrMsg);
                    return new ApiResponse<Designation>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                
                Designation designation = _designationService.GetDesignationByName(name, companyLocationId);
                if (designation != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(name), ErrMsg);
                    return new ApiResponse<Designation>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.Insert,
                        Data = designation
                    };
                }
                ErrMsg = Constants.Messages.RecordNotFound;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(name), ErrMsg);
                return new ApiResponse<Designation>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.RecordNotFound,
                    ResponseType = Constants.ResponseType.Error,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return PagedResponse<Designation>.PagedList(
                    null,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// For getting designation associated users
        /// </summary>
        /// <param name="DesignationId"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>        
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Designation, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Routes.GetUsersWithDesignation)]
        public ApiResponse<User> GetUsersWithDesignation(Guid DesignationId, Guid CompanyId)
        {
            try
            {
                if (DesignationId != Guid.Empty && DesignationId != null)
                {
                    var AccociatedUsers = _userService.GetUserByDesignationId(DesignationId,CompanyId);
                    if (AccociatedUsers.Count() > 0)
                    {
                        response = Constants.ApiRequestResponse.ResponseSuccess;
                        _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(DesignationId), ErrMsg);
                        return PagedResponse<User>.PagedList(
                                AccociatedUsers,
                                Constants.Messages.Success,
                                true,
                                Enums.HttpStatusCode.OK);
                    }
                    else
                    {
                        response = Constants.ApiRequestResponse.ResponseFailed;
                        ErrMsg = Constants.Messages.NotDataExistsInTable;
                        _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(DesignationId), ErrMsg);
                        return PagedResponse<User>.PagedList(
                                null,
                                ErrMsg,
                                true,
                                Enums.HttpStatusCode.OK);
                    }
                }
                else
                {
                    response = Constants.ApiRequestResponse.ResponseFailed;
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(DesignationId), ErrMsg);
                    return PagedResponse<User>.PagedList(
                            null,
                            ErrMsg,
                            false,
                            Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<User>.PagedList(
                    null,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

    }
}
