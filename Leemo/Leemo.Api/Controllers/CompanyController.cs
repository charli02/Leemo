using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Leemo.Api.ActionFilters;
using Leemo.Api.Filters;
using TPSS.Common;
using TPSS.Common.Wrappers;
using Leemo.Model;
using Leemo.Model.InputModels;
using Leemo.Service.Interface;
using Leemo.Model.Domain;
using Leemo.Model.WrapperModels;

namespace Leemo.Api.Controllers
{
    /// <summary>
    /// Company controller class contains all the methods related to company entity.
    /// </summary>
    [Authorize]
    [Route(Constants.Attrributes.ApiDefaultRoute)]
    [ApiController]
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;
        private readonly ICommonService _commonService;
        private readonly IAddressesService _addressService;
        private readonly ICompanyLocationService _companyLocationService;
        private readonly IAddressTypeService _addressTypeService;
        private List<object> _paraList = new List<object>();
        private string ErrMsg = "";
        private bool response = false;

        /// <summary>
        /// Constructor of compnay controller for initialize the required stuff.
        /// </summary>
        /// <param name="companyService">Refers to company service class</param>
        /// <param name="commonService">Refers to common service class</param>
        public CompanyController(ICompanyService companyService, ICommonService commonService, IAddressesService addressService, ICompanyLocationService companyLocationService, IAddressTypeService addressTypeService)
        {
            _companyService = companyService;
            _commonService = commonService;
            _addressService = addressService;
            _companyLocationService = companyLocationService;
            _addressTypeService = addressTypeService;
        }

        /// <summary>
        /// Return the list of companies
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.OrganizationSettings, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet]
        [Route(Constants.Attrributes.ListApiName)]
        public PagedResponse<Company> GetCompanies([FromQuery] PaginationFilter filter)
        {
            IEnumerable<Company> companies = _companyService.GetCompanies();
            try
            {
                if (companies != null && companies.Count() > 0)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(filter), ErrMsg);
                    return PagedResponse<Company>.PagedList(
                        companies,
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
                    return PagedResponse<Company>.PagedList(
                        companies,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK,
                        filter.PageNumber,
                        filter.PageSize);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<Company>.PagedList(
                    companies,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError,
                        filter.PageNumber,
                        filter.PageSize);
            }
        }

        /// <summary>
        /// Get company method for fetching
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.OrganizationSettings, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Attrributes.GetByIdApiName)]
        public ApiResponse<CompanyAndAddresses> GetCompany(Guid id)
        {
            try
            {
                var userClaims = User.Claims.FirstOrDefault();
                if (id == null || id == Guid.Empty)
                {
                    response = Constants.ApiRequestResponse.ResponseFailed;
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<CompanyAndAddresses>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }
                ResultCompanyAndAddresses company = new ResultCompanyAndAddresses();
                company.resultCompany = _companyService.GetCompany(id);
                //Guid HeadOfficeAddressId = _companyLocationService.GetLocationsByCompanyId(id).Where(x => x.IsHeadOffice == true).FirstOrDefault().AddressId;
                //company.CompanyAddress = _addressService.GetAddressById(HeadOfficeAddressId);
                Guid HeadOfficeLocationId = _companyLocationService.GetLocationsByCompanyId(id).Where(x => x.IsHeadOffice == true).FirstOrDefault().Id;
                Guid AddressTypeId = _addressTypeService.GetAddressTypeIdWithName(Constants.AddressTypeNames.CompanyAddress);
                company.CompanyAddress = _addressService.GetAddressByReference(HeadOfficeLocationId, AddressTypeId);
                if (company != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<CompanyAndAddresses>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = company
                    };
                }

                return new ApiResponse<CompanyAndAddresses>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.NotFound,
                    Message = Constants.Messages.RecordNotFound,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CompanyAndAddresses>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// For inserting a new company record.
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.OrganizationSettings, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.OrganizationSettings, Constants.PermissionConstants.CodeValue.Add)]
        [HttpPost(Constants.Attrributes.InsertApiName)]
        public ApiResponse<InputCompanyAndAddresses> CreateCompany(InputCompanyAndAddresses company)
        {
            try
            {
                _companyService.CreateCompany(company);
                response = Constants.ApiRequestResponse.ResponseSuccess;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(company), ErrMsg);
                return new ApiResponse<InputCompanyAndAddresses>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.Success,
                    Data = company
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<InputCompanyAndAddresses>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// Update company record against the company id passed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.OrganizationSettings, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.OrganizationSettings, Constants.PermissionConstants.CodeValue.Update)]
        [HttpPut(Constants.Attrributes.UpdateApiName)]
        public ApiResponse<InputCompanyAndAddresses> UpdateCompany(Guid id, InputCompanyAndAddresses company)
        {
            _paraList.Add(id);
            _paraList.Add(company);
            if (id != company.inputCompany.Id)
            {
                ErrMsg = Constants.Messages.BadRequest;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                return new ApiResponse<InputCompanyAndAddresses>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.BadRequest,
                    Data = null
                };
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _companyService.EditCompany(company);
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<InputCompanyAndAddresses>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = company
                    };
                }
                catch (Exception ex)
                {
                    return new ApiResponse<InputCompanyAndAddresses>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.InternalServerError,
                        Message = Constants.Messages.InternalServerError,
                        Data = null
                    };
                }
            }
            ErrMsg = Constants.Messages.InternalServerError;
            _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
            return new ApiResponse<InputCompanyAndAddresses>
            {
                Succeeded = false,
                ResponseCode = Enums.HttpStatusCode.InternalServerError,
                Message = Constants.Messages.InternalServerError,
                Data = null
            };
        }

        /// <summary>
        /// Update Company profile image against the company id passed.
        /// </summary>
        /// <param name="updateCompanyImage"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.OrganizationSettings, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.OrganizationSettings, Constants.PermissionConstants.CodeValue.Update)]
        [HttpPut(Constants.Routes.UpdateCompanyImage)]
        public ApiResponse<Company> UpdateCompanyImage(InputUpdateCompanyImage updateCompanyImage)
        {
            try
            {
                Company company = new Company();
                company = _companyService.UpdateCompanyImage(updateCompanyImage);
                if (company != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(updateCompanyImage), ErrMsg);
                    return new ApiResponse<Company>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.CompanyImageUpdated,
                        Data = company
                    };
                }
                ErrMsg = Constants.Messages.RecordNotFound;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(updateCompanyImage), ErrMsg);
                return new ApiResponse<Company>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.RecordNotFound,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Company>
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
