using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using TPSS.Common;
using TPSS.Common.Wrappers;
using Leemo.Api.ActionFilters;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;
using Leemo.Service.Interface;

namespace Leemo.Api.Controllers
{
    /// <summary>
    /// Designation Hierarchy controller class contains all the methods related to designation hierarchy entity.
    /// </summary>
    [Authorize]
    [Route(Constants.Attrributes.ApiDefaultRoute)]
    [ApiController]
    public class DesignationHierarchyController : BaseController
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
        public DesignationHierarchyController(IDesignationService designationService, IDesignationHierarchyService designationHierarchyService, IOptions<AppSettings> appSettings, ICommonService commonService, IUserService userService)
        {
            _designationService = designationService;
            _designationHierarchyService = designationHierarchyService;
            _appSettings = appSettings.Value;
            _commonService = commonService;
            _userService = userService;

        }


        /// <summary>
        /// For get designation structure
        /// </summary>
        /// <returns></returns>
        [HttpGet(Constants.Routes.GetDesignationHierarchy)]
        public PagedResponse<DesignationHierarchy> GetDesignationHierarchy(Guid CompanyLocationId)
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
        /// For inserting a new Designation Hierarchy record.
        /// </summary>
        /// <param name="designationHierarchy"></param>
        /// <returns></returns> 
        //[ActionPermissionFilter(Constants.ProfilePermissionNames.DesignationManagement, Constants.AccessRequested.Insert)]
        [HttpPost(Constants.Attrributes.InsertApiName)]
        public ApiResponse<DesignationHierarchy> CreateDesignationHierarchy(DesignationHierarchy designationHierarchy)
        {
            try
            {
                // roleHierarchy.CreatedOn = DateTime.UtcNow;
                _designationHierarchyService.SetPosition(designationHierarchy);
                response = Constants.ApiRequestResponse.ResponseSuccess;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(designationHierarchy), ErrMsg);
                return new ApiResponse<DesignationHierarchy>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.Success,
                    ResponseType = Constants.ResponseType.Insert,
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
                    ResponseType = Constants.ResponseType.Error,
                    Data = null
                };
            }
        }

        /// <summary>
        /// Get Designation Hierarchies
        /// </summary>
        /// <param name="designationHierarchies"></param>
        /// <returns></returns>
        [HttpPost(Constants.Routes.ResetDesignationHierarchy)]
        public ApiResponse<CommonResultModel> ResetDesignationHierarchy(IList<DesignationHierarchy> designationHierarchies)
        {
            try
            {
                // roleHierarchy.CreatedOn = DateTime.UtcNow;
                _designationHierarchyService.ResetDesignationHierarchies(designationHierarchies);
                response = Constants.ApiRequestResponse.ResponseSuccess;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(true), ErrMsg);
                return new ApiResponse<CommonResultModel>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.Success,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CommonResultModel>
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