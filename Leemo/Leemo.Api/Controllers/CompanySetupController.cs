using Leemo.Api.Controllers;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Service.Interface;
using Lemmo.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPSS.Common;
using TPSS.Common.Wrappers;


namespace Lemmo.Api.Controllers
{



    /// <summary>
    /// CompanySetup controller class contains all the methods related to CompanySetup entity.
    /// </summary>
    [Route(Constants.Attrributes.ApiDefaultRoute)]
    [ApiController]
    public class CompanySetupController : BaseController
    {
        private readonly IProductLeadService _productLeadService;
        private readonly ICommonService _commonService;
        private bool response = false;
        private string ErrMsg = "";
        private readonly AppSettings _appSettings;
        private List<object> _paraList = new List<object>();

        /// <summary>
        /// Constructor of CompanySetup controller for initialize the required stuff.
        /// </summary>
        /// <param name="productLeadService">Refers to productLead service class</param>
        /// <param name="appSettings"></param>
        /// <param name="commonService"></param>
       
        public CompanySetupController(IProductLeadService productLeadService,

            ICommonService commonService, 
            IOptions<AppSettings> appSettings)
        {
            _productLeadService = productLeadService;
            _commonService= commonService;
            _appSettings = appSettings.Value;
        }


        /// <summary>
        /// For inserting a new ProductLead record.
        /// </summary>
        /// <param name="inputProductLead"></param>
        /// <returns></returns>
        [HttpPost(Constants.Attrributes.InsertApiName)]
        public ApiResponse<InputProductLead> CreateProductLead(InputProductLead inputProductLead)
        {
            try
            {
                if (inputProductLead != null)
                {

                    var verifyEmail= _productLeadService.GetProductLeadByEmail(inputProductLead.Email);
                    var verifyCompanyName = _productLeadService.GetProductLeadByCompanyName(inputProductLead.CompanyName);



                    if (verifyEmail == true || verifyCompanyName == true) {

                        response = Constants.ApiRequestResponse.ResponseSuccess;
                        _commonService.ApiRequestLogInDb(Request.Path.Value, response, inputProductLead.Email, CommonFunction.returnJsontoString(inputProductLead), ErrMsg);
                        return new ApiResponse<InputProductLead>
                        {
                            Succeeded = true,
                            ResponseCode = Enums.HttpStatusCode.OK,
                            Message = Constants.Messages.DataAlreadyExists,
                            Data = null,
                            ResponseType = Constants.ResponseType.AlreadyExists
                        };

                    }


                        var resultData = _productLeadService.CreateProductLead(inputProductLead);

                    
                        response = Constants.ApiRequestResponse.ResponseSuccess;
                        _commonService.ApiRequestLogInDb(Request.Path.Value, response, inputProductLead.Email,  CommonFunction.returnJsontoString(inputProductLead), ErrMsg);
                        return new ApiResponse<InputProductLead>
                        {
                            Succeeded = true,
                            ResponseCode = Enums.HttpStatusCode.OK,
                            Message = Constants.Messages.Success,
                            Data = resultData,
                            ResponseType = Constants.ResponseType.Insert
                        };

                    }
                    else
                    {
                        response = Constants.ApiRequestResponse.ResponseFailed;
                       _commonService.ApiRequestLogInDb(Request.Path.Value, response, inputProductLead.Email, CommonFunction.returnJsontoString(inputProductLead), ErrMsg);
                        return new ApiResponse<InputProductLead>
                        {
                            Succeeded = false,
                            ResponseCode = Enums.HttpStatusCode.BadRequest,
                            Message = Constants.Messages.BadRequest,
                            ResponseType = Constants.ResponseType.Error,
                            Data = null
                        };

                    }
               
            }
            catch (Exception ex)
            {
                return new ApiResponse<InputProductLead>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }


        /// <summary>
        /// Get ProductLead method for fetching
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet(Constants.Attrributes.GetByIdApiName)]
        public ApiResponse<ResultProductLead> GetProductLead(Guid id)
        {
            try
            {
                ResultProductLead resultProductLead = new ResultProductLead();
                if (id == null)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, id.ToString(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<ResultProductLead>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }

                resultProductLead = _productLeadService.GetResultProductLeadById(id);

                if (resultProductLead != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, resultProductLead.Email, CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<ResultProductLead>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.Insert,
                        Data = resultProductLead
                    };
                }
                ErrMsg = Constants.Messages.ProductLeadDoesNotExist;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, resultProductLead.Email, CommonFunction.returnJsontoString(id), ErrMsg);
                return new ApiResponse<ResultProductLead>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.ProductLeadDoesNotExist,
                    ResponseType = Constants.ResponseType.NotFound,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultProductLead>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.Failed,
                    Data = null,
                    ResponseType = Constants.ResponseType.Error,
                };
            }
        }






        /// <summary>
        /// Get ProductLead method for fetching
        /// </summary>
        /// <param name="ProductLeadId"></param>
        /// <returns></returns>
        [HttpGet(Constants.Routes.VerifyProductLead)]
        public ApiResponse<UpdateInputProductLead> VerifyProductLead(string ProductLeadId)
        {
            try
            {
                
                if (ProductLeadId == null)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, ProductLeadId.ToString(),  CommonFunction.returnJsontoString(ProductLeadId), ErrMsg);
                    return new ApiResponse<UpdateInputProductLead>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }

                var  updateInputProductLead = _productLeadService.VerifyInputProductLeadById(ProductLeadId);
               
                if (updateInputProductLead != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, ProductLeadId.ToString(), CommonFunction.returnJsontoString(ProductLeadId), ErrMsg);
                    return new ApiResponse<UpdateInputProductLead>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.Insert,
                        Data = updateInputProductLead
                    };
                }
                ErrMsg = Constants.Messages.ProductLeadDoesNotExist;
                 _commonService.ApiRequestLogInDb(Request.Path.Value, response, ProductLeadId.ToString(), CommonFunction.returnJsontoString(ProductLeadId), ErrMsg);
                return new ApiResponse<UpdateInputProductLead>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.ProductLeadDoesNotExist,
                    ResponseType = Constants.ResponseType.NotFound,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<UpdateInputProductLead>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.Failed,
                    Data = null,
                    ResponseType = Constants.ResponseType.Error,
                };
            }
        }




        /// <summary>
        /// Get ProductLeadbyEmail method for fetching by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>

        [HttpGet(Constants.Routes.GetProductLeadByEmail)]
        public ApiResponse<InputProductLead> GetProductLeadByEmail(string email)
        {
            try
            {
                if (email == null || email == "")
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, email,  CommonFunction.returnJsontoString(email), ErrMsg);
                    return new ApiResponse<InputProductLead>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                var inputProductLead = _productLeadService.GetProductLeadByEmail(email);
                if (inputProductLead == true)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, email, CommonFunction.returnJsontoString(email), ErrMsg);
                    return new ApiResponse<InputProductLead>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.AlreadyExists,
                        Data = inputProductLead
                    };


                }
                else {
                    ErrMsg = Constants.Messages.ProductLeadDoesNotExist;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, email,  CommonFunction.returnJsontoString(email), ErrMsg);
                    return new ApiResponse<InputProductLead>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.ProductLeadDoesNotExist,
                        ResponseType = Constants.ResponseType.NotFound,
                        Data = null
                    };
                }
              
            }
            catch (Exception ex)
            {
                return new ApiResponse<InputProductLead>
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
        /// Get ProductLeadbyCompanyName method for fetching by CompanyName
        /// </summary>
        /// <param name="CompanyName"></param>
        /// <returns></returns>

        [HttpGet(Constants.Routes.GetProductLeadByCompanyName)]
        public ApiResponse<InputProductLead> GetProductLeadByCompanyName(string CompanyName)
        {
            try
            {
                if (CompanyName == null || CompanyName == "")
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, CompanyName, CommonFunction.returnJsontoString(CompanyName), ErrMsg);
                    return new ApiResponse<InputProductLead>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                var verifyCompanyName = _productLeadService.GetProductLeadByCompanyName(CompanyName);

                //var inputProductLead = _productLeadService.GetProductLeadByEmail(CompanyName);
                if (verifyCompanyName == true)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, CompanyName, CommonFunction.returnJsontoString(CompanyName), ErrMsg);
                    return new ApiResponse<InputProductLead>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.AlreadyExists,
                        Data = verifyCompanyName
                    };


                }
                else
                {
                    ErrMsg = Constants.Messages.ProductLeadDoesNotExist;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, CompanyName, CommonFunction.returnJsontoString(CompanyName), ErrMsg);
                    return new ApiResponse<InputProductLead>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.ProductLeadDoesNotExist,
                        ResponseType = Constants.ResponseType.NotFound,
                        Data = null
                    };
                }

            }
            catch (Exception ex)
            {
                return new ApiResponse<InputProductLead>
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
        /// Get ProductLead Check Available Domain method for fetching by domain
        /// </summary>
        /// <param name="domainName"></param>
        /// <returns></returns>
        [HttpGet(Constants.Routes.GetProductLeadCheckAvailableDomain)]
        public ApiResponse<InputProductLead> GetProductLeadCheckAvailableDomain(string domainName)
        {
            try
            {

                string message = "";

                if (domainName != null || domainName != "")
                {
                    

                if (domainName.Length >= _appSettings.DomainMinLength && domainName.Length <= _appSettings.DomainMaxLength)
                {
                    //validate Domain name
                    if (CommonFunction.RegexPatternCheckforDomain(domainName.Trim().ToLower()) == true)
                    {
                        
                        var inputProductLead = _productLeadService.GetProductLeadCheckAvailableDomain(domainName);
                        if (inputProductLead == Constants.ResponseType.AlreadyExists)
                        {
                            var suggestDomainName = _productLeadService.RandomNumberWithDomainSuggestion(domainName);
                            response = Constants.ApiRequestResponse.ResponseSuccess;
                            _commonService.ApiRequestLogInDb(Request.Path.Value, response, domainName, CommonFunction.returnJsontoString(domainName), ErrMsg);
                            return new ApiResponse<InputProductLead>
                            {
                                Succeeded = true,
                                ResponseCode = Enums.HttpStatusCode.OK,
                                Message = Constants.Messages.Success,
                                ResponseType = Constants.ResponseType.AlreadyExists,
                                Data = suggestDomainName
                            };


                        }
                        else
                        {
                            ErrMsg = Constants.Messages.ProductLeadDoesNotExist;
                            _commonService.ApiRequestLogInDb(Request.Path.Value, response, domainName, CommonFunction.returnJsontoString(domainName), ErrMsg);
                            return new ApiResponse<InputProductLead>
                            {
                                Succeeded = true,
                                ResponseCode = Enums.HttpStatusCode.OK,
                                Message = Constants.Messages.ProductLeadDoesNotExist,
                                ResponseType = Constants.ResponseType.NotFound,
                                Data = null
                            };
                        }
                    }
                        ErrMsg = Constants.Messages.BadRequest;
                        message = "*Start with alphabets, Enter numbers and alphabets only.";
                        _commonService.ApiRequestLogInDb(Request.Path.Value, response, domainName, CommonFunction.returnJsontoString(domainName), ErrMsg);
                        return new ApiResponse<InputProductLead>
                        {
                            Succeeded = false,
                            ResponseCode = Enums.HttpStatusCode.BadRequest,
                            Message = message /*Constants.Messages.BadRequest*/,
                            ResponseType = Constants.ResponseType.Error,
                            Data = null
                        };
                    }
                    ErrMsg = Constants.Messages.BadRequest;
                    message = "*DomainName should be between " + _appSettings.DomainMinLength + " and " + _appSettings.DomainMaxLength + " characters";
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, domainName, CommonFunction.returnJsontoString(domainName), ErrMsg);
                    return new ApiResponse<InputProductLead>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = message /*Constants.Messages.BadRequest*/,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                
                    ErrMsg = Constants.Messages.BadRequest;
                
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, domainName, CommonFunction.returnJsontoString(domainName), ErrMsg);
                    return new ApiResponse<InputProductLead>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                
                    
            

            }
            catch (Exception ex)
            {
                return new ApiResponse<InputProductLead>
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
        /// For update a new ProductLead record.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateInputProductLead"></param>
        /// <returns></returns>
        [HttpPut(Constants.Attrributes.UpdateApiName)]
        public ApiResponse<ResultProductLead> PutProductLead(Guid id ,UpdateInputProductLead updateInputProductLead)
        {
            try
            {
                _paraList.Add(id);
                _paraList.Add(updateInputProductLead);
                if (updateInputProductLead != null)
                {

                    var resultData = _productLeadService.EditUser(updateInputProductLead);


                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, updateInputProductLead.Id.ToString(), CommonFunction.returnJsontoString(updateInputProductLead), ErrMsg);
                    return new ApiResponse<ResultProductLead>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = resultData,
                        ResponseType = Constants.ResponseType.Insert
                    };

                }
                else
                {
                    response = Constants.ApiRequestResponse.ResponseFailed;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, updateInputProductLead.Id.ToString(), CommonFunction.returnJsontoString(updateInputProductLead), ErrMsg);
                    return new ApiResponse<ResultProductLead>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };

                }

            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultProductLead>
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
