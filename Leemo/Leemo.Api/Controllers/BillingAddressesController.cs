using Leemo.Api.ActionFilters;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Model.WrapperModels;
using Leemo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPSS.Common;
using TPSS.Common.Wrappers;

namespace Leemo.Api.Controllers
{
    [Authorize]
    [Route(Constants.Attrributes.ApiDefaultRoute)]
    [ApiController]
    public class BillingAddressesController : BaseController
    {
        private readonly ICommonService _commonService;
        private readonly IBillingAddressService _billingAddressService;
        private string ErrMsg = "";
        private bool response = false;
        private List<object> _paraList = new List<object>();

        public BillingAddressesController(ICommonService commonService,
            IBillingAddressService billingAddressService)
        {
            _commonService = commonService;
            _billingAddressService = billingAddressService;
        }

        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.BillingAddresses, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet]
        [Route(Constants.Attrributes.ListApiName)]
        public ApiResponse<ResultBillingAddress> GetBillingAddressesByCompanyLocation(Guid CompanyLocationId)
        {
            try
            {
                if (CompanyLocationId == null || CompanyLocationId == Guid.Empty)
                {
                    response = Constants.ApiRequestResponse.ResponseFailed;
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(CompanyLocationId), ErrMsg);
                    return new ApiResponse<ResultBillingAddress>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        Data = null
                    };
                }


                IEnumerable<ResultBillingAddress> model =_billingAddressService.GetBillingAddressByCompanyLocation(CompanyLocationId);
                if (model != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(CompanyLocationId), ErrMsg);
                    return new ApiResponse<ResultBillingAddress>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = model
                    };
                }

                return new ApiResponse<ResultBillingAddress>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.NotFound,
                    Message = Constants.Messages.RecordNotFound,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultBillingAddress>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }


        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.BillingAddresses, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Attrributes.GetByIdApiName)]
        public ApiResponse<ResultBillingAddress> GetBillingAddress(Guid id)
        {
            try
            {
                ResultBillingAddress billingAddress = new ResultBillingAddress();
                if (id == null)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<ResultBillingAddress>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                billingAddress = _billingAddressService.GetBillingAddressById(id);


                if (billingAddress != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<ResultBillingAddress>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.Insert,
                        Data = billingAddress
                    };
                }
                ErrMsg = Constants.Messages.NotDataExistsInTable;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                return new ApiResponse<ResultBillingAddress>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.UserDoesNotExist,
                    ResponseType = Constants.ResponseType.AlreadyExists,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultBillingAddress>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.Failed,
                    Data = null,
                    ResponseType = Constants.ResponseType.Error,
                };
            }
        }

        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.BillingAddresses, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.BillingAddresses, Constants.PermissionConstants.CodeValue.Add)]
        [HttpPost(Constants.Attrributes.InsertApiName)]
        public ApiResponse<Addresses> CreateBillingAddress(InputAddress inputAddress)
        {
            try
            {
                var createAddress = _billingAddressService.InsertBillingAddress(inputAddress);
                response = Constants.ApiRequestResponse.ResponseSuccess;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(inputAddress), ErrMsg);
                return new ApiResponse<Addresses>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.Success,
                    Data = createAddress
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Addresses>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }


        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.BillingAddresses, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.BillingAddresses, Constants.PermissionConstants.CodeValue.Update)]
        [HttpPut(Constants.Attrributes.UpdateApiName)]
        public ApiResponse<ResultBillingAddress> UpdateBillingAddress(Guid id, InputAddress inputAddress)
        {
            try
            {
                _paraList.Add(id);
                _paraList.Add(inputAddress);
                if (id != inputAddress.Id)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<ResultBillingAddress>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                if (ModelState.IsValid)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<ResultBillingAddress>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.Update,
                        Data = _billingAddressService.UpdateBillingAddress(inputAddress)
                    };
                }
                else
                {
                    ErrMsg = Constants.Messages.InternalServerError;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<ResultBillingAddress>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.InternalServerError,
                        Message = Constants.Messages.InternalServerError,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultBillingAddress>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    ResponseType = Constants.ResponseType.Error,
                    Data = null
                };
            }
        }

        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.BillingAddresses, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.BillingAddresses, Constants.PermissionConstants.CodeValue.Delete)]
        [HttpDelete(Constants.Attrributes.DeleteApiName)]
        public ApiResponse<ResultBillingAddress> DeleteBillingAddress(Guid id)
        {
            if (id == null)
            {
                ErrMsg = Constants.Messages.BadRequest;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                return new ApiResponse<ResultBillingAddress>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.BadRequest,
                    Data = null
                };
            }

            try
            {
                 _billingAddressService.DeleteBillingAddress(id);
                
                    int ReturnValue = 0;
                    string ErrorMessage = string.Empty;
                    _billingAddressService.DeleteBillingAddress(id);
                    object _data = new { ReturnValue = ReturnValue, ErrorMessage = ErrorMessage };
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<ResultBillingAddress>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = null
                    };
                
              
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultBillingAddress>
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


