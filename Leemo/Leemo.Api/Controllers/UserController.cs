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
using System.Security.Claims;
using Newtonsoft.Json;
using Leemo.Model.WrapperModels;

namespace Leemo.Api.Controllers
{
    /// <summary>
    /// User controller class contains all the methods related to user entity.
    /// </summary>
    [Authorize]
    [Route(Constants.Attrributes.ApiDefaultRoute)]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuth_RoleUserMappingService _profilePermissionMappingService;
        private readonly ICommonService _commonService;
        private readonly AppSettings _appSettings;
        private readonly IAddressesService _addressService;
        private readonly ICompanyService _companyService;
        private readonly ICompanyLocationUserMappingService _companyLocationUserMappingService;
        private readonly ICompanyLocationService _companyLocationService;
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
        public UserController(ICompanyService companyService,
                              IUserService userService,
                              IAuth_RoleUserMappingService profilePermissionMappingService,
                              ICommonService commonService, IOptions<AppSettings> appSettings,
                              IAddressesService addressService,
                              ICompanyLocationUserMappingService companyLocationUserMappingService, 
                              ICompanyLocationService companyLocationService,
                              IAddressTypeService addressTypeService)
        {
            _userService = userService;
            _profilePermissionMappingService = profilePermissionMappingService;
            _commonService = commonService;
            _appSettings = appSettings.Value;
            _addressService = addressService;
            _companyService = companyService;
            _companyLocationUserMappingService = companyLocationUserMappingService;
            _companyLocationService = companyLocationService;
            _addressTypeService = addressTypeService;
        }

        /// <summary>
        /// Return the list of active users with username and status
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Users, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet]
        [Route(Constants.Routes.GetActiveUsers)]
        public PagedResponse<ResultActiveUser> GetActiveUsers()
        {
            IEnumerable<ResultActiveUser> users = _userService.GetActiveUsers();
            try
            {
                if (users != null && users.Count() > 0)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<ResultActiveUser>.PagedList(
                        users,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<ResultActiveUser>.PagedList(
                        users,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<ResultActiveUser>.PagedList(
                    users,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Return the list of users
        /// </summary>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Users, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet]
        [Route(Constants.Attrributes.ListApiName)]
        public PagedResponse<ResultUser> GetUsers([FromQuery] PaginationFilter filter, Guid CompanyId)
        {
            IEnumerable<ResultUser> users = _userService.GetUsers(CompanyId);
            try
            {
                if (users != null && users.Count() > 0)
                {
                    //ELIMINATE USERS WHOSE PROFILE ARE NULL
                    users = users.Where(x => x.UserProfile != null).OrderByDescending(x => x.UserProfile.ModifiedOn != null ? x.UserProfile.ModifiedOn : x.UserProfile.CreatedOn);
                    //CHECK FOR ACTIVE AND ALL USERS BESED ON PAGINATION PARAMETER
                    if (filter.GetActiveUsrs == 1)
                        users = users.Where(x => x.IsActive);
                    if (filter.GetActiveUsrs == 2)
                        users = users.Where(x => !(x.IsActive));

                    //FUNCTION FOR SEARCH USER BY FULLNAME OR EMAIL
                    //Search Parameter [With null check]  
                    if (!string.IsNullOrEmpty(filter.QuerySearch))
                    {
                        filter.QuerySearch = filter.QuerySearch.ToLower();
                        users = users.Where(a => (a.UserName.ToLower().Contains(filter.QuerySearch) || String.Format("{0} {1}", a.UserProfile == null ? "" : a.UserProfile.FirstName, a.UserProfile == null ? "" : a.UserProfile.LastName).ToLower().Contains(filter.QuerySearch)));

                    }
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(filter), ErrMsg);
                    return PagedResponse<ResultUser>.PagedList(
                        users,
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
                    return PagedResponse<ResultUser>.PagedList(
                        users,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK,
                        filter.PageNumber,
                        filter.PageSize);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<ResultUser>.PagedList(
                    users,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError,
                    filter.PageNumber,
                    filter.PageSize);
            }
        }

        /// <summary>
        /// For inserting a new user record.
        /// </summary>
        /// <param name="inputUser"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Users, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Users, Constants.PermissionConstants.CodeValue.Add)]
        [HttpPost(Constants.Attrributes.InsertApiName)]
        public ApiResponse<ResultUser> PostUser(InputUser inputUser)
        {
            try
            {
                User user = null;
                if (inputUser.IsExistingUser)
                {
                    CompanyLocationUserMapping companyLocationUserMapping1 = _companyLocationUserMappingService.GetCompanyLocationUserMapping(inputUser.CompanyLocationId, (Guid)inputUser.ExistingUserData.Id);
                    if (companyLocationUserMapping1 != null)
                    {
                        ErrMsg = Constants.Messages.DataAlreadyExists;
                        _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(companyLocationUserMapping1), ErrMsg);
                        return new ApiResponse<ResultUser>
                        {
                            Succeeded = true,
                            ResponseCode = Enums.HttpStatusCode.OK,
                            Message = Constants.Messages.DataAlreadyExists,
                            ResponseType = Constants.ResponseType.AlreadyExists,
                            Data = null
                        };
                    }
                }
                if (!inputUser.IsExistingUser)
                    user = _userService.GetUserByEmail(inputUser.UserName, inputUser.userProfile.CompanyId);
                if (user == null)
                {
                    ResultUser resultUser = _userService.CreateUser(inputUser);
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(inputUser), ErrMsg);
                    return new ApiResponse<ResultUser>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.Insert,
                        Data = resultUser
                    };
                }
                else
                {
                    ErrMsg = Constants.Messages.UserAlreadyExists;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(inputUser), ErrMsg);
                    return new ApiResponse<ResultUser>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.UserAlreadyExists,
                        ResponseType = Constants.ResponseType.AlreadyExists,
                        Data = null
                    };
                }

            }

            catch (Exception ex)
            {
                return new ApiResponse<ResultUser>
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
        /// Get user method for fetching
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Users, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Attrributes.GetByIdApiName)]
        public ApiResponse<ResultUserAndAddresses> GetUser(Guid id, Guid CompanyId)
        {
            try
            {
                ResultUserAndAddresses userResult = new ResultUserAndAddresses();
                if (id == null)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<ResultUserAndAddresses>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                userResult.ResultUser = _userService.GetUser(id, CompanyId, getCompanyLocationId());
                Guid AddressTypeId = _addressTypeService.GetAddressTypeIdWithName(Constants.AddressTypeNames.UserAddress);
                userResult.userAddress = _addressService.GetAddressByReference(id,AddressTypeId);


                if (userResult.ResultUser != null)
                {
                    var ResultuserCompanyId = userResult.ResultUser.UserProfile.CompanyId.ToString();
                    userResult.resultCompany =
                        _companyService.GetCompany(Guid.Parse(ResultuserCompanyId));
                }


                if (userResult != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<ResultUserAndAddresses>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.Insert,
                        Data = userResult
                    };
                }
                ErrMsg = Constants.Messages.UserDoesNotExist;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                return new ApiResponse<ResultUserAndAddresses>
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
                return new ApiResponse<ResultUserAndAddresses>
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
        /// Validate user using email and password
        /// </summary>
        /// <param name="inputUserLogin"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route(Constants.Routes.Login)]
        public ApiResponse<ResultUser> ValidateUser(InputUserLogin inputUserLogin)
        {
            try
            {
                User user = _userService.GetUserByUserName(inputUserLogin.Email);
                if (user == null)
                {
                    return new ApiResponse<ResultUser>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.UserDoesNotExist,
                        Data = null
                    };
                }
                else if (!(user.IsActive))
                {
                    return new ApiResponse<ResultUser>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.UserDisabled,
                        Data = null
                    };
                }
                else if (CommonFunction.VerifyPassword(string.Concat(user.PasswordHash, ":", user.PasswordSalt), inputUserLogin.Password) && user.IsActive)
                {
                    var profilePermissions = _profilePermissionMappingService.GetAuth_RoleUserMappingByUserId(user.Id);

                    ResultUser resultUser = _userService.GetUser(user.Id, user.UserProfile.CompanyId, inputUserLogin.CompanyLocationID);
                    if (resultUser != null)
                    {
                        string responseData = JsonConvert.SerializeObject(resultUser).ToString(); //Serialize resultUser object into JSON string in this object
                        List<Guid> locationIds = _companyLocationService.GetLocationsByUserId(user.Id).Where(x => x.IsActive == true).Select(x => x.Id).ToList();
                        if (locationIds.Count == 0 || locationIds == null)
                        {
                            return new ApiResponse<ResultUser>
                            {
                                Succeeded = false,
                                ResponseCode = Enums.HttpStatusCode.BadRequest,
                                Message = Constants.Messages.LocationDisabled,
                                Data = null
                            };
                        }

                        //Then add above object into following claim so that we can fetch it in controllers.
                        var locationID = string.Empty;
                        if (inputUserLogin.CompanyLocationID != null)
                        {
                            locationID = inputUserLogin.CompanyLocationID.ToString();
                        }
                        else
                        {
                            locationID = _companyLocationUserMappingService.GetByIds(user.Id).Where(x => x.isBaseLocation).Select(x => x.CompanyLocationId).FirstOrDefault().ToString();
                        }
                        //if (_companyLocationUserMappingService.GetByIds(user.Id).Count() == 1)
                        //{
                        //    locationID = _companyLocationUserMappingService.GetByIds(user.Id).FirstOrDefault().CompanyLocationId.ToString();
                        //}
                        var tokenWithExpiration = CommonFunction.GenerateJSONWebToken(
                            user.Id,
                            user.UserName,
                            user.UserProfile.Role.Name,
                            _appSettings.jwt.Issuer,
                             _appSettings.jwt.Key, _appSettings.jwt.ExpiryTime,
                            responseData, locationID
                            ).Split(",");

                        resultUser.Token = tokenWithExpiration[0];
                        resultUser.TokenExpiration = Convert.ToDateTime(tokenWithExpiration[1]);
                        return new ApiResponse<ResultUser>
                        {
                            Succeeded = true,
                            ResponseCode = Enums.HttpStatusCode.OK,
                            Message = Constants.Messages.Success,
                            Data = resultUser
                        };
                    }
                }

                return new ApiResponse<ResultUser>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.InvalidLogin,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultUser>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// Update user record against the user id passed.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateUser"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Users, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Users, Constants.PermissionConstants.CodeValue.Update)]
        [HttpPut(Constants.Attrributes.UpdateApiName)]
        public ApiResponse<ResultUpdateUser> PutUser(Guid id, InputUserAndAddresses updateUser)
        {
            try
            {
                _paraList.Add(id);
                _paraList.Add(updateUser);
                if (id != updateUser.InputUser.Id)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<ResultUpdateUser>
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
                    return new ApiResponse<ResultUpdateUser>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.Update,
                        Data = _userService.EditUser(updateUser)
                    };
                }
                else
                {
                    ErrMsg = Constants.Messages.InternalServerError;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<ResultUpdateUser>
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
                return new ApiResponse<ResultUpdateUser>
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
        /// Change user password using old password
        /// </summary>
        /// <param name="inputChangePassword"></param>
        /// <returns></returns>
        //[ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Users, Constants.PermissionConstants.CodeValue.View)]
        //[ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Users, Constants.PermissionConstants.CodeValue.Update)]
        [HttpPost]
        [Route(Constants.Routes.ChangePassword)]
        public ApiResponse<ResultUser> ChangePassword(InputChangePassword inputChangePassword)
        {
            try
            {
                User user = _userService.GetUserByEmail(inputChangePassword.Email, inputChangePassword.CompanyId);
                if (user == null)
                {
                    ErrMsg = Constants.Messages.UserDoesNotExist;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(inputChangePassword), ErrMsg);
                    return new ApiResponse<ResultUser>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.UserDoesNotExist,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                else if (CommonFunction.VerifyPassword(string.Concat(user.PasswordHash, ":", user.PasswordSalt), inputChangePassword.OldPassword))
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(inputChangePassword), ErrMsg);
                    return new ApiResponse<ResultUser>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.PasswordChangedSuccess,
                        ResponseType = Constants.ResponseType.Insert,
                        Data = _userService.ChangePassword(inputChangePassword, user)
                    };
                }
                ErrMsg = Constants.Messages.IncorrectOldPassword;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(inputChangePassword), ErrMsg);
                return new ApiResponse<ResultUser>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.IncorrectOldPassword,
                    ResponseType = Constants.ResponseType.IncorrectOldPassword,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultUser>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    ResponseType = Constants.ResponseType.Error,
                    Data = null
                };
            }
        }


        /// <summary>
        /// Send Forget Password Link
        /// </summary>
        /// <param name="inputForgetPassword"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route(Constants.Routes.ForgotPassword)]
        public ApiResponse<ResultUser> SendForgetPasswordLink(InputForgetPassword inputForgetPassword)
        {
            try
            {
                User user = _userService.GetUserByUserName(inputForgetPassword.Email);
                if (user == null)
                {
                    ErrMsg = Constants.Messages.UserDoesNotExist;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, inputForgetPassword.Email, CommonFunction.returnJsontoString(inputForgetPassword), ErrMsg);
                    return new ApiResponse<ResultUser>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.UserDoesNotExist,
                        Data = null
                    };
                }
                if ((!user.IsActive))
                {
                    ErrMsg = Constants.Messages.UserDisabled;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, inputForgetPassword.Email, CommonFunction.returnJsontoString(inputForgetPassword), ErrMsg);
                    return new ApiResponse<ResultUser>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.UserDisabled,
                        Data = null
                    };
                }
                else
                {
                    string resultData = _userService.ForgetPassword(inputForgetPassword, user);
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, inputForgetPassword.Email, CommonFunction.returnJsontoString(inputForgetPassword), ErrMsg);
                    string baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value}{this.Request.PathBase.Value}/Account/ResetPassword";
                    //string bodymsg = _appSettings.sendingEmailSettings.BodyMessage + "?Email=" + inputForgetPassword.Email + "&OldPassword=" +resultData;
                    string enEmail = CommonFunction.EncodeData(inputForgetPassword.Email);
                    string enPass = CommonFunction.EncodeData(resultData);
                    string bodymsg = String.Format("{0}/{1}?p1={2}&p2={3}&p3={4}", _appSettings.sendingEmailSettings.WebUrl, Constants.WebConstants.Urls.WEB_ResetPassword, enEmail, enPass, user.Id);
                    string message = "Dear User<br/> <br/> We have received password reset request for Leemo User. <br/> <br/>  <a href=" + bodymsg + "> Please Click Here to reset your password</a> <br/><br/> <b> Note: This link is only valid for single use or upto 20 minutes from request of generation, whichever happens first.</b> <br/> <br/> Thanks & Regards <br/> Team Leemo";

                    CommonFunction.sendEmail(_appSettings.sendingEmailSettings.From, inputForgetPassword.Email, message, Constants.EmailConstants.EmailSubjects.ResetPassword, _appSettings.sendingEmailSettings.Password, _appSettings.sendingEmailSettings.Host, _appSettings.sendingEmailSettings.Port, _appSettings.sendingEmailSettings.EnableSsl, _appSettings.sendingEmailSettings.IsBodyHtml, _appSettings.sendingEmailSettings.UseDefaultCredentials, _appSettings.sendingEmailSettings.alias);
                    return new ApiResponse<ResultUser>
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
                return new ApiResponse<ResultUser>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }

        /// <summary>
        /// Reset user password using temp password
        /// </summary>
        /// <param name="inputChangePassword"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route(Constants.Routes.ResetPassword)]
        public ApiResponse<ResultUser> ResetPassword(InputChangePassword inputChangePassword)
        {
            try
            {
                User user = _userService.GetUserByUserName(inputChangePassword.Email);
                if (user == null)
                {
                    ErrMsg = Constants.Messages.UserDoesNotExist;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, inputChangePassword.Email, CommonFunction.returnJsontoString(inputChangePassword), ErrMsg);
                    return new ApiResponse<ResultUser>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.UserDoesNotExist,
                        Data = null
                    };
                }
                else if (CommonFunction.VerifyPassword(string.Concat(user.TempPasswordHash, ":", user.TempPasswordSalt), inputChangePassword.OldPassword) && (user.TempPasswordExpiryDate > DateTime.UtcNow))
                {
                    ResultUser resultData = _userService.ResetPassword(inputChangePassword, user);
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, inputChangePassword.Email, CommonFunction.returnJsontoString(inputChangePassword), ErrMsg);
                    return new ApiResponse<ResultUser>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.PasswordResetSuccess,
                        Data = resultData
                    };
                }
                ErrMsg = Constants.Messages.IncorrectTempPassword;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, inputChangePassword.Email, CommonFunction.returnJsontoString(inputChangePassword), ErrMsg);
                return new ApiResponse<ResultUser>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.IncorrectTempPassword,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultUser>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    Data = null
                };
            }
        }


        /// <summary>
        /// Update user profile image against the user id passed.
        /// </summary>
        /// <param name="updateProfileImage"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Users, Constants.PermissionConstants.CodeValue.View)]
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Users, Constants.PermissionConstants.CodeValue.Update)]
        [HttpPut(Constants.Routes.UpdateProfileImage)]
        public ApiResponse<ResultUpdateUser> UpdateProfileImage(InputUpdateProfileImage updateProfileImage)
        {
            try
            {
                string imageName = _userService.UpdateProfileImage(updateProfileImage);
                if (imageName == null || imageName == "")
                {
                    ErrMsg = Constants.Messages.UserDoesNotExist;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(updateProfileImage), ErrMsg);
                    return new ApiResponse<ResultUpdateUser>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.InternalServerError,
                        Message = Constants.Messages.UserDoesNotExist,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                response = Constants.ApiRequestResponse.ResponseSuccess;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(updateProfileImage), ErrMsg);
                return new ApiResponse<ResultUpdateUser>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.UserProfileImageUpdated,
                    ResponseType = Constants.ResponseType.Update,
                    Data = imageName
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultUpdateUser>
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
        /// Get user method for fetching by email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Users, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Routes.GetUserByEmail)]
        public ApiResponse<ResultUser> GetUserByEmail(string email, Guid CompanyId)
        {
            try
            {
                if (email == null || email == "")
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(email), ErrMsg);
                    return new ApiResponse<ResultUser>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                User userResult = _userService.GetUserByEmail(email, CompanyId);
                if (userResult == null)
                {
                    ErrMsg = Constants.Messages.UserDoesNotExist;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(email), ErrMsg);
                    return new ApiResponse<ResultUser>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.UserDoesNotExist,
                        //ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                response = Constants.ApiRequestResponse.ResponseSuccess;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(email), ErrMsg);
                return new ApiResponse<ResultUser>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.Success,
                    ResponseType = Constants.ResponseType.Insert,
                    Data = userResult
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultUser>
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
        /// Return the list of users
        /// </summary>
        /// <returns></returns>
        //[ActionPermissionFilter(Constants.PermissionConstants.FeatureName.SecurityControls_Roles, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet]
        [Route(Constants.Routes.GetUserFeatures)]
        public PagedResponse<Auth_FeatureListWithGeneralCodeByUserIdResult> GetUserFeatures(Guid userId, Guid CompanyLocationId)
        {
            //ELIMINATE USERS WHOSE PROFILE ARE NULL 
            IEnumerable<Auth_FeatureListWithGeneralCodeByUserIdResult> users = _userService.GetAuth_FeatureListWithGeneralCodeByUserId(userId, CompanyLocationId);

            try
            {
                if (users != null && users.Count() > 0)
                {

                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(userId), ErrMsg);
                    return PagedResponse<Auth_FeatureListWithGeneralCodeByUserIdResult>.PagedList(
                        users,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(userId), ErrMsg);
                    return PagedResponse<Auth_FeatureListWithGeneralCodeByUserIdResult>.PagedList(
                        users,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<Auth_FeatureListWithGeneralCodeByUserIdResult>.PagedList(
                    users,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet(Constants.Routes.GetPersonalUser)]
        public ApiResponse<ResultUserAndAddresses> GetPersonalUser(Guid id, Guid CompanyId, Guid CompanyLocationId)
        {
            try
            {
                ResultUserAndAddresses userResult = new ResultUserAndAddresses();
                if (id == null)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<ResultUserAndAddresses>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                userResult.ResultUser = _userService.GetUser(id, CompanyId, getCompanyLocationId());
                Guid AddressTypeId = _addressTypeService.GetAddressTypeIdWithName(Constants.AddressTypeNames.UserAddress);
                userResult.userAddress = _addressService.GetAddressByReference(id,AddressTypeId);


                if (userResult.ResultUser != null)
                {
                    var ResultuserCompanyId = userResult.ResultUser.UserProfile.CompanyId.ToString();
                    userResult.resultCompany =
                        _companyService.GetCompany(Guid.Parse(ResultuserCompanyId));
                }


                if (userResult != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                    return new ApiResponse<ResultUserAndAddresses>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.Insert,
                        Data = userResult
                    };
                }
                ErrMsg = Constants.Messages.UserDoesNotExist;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(id), ErrMsg);
                return new ApiResponse<ResultUserAndAddresses>
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
                return new ApiResponse<ResultUserAndAddresses>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.BadRequest,
                    Message = Constants.Messages.Failed,
                    Data = null,
                    ResponseType = Constants.ResponseType.Error,
                };
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut(Constants.Routes.UpdatePersonalUser)]
        public ApiResponse<ResultUpdateUser> UpdatePersonalUser(Guid id, InputUserAndAddresses updateUser)
        {
            try
            {
                _paraList.Add(id);
                _paraList.Add(updateUser);
                if (id != updateUser.InputUser.Id)
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<ResultUpdateUser>
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
                    return new ApiResponse<ResultUpdateUser>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.Update,
                        Data = _userService.EditUser(updateUser)
                    };
                }
                else
                {
                    ErrMsg = Constants.Messages.InternalServerError;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<ResultUpdateUser>
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
                return new ApiResponse<ResultUpdateUser>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    ResponseType = Constants.ResponseType.Error,
                    Data = null
                };
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut(Constants.Routes.UpdatePersonalProfileImage)]
        public ApiResponse<ResultUpdateUser> UpdatePersonalProfileImage(InputUpdateProfileImage updateProfileImage)
        {
            try
            {
                string imageName = _userService.UpdateProfileImage(updateProfileImage);
                if (imageName == null || imageName == "")
                {
                    ErrMsg = Constants.Messages.UserDoesNotExist;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(updateProfileImage), ErrMsg);
                    return new ApiResponse<ResultUpdateUser>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.InternalServerError,
                        Message = Constants.Messages.UserDoesNotExist,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                response = Constants.ApiRequestResponse.ResponseSuccess;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(updateProfileImage), ErrMsg);
                return new ApiResponse<ResultUpdateUser>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.UserProfileImageUpdated,
                    ResponseType = Constants.ResponseType.Update,
                    Data = imageName
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<ResultUpdateUser>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    ResponseType = Constants.ResponseType.Error,
                    Data = null
                };
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route(Constants.Routes.GetUsersPersonal)]
        public PagedResponse<ResultUser> GetUsersPersonal([FromQuery] PaginationFilter filter, Guid CompanyLocationId)
        {
            //ELIMINATE USERS WHOSE PROFILE ARE NULL 
            IEnumerable<ResultUser> users = _companyLocationUserMappingService.GetUsersWithLocation(CompanyLocationId).Where(x => x.UserProfile != null).OrderByDescending(x => x.UserProfile.ModifiedOn != null ? x.UserProfile.ModifiedOn : x.UserProfile.CreatedOn);

            try
            {
                if (users != null && users.Count() > 0)
                {
                    //CHECK FOR ACTIVE AND ALL USERS BESED ON PAGINATION PARAMETER
                    if (filter.GetActiveUsrs == 1)
                        users = users.Where(x => x.IsActive);
                    if (filter.GetActiveUsrs == 2)
                        users = users.Where(x => !(x.IsActive));

                    //FUNCTION FOR SEARCH USER BY FULLNAME OR EMAIL
                    //Search Parameter [With null check]  
                    if (!string.IsNullOrEmpty(filter.QuerySearch))
                    {
                        filter.QuerySearch = filter.QuerySearch.ToLower();
                        users = users.Where(a => (a.UserName.ToLower().Contains(filter.QuerySearch) || String.Format("{0} {1}", a.UserProfile == null ? "" : a.UserProfile.FirstName, a.UserProfile == null ? "" : a.UserProfile.LastName).ToLower().Contains(filter.QuerySearch)));

                    }
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(filter), ErrMsg);
                    return PagedResponse<ResultUser>.PagedList(
                        users,
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
                    return PagedResponse<ResultUser>.PagedList(
                        users,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK,
                        filter.PageNumber,
                        filter.PageSize);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<ResultUser>.PagedList(
                    users,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError,
                    filter.PageNumber,
                    filter.PageSize);
            }
        }


        [HttpPost(Constants.Routes.InsertCompanyLocationUserMapping)]
        public ApiResponse<CompanyLocationUserMapping> InsertCompanyLocationUserMapping(InputCompanyLocationUserMapping inputData)
        {
            try
            {
                CompanyLocationUserMapping companyLocationUserMapping = _companyLocationUserMappingService.GetCompanyLocationUserMapping(inputData.CompanyLocationId, inputData.UserId);
                if (companyLocationUserMapping == null)
                {
                    CompanyLocationUserMapping resultData = _companyLocationUserMappingService.Insert(inputData);
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(inputData), ErrMsg);
                    return new ApiResponse<CompanyLocationUserMapping>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.Insert,
                        Data = resultData
                    };
                }
                else
                {
                    ErrMsg = Constants.Messages.DataAlreadyExists;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(inputData), ErrMsg);
                    return new ApiResponse<CompanyLocationUserMapping>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.DataAlreadyExists,
                        ResponseType = Constants.ResponseType.AlreadyExists,
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<CompanyLocationUserMapping>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    ResponseType = Constants.ResponseType.Error,
                    Data = null
                };
            }
        }

        [HttpPut(Constants.Routes.UpdateCompanyLocationUserMapping)]
        public ApiResponse<CompanyLocationUserMapping> UpdateCompanyLocationUserMapping(InsertUpdateCompanyLocationUserMapping inputData)
        {
            try
            {
                if (inputData != null)
                {
                    CompanyLocationUserMapping resultData = _companyLocationUserMappingService.Update(inputData);
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(inputData), ErrMsg);
                    return new ApiResponse<CompanyLocationUserMapping>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        ResponseType = Constants.ResponseType.Insert,
                        Data = resultData
                    };
                }
                else
                {
                    ErrMsg = Constants.Messages.RecordNotFound;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(inputData), ErrMsg);
                    return new ApiResponse<CompanyLocationUserMapping>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.RecordNotFound,
                        ResponseType = Constants.ResponseType.AlreadyExists,
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<CompanyLocationUserMapping>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    ResponseType = Constants.ResponseType.Error,
                    Data = null
                };
            }
        }

        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Users, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet]
        [Route(Constants.Routes.GetUsersWithCompanyLocation)]
        public PagedResponse<ResultUser> GetUsersWithCompanyLocation([FromQuery] PaginationFilter filter, Guid companyLocationId)
        {
            List<ResultUser> users = _companyLocationUserMappingService.GetUsersWithLocation(companyLocationId).ToList();
            try
            {
                if (users != null && users.Count() > 0)
                {
                    //GET ACTIVE,INACTIVE,ALL USERS
                    users.ForEach(x => {
                        x.TotalUsers = users.Count();
                        x.ActiveUsers = users.Where(y => y.IsActive == true).Count();
                    });

                    //ELIMINATE USERS WHOSE PROFILE ARE NULL
                    users = users.Where(x => x.UserProfile != null).OrderByDescending(x => x.UserProfile.ModifiedOn != null ? x.UserProfile.ModifiedOn : x.UserProfile.CreatedOn).ToList();
                    //CHECK FOR ACTIVE AND ALL USERS BESED ON PAGINATION PARAMETER
                    if (filter.GetActiveUsrs == 1)
                        users = users.Where(x => x.IsActive).ToList();
                    if (filter.GetActiveUsrs == 2)
                        users = users.Where(x => !(x.IsActive)).ToList();

                    //FUNCTION FOR SEARCH USER BY FULLNAME OR EMAIL
                    //Search Parameter [With null check]  
                    if (!string.IsNullOrEmpty(filter.QuerySearch))
                    {
                        filter.QuerySearch = filter.QuerySearch.ToLower();
                        users = users.Where(a => (a.UserName.ToLower().Contains(filter.QuerySearch) || String.Format("{0} {1}", a.UserProfile == null ? "" : a.UserProfile.FirstName, a.UserProfile == null ? "" : a.UserProfile.LastName).ToLower().Contains(filter.QuerySearch))).ToList();

                    }
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(companyLocationId), ErrMsg);
                    return PagedResponse<ResultUser>.PagedList(
                        users,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK,
                        filter.PageNumber,
                        filter.PageSize);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(companyLocationId), ErrMsg);
                    return PagedResponse<ResultUser>.PagedList(
                        users,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK,
                        filter.PageNumber,
                        filter.PageSize);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<ResultUser>.PagedList(
                     users,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError,
                    filter.PageNumber,
                    filter.PageSize);
            }
        }

        [HttpGet]
        [Route(Constants.Routes.GetCompanyLocationWithUserID)]
        public PagedResponse<CompanyLocation> GetCompanyLocationWithUserID(Guid userID)
        {
            IEnumerable<CompanyLocation> users = _companyLocationUserMappingService.GetCompanyLocationWithUserId(userID);
            try
            {
                if (users != null && users.Count() > 0)
                {

                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(userID), ErrMsg);
                    return PagedResponse<CompanyLocation>.PagedList(
                        users,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(userID), ErrMsg);
                    return PagedResponse<CompanyLocation>.PagedList(
                        users,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<CompanyLocation>.PagedList(
                    users,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }


        [HttpGet]
        [Route(Constants.Routes.GetUserByEmailAndCompanyLocationAndCompanyId)]
        public ApiResponse<Dictionary<string, string>> GetUserByEmailAndCompanyLocationAndCompanyId(string email, Guid companyid, Guid companyLocationId)
        {
            Dictionary<string, string> users = _userService.GetUserByEmailAndCompanyLocation(email, companyid, companyLocationId);
            _paraList.Add(email);
            _paraList.Add(companyid);
            _paraList.Add(companyLocationId);

            try
            {
                if (users != null && users.Count() > 0)
                {

                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<Dictionary<string, string>>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = users
                    };
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<Dictionary<string, string>>
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
                return new ApiResponse<Dictionary<string, string>>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.InternalServerError,
                    ResponseType = Constants.ResponseType.Error,
                    Data = null
                };
            }
        }



        [HttpGet]
        [Route(Constants.Routes.GetCompanyUsersExceptCurrentCompanyLocation)]
        public PagedResponse<ResultUserByEmailandCompanyID> GetCompanyUsersExceptCurrentCompanyLocation(string email, Guid companyid, Guid companyLocationId)
        {
            IEnumerable<ResultUserByEmailandCompanyID> users = _userService.GetCompanyUsersExceptCurrentCompanyLocation(email, companyid, companyLocationId);
            _paraList.Add(email);
            _paraList.Add(companyid);
            _paraList.Add(companyLocationId);
            try
            {
                if (users != null && users.Count() > 0)
                {

                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<ResultUserByEmailandCompanyID>.PagedList(
                        users,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return PagedResponse<ResultUserByEmailandCompanyID>.PagedList(
                        users,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<ResultUserByEmailandCompanyID>.PagedList(
                    users,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }


        [HttpGet]
        [Route(Constants.Routes.GetExistingUserData)]
        public ApiResponse<InputUser> GetExistingUserData(string UserId, string CompanyLocationId)
        {
            InputUser users = _userService.GetExistingUserData(Guid.Parse(UserId), Guid.Parse(CompanyLocationId));
            _paraList.Add(UserId);
            _paraList.Add(CompanyLocationId);
            try
            {
                if (users != null)
                {

                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<InputUser>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.Success,
                        Data = users
                    };
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(_paraList), ErrMsg);
                    return new ApiResponse<InputUser>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.NotFound,
                        Message = Constants.Messages.Failed,
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<InputUser>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.Failed,
                    Data = null
                };
            }
        }


        [ActionPermissionFilter(Constants.PermissionConstants.FeatureName.Users, Constants.PermissionConstants.CodeValue.View)]
        [HttpGet(Constants.Routes.GetUserCountsByLocation)]
        public ApiResponse<Dictionary<string, int>> GetUserCounts(Guid companyLocationid)
        {
            try
            {
                var resultUser = _userService.GetUserCounts(companyLocationid);
                if (resultUser != null)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(),
                        CommonFunction.returnJsontoString(companyLocationid), ErrMsg);
                    return new ApiResponse<Dictionary<string, int>>
                    {
                        Succeeded = true,
                        ResponseCode = Enums.HttpStatusCode.OK,
                        Message = Constants.Messages.GroupImageUpdated,
                        Data = resultUser
                    };
                }
                ErrMsg = Constants.Messages.UserDoesNotExist;
                _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(),
                    CommonFunction.returnJsontoString(companyLocationid), ErrMsg);
                return new ApiResponse<Dictionary<string, int>>
                {
                    Succeeded = false,
                    ResponseCode = Enums.HttpStatusCode.InternalServerError,
                    Message = Constants.Messages.UserDoesNotExist,
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

        [ApiExplorerSettings(IgnoreApi = true)]
        [AllowAnonymous]
        [HttpGet(Constants.Routes.GetUserByOnlyEmail)]
        public ApiResponse<User> GetUserByOnlyEmail(string email)
        {
            try
            {
                if (email == null || email == "")
                {
                    ErrMsg = Constants.Messages.BadRequest;
                    //_commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(email), ErrMsg);
                    return new ApiResponse<User>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.BadRequest,
                        ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                User userResult = _userService.GetUserByUserName(email);
                if (userResult == null)
                {
                    ErrMsg = Constants.Messages.UserDoesNotExist;
                    //_commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(email), ErrMsg);
                    return new ApiResponse<User>
                    {
                        Succeeded = false,
                        ResponseCode = Enums.HttpStatusCode.BadRequest,
                        Message = Constants.Messages.UserDoesNotExist,
                        //ResponseType = Constants.ResponseType.Error,
                        Data = null
                    };
                }
                response = Constants.ApiRequestResponse.ResponseSuccess;
                //_commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(email), ErrMsg);
                return new ApiResponse<User>
                {
                    Succeeded = true,
                    ResponseCode = Enums.HttpStatusCode.OK,
                    Message = Constants.Messages.Success,
                    ResponseType = Constants.ResponseType.Insert,
                    Data = userResult
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<User>
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