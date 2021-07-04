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
using Leemo.Api.Filters;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Model.WrapperModels;
using Leemo.Service.Interface;

namespace Leemo.Api.Controllers
{
    [Authorize]
    [Route(Constants.Attrributes.ApiDefaultRoute)]
    [ApiController]
    public class LocationController : BaseController
    {
        private readonly ICompanyLocationService _companyLocationService;
        private readonly IAuth_RoleUserMappingService _profilePermissionMappingService;
        private readonly ICommonService _commonService;
        private readonly AppSettings _appSettings;
        private readonly IAddressesService _addressService;
        private readonly ICompanyService _companyService;
        private readonly IAddressTypeService _addressTypeService;
        private List<object> _paraList = new List<object>();
        private string ErrMsg = "";
        private bool response = false;

        /// <summary>
        /// Constructor of user controller for initialize the required stuff.
        /// </summary>
        /// <param name="userService">Refers to user service class</param>
        /// <param name="profilePermissionMappingService"></param>
        /// <param name="appSettings"></param>
        /// <param name="commonService"></param>
        public LocationController(ICompanyLocationService companyLocationService,
            IAuth_RoleUserMappingService profilePermissionMappingService,
            ICommonService commonService,
            IOptions<AppSettings> appSettings, IAddressesService addressService,
            ICompanyService companyService, IAddressTypeService addressTypeService)
        {
            _companyLocationService = companyLocationService;
            _profilePermissionMappingService = profilePermissionMappingService;
            _commonService = commonService;
            _appSettings = appSettings.Value;
            _addressService = addressService;
            _companyService = companyService;
            _addressTypeService = addressTypeService;
        }

        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Locations, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet]
        [Route(Constants.Attrributes.ListApiName)]
        public PagedResponse<ResultLocation> GetAllLocations([FromQuery] PaginationFilter filter, Guid CompanyId)
        {
            var locations = new List<ResultLocation>();
            if (filter.GetActiveLocations == 0)
                locations = _companyLocationService.GetLocationsByCompanyId(CompanyId).OrderByDescending(x => x.ModifiedOn != null ? x.ModifiedOn : x.CreatedOn).ToList();

            if (filter.GetActiveLocations == 1)
                locations = _companyLocationService.GetActiveorInActiveLocation(CompanyId, true).OrderByDescending(x => x.ModifiedOn != null ? x.ModifiedOn : x.CreatedOn).ToList();

            if (filter.GetActiveLocations == 2)
                locations = _companyLocationService.GetActiveorInActiveLocation(CompanyId, false).OrderByDescending(x => x.ModifiedOn != null ? x.ModifiedOn : x.CreatedOn).ToList();
            try
            {
                if (locations != null && locations.Count() > 0)
                {
                    

                    if (!string.IsNullOrEmpty(filter.QuerySearch))
                    {
                        filter.QuerySearch = filter.QuerySearch.ToLower();
                        locations = locations.Where(a => (a.LocationName.ToLower().Contains(filter.QuerySearch))).ToList();

                    }
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    //_commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<ResultLocation>.PagedList(
                        locations,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<ResultLocation>.PagedList(
                        locations,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<ResultLocation>.PagedList(
                    locations,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Locations, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Attrributes.GetByIdApiName)]
        public ApiResponse<ResultLocationAndAddress> GetCompanyLocationById(Guid id)
        {
            try
            {
                ResultLocationAndAddress model = new ResultLocationAndAddress();
                var userClaims = User.Claims.FirstOrDefault();
                if (id == null || id == Guid.Empty)
                {
                    response = Constants.ApiRequestResponse.ResponseFailed;
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<ResultLocationAndAddress>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }
                CompanyLocation resultLocation = new CompanyLocation();
                model.ResultLocation = _companyLocationService.GetCompanyLocationById(id);
                //model.Address = _addressService.GetAddressById(model.ResultLocation.AddressId);
                Guid AddressTypeId = _addressTypeService.GetAddressTypeIdWithName(Constants.AddressTypeNames.CompanyAddress);
                model.Address = _addressService.GetAddressByReference(model.ResultLocation.Id,AddressTypeId);
                model.resultCompany = _companyService.GetCompany(model.ResultLocation.CompanyId);
                if (model != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<ResultLocationAndAddress>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = model
                    };
                }

                return new ApiResponse<ResultLocationAndAddress>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.NotFound,
                    Message = Constants.Messages.RecordNotFound,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultLocationAndAddress>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Locations, Constants.PermissionConstants.CodeValue.Add)]
        [HttpPost(Constants.Attrributes.InsertApiName)]
        public ApiResponse<InputLocationandAddress> CreateCompanyLocation(InputLocationandAddress inputLocationandAddress)
        {
            try
            {
                inputLocationandAddress.Addresses.AddressTypeId = _addressTypeService.GetAddressTypeIdWithName(Constants.AddressTypeNames.CompanyAddress);
                //inputLocationandAddress.inputLocation.AddressId = resultData.Id;
                var LocationCreated = _companyLocationService.CreateCompanyLocation(inputLocationandAddress.inputLocation);

                inputLocationandAddress.Addresses.ReferenceId = LocationCreated.Id; 
                var resultData = _addressService.CreateAddress(inputLocationandAddress.Addresses);

                response = Constants.ApiRequestResponse.ResponseSuccess;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(inputLocationandAddress), ErrMsg);
                return new ApiResponse<InputLocationandAddress>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.Success,
                    Data = inputLocationandAddress
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<InputLocationandAddress>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Locations, Constants.PermissionConstants.CodeValue.Update)]
        [HttpPut(Constants.Attrributes.UpdateApiName)]
        public ApiResponse<CompanyLocation> PutCompanyLocation(Guid id, InputLocationandAddress updateLocationandAddress)
        {
            _paraList.Add(id);
            _paraList.Add(updateLocationandAddress);
            try
            {
                if (id != updateLocationandAddress.inputLocation.Id)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<CompanyLocation>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }
                if (ModelState.IsValid)
                {
                    //updateLocationandAddress.Addresses.Id = updateLocationandAddress.inputLocation.AddressId;
                    _companyLocationService.EditCompanyLocation(updateLocationandAddress.inputLocation);
                    _addressService.EditAddress(updateLocationandAddress.Addresses);
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<CompanyLocation>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = updateLocationandAddress
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<CompanyLocation>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
            ErrMsg = Constants.Messages.InternalServerError;
            _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
            return new ApiResponse<CompanyLocation>
            {
                Succeeded = false,
                ResponseCode = Enums.HttpStatusCode.InternalServerError,
                Message = Constants.Messages.InternalServerError,
                Data = null
            };
        }

        [AllowAnonymous]
        [HttpGet(Constants.Routes.GetCompanyLocationByUserId)]
        public ApiResponse<ResultLocation> GetCompanyLocationByUserId(Guid UserId)
        {
            try
            {
                IEnumerable<ResultLocation> model;
                var userClaims = User.Claims.FirstOrDefault();
                if (UserId == null || UserId == Guid.Empty)
                {
                    response = Constants.ApiRequestResponse.ResponseFailed;
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(UserId), ErrMsg);
                    return new ApiResponse<ResultLocation>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }
                model = _companyLocationService.GetLocationsByUserId(UserId).Where(x => x.IsActive == true);
                if (model != null || model.Count()>0)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(UserId), ErrMsg);
                    return new ApiResponse<ResultLocation>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = model
                    };
                }

                return new ApiResponse<ResultLocation>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.NotFound,
                    Message = Constants.Messages.NotDataExistsInTable,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultLocation>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        [HttpGet(Constants.Routes.GetLocationCounts)]
        public ApiResponse<Dictionary<string, int>> GetLocationCounts(Guid CompanyId)
        {
            try
            {
                var resultCount = _companyLocationService.GetLocationCounts(CompanyId);
                if (resultCount != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(),
                        CommonFunction.returnJsontoString(CompanyId), ErrMsg);
                    return new ApiResponse<Dictionary<string, int>>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = resultCount
                    };
                }
                ErrMsg = Constants.Messages.NotDataExistsInTable;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(),
                    CommonFunction.returnJsontoString(CompanyId), ErrMsg);
                return new ApiResponse<Dictionary<string, int>>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.NotDataExistsInTable,
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
        /// <summary>
        /// Update Head Office
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isHeadOffice"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Locations, Constants.PermissionConstants.CodeValue.Update)]
        [HttpPut(Constants.Routes.UpdateHeadOffice)]
        public ApiResponse<CompanyLocation> UpdateHeadOffice(Guid id,bool isHeadOffice, Guid CompanyId)
        {
            _paraList.Add(id);
            _paraList.Add(isHeadOffice);
            try
            {
                if (id == null)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<CompanyLocation>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }
                    var resultLocation = _companyLocationService.UpdateHeadOffice(id,isHeadOffice,CompanyId);
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<CompanyLocation>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = resultLocation
                    };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CompanyLocation>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Locations, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Routes.GetLocationByName)]
        public ApiResponse<ResultLocation> GetLocationByName(string locationName)
        {
            try
            {
                if (locationName == null || locationName == "")
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(locationName), ErrMsg);
                    return new ApiResponse<ResultLocation>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }

                CompanyLocation resultLocation = _companyLocationService.GetLocationByName(locationName);
                if (resultLocation != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(locationName), ErrMsg);
                    return new ApiResponse<ResultLocation>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = resultLocation
                    };
                }
                ErrMsg = Constants.Messages.LocationNotExist;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(locationName), ErrMsg);
                return new ApiResponse<ResultLocation>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.LocationNotExist,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultLocation>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }
    }
}