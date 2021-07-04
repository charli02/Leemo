using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Model.WrapperModels;
using Leemo.Web.Filters;
using Leemo.Web.HttpClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TPSS.Common;

namespace Leemo.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly SessionManager _sessionManager;

        public UserController(IOptions<AppSettings> appSettings, SessionManager sessionManager)
        {
            _appSettings = appSettings.Value;
            _sessionManager = sessionManager;
        }
        [ActionPermissionFilter]
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                Guid companyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
                var model = new GroupsAndUsers();
                List<Auth_FeatureListWithGeneralCodeByUserIdResult> PermissionData = (List<Auth_FeatureListWithGeneralCodeByUserIdResult>)HttpContext.Items["PermissionData"];
                if (PermissionData != null && PermissionData.Count > 0)
                {
                    var RoleName = PermissionData[0].RoleName;
                    var addPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.Users && x.CodeValue == Constants.PermissionConstants.CodeValue.Add).ToList();
                    var viewPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.Users && x.CodeValue == Constants.PermissionConstants.CodeValue.View).ToList();
                    if (addPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        ViewBag.addUserPermission = true;
                    }
                    if (viewPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        ViewBag.viewUserPermission = true;
                        //var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}?pagenumber={3}&pagesize={4}", _appSettings.Leemo_API_Config.BaseUrl,
                        //Constants.WebConstants.Urls.API_GetAllUsersWithLocation, companyLocationId, _appSettings.PageSettings.DefaultPageNumber, _appSettings.PageSettings.DefaultPageSize), _sessionManager.BearerToken);
                        //var result = response.ContentAsType<List<ResultUser>>();


                        var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                            Constants.WebConstants.Urls.API_GetUsersCounts, companyLocationId), _sessionManager.BearerToken);

                        var result1 = response.ContentAsType<Dictionary<string, int>>();
                        ViewBag.AllUsers = Convert.ToInt32(result1["data"].All);
                        ViewBag.ActiveUsers = Convert.ToInt32(result1["data"].Active);
                        ViewBag.InActiveUsers = Convert.ToInt32(result1["data"].InActive);

                        //var result = response.ContentAsType<List<Group>>();
                        //model.ResultGroup = result[Constants.WebConstants.Data].ToObject<List<ResultUser>>();

                        //if (result != null)
                        //{
                        //    model.ResultUser = result[Constants.WebConstants.Data].ToObject<List<ResultUser>>();
                        //    ViewBag for Show active and all User
                        //    if (model.ResultUser.Count() > 0)
                        //        {
                        //            ViewBag.AllUsers = model.ResultUser.FirstOrDefault().TotalUsers;
                        //            ViewBag.ActiveUsers = model.ResultUser.FirstOrDefault().ActiveUsers;
                        //        }
                        //        else
                        //        {
                        //            ViewBag.AllUsers = 0;
                        //        }
                        return View(model);
                        //}

                    }

                    //return View(model);
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", Constants.WebConstants.Controllers.Account);
            }
            return View();
        }
        [ActionPermissionFilter]
        public async Task<IActionResult> UserList(string QuerySearch = "", int GetActiveUsrs = 1)
        {
            Guid companyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}?PageNumber={3}&PageSize={4}&GetActiveUsrs={5}&QuerySearch={6}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_GetAllUsersWithLocation, companyLocationId, _appSettings.PageSettings.DefaultPageNumber, _appSettings.PageSettings.DefaultPageSize, GetActiveUsrs, QuerySearch), _sessionManager.BearerToken);

            var result = response.ContentAsType<List<ResultUser>>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            //var response1 = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}?PageSize={3}&GetActiveUsrs={4}", _appSettings.Leemo_API_Config.BaseUrl,
            //                Constants.WebConstants.Urls.API_GetAllUsersWithLocation, _sessionManager.CompanyLocationID, 1000, 0), _sessionManager.BearerToken);

            var response1 = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                  Constants.WebConstants.Urls.API_GetUsersCounts, companyLocationId), _sessionManager.BearerToken);

            var result1 = response1.ContentAsType<Dictionary<string, int>>();
            if (result1 != null)
            {
                ViewBag.AllUsers = Convert.ToInt32(result1["data"].All);
                ViewBag.ActiveUsers = Convert.ToInt32(result1["data"].Active);
                ViewBag.InActiveUsers = Convert.ToInt32(result1["data"].InActive);
            }

            //var result1 = response1.ContentAsType<List<ResultUser>>();
            //if (result1 != null)
            //{
            //    List<ResultUser> filterResult = result1[Constants.WebConstants.Data].ToObject<List<ResultUser>>();
            //    ViewBag.ActiveUsers = filterResult.Where(x => x.IsActive == true).Count();
            //    ViewBag.inActiveUsers = filterResult.Where(x => x.IsActive == false).Count();
            //    ViewBag.AllUsers = filterResult.Count();
            //}

            if (result != null)
            {
                return PartialView(Constants.WebConstants.PartialViews.UserList, result[Constants.WebConstants.Data].ToObject<List<ResultUser>>());
            }
            return PartialView(Constants.WebConstants.PartialViews.UserList, null);
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> UserDetails(Guid id)
        {
            List<Auth_FeatureListWithGeneralCodeByUserIdResult> PermissionData = (List<Auth_FeatureListWithGeneralCodeByUserIdResult>)HttpContext.Items["PermissionData"];
            if (PermissionData != null && PermissionData.Count > 0)
            {
                var RoleName = PermissionData[0].RoleName;
                var editPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.Users && x.CodeValue == Constants.PermissionConstants.CodeValue.Update).ToList();
                var viewPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.Users && x.CodeValue == Constants.PermissionConstants.CodeValue.View).ToList();
                if (editPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    ViewBag.editUserPermission = true;
                }
                if (viewPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}?CompanyId={3}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetUser, id, _sessionManager.CompanyId), _sessionManager.BearerToken);
                    var result = response.ContentAsType<ResultUserAndAddresses>();
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                    if (RoleName.ToLower() != Constants.WebConstants.Owner)
                    {
                        var resultdata = result[Constants.WebConstants.Data].ToObject<ResultUserAndAddresses>();
                        string rolename = resultdata.ResultUser.Auth_Roles[0].Name;
                        if (rolename.ToLower() == Constants.WebConstants.Owner)
                        {
                            ViewBag.editUserPermission = false;
                        }
                    }
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                    if (result != null)
                    {
                        ViewBag.returnPageType = Constants.WebConstants.UserSettings;
                        return PartialView(Constants.WebConstants.PartialViews.UserDetails, result[Constants.WebConstants.Data].ToObject<ResultUserAndAddresses>());
                    }
                }
            }
            return Json(Constants.ResponseType.AccessDenied);
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> CreateUser()
        {
            Guid companyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}?PageNumber={2}&PageSize={3}&CompanyLocationId={4}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_GetAllPersonalRoles, _appSettings.PageSettings.DefaultPageNumber, _appSettings.PageSettings.DefaultPageSize, _sessionManager.CompanyLocationID), _sessionManager.BearerToken);

            var roles = response.ContentAsType<List<Auth_Role>>();
            if (roles[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(roles[Constants.WebConstants.ResponseType]);
            }
            if (roles[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
            {
                return Json(roles[Constants.WebConstants.ResponseType]);
            }
            var response1 = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_GetPersonalDesignations, companyLocationId), _sessionManager.BearerToken);

            var designations = response1.ContentAsType<List<Designation>>();
            if (designations[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(designations[Constants.WebConstants.ResponseType]);
            }
            if (designations[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
            {
                return Json(designations[Constants.WebConstants.ResponseType]);
            }
            if (roles != null && designations != null)
            {
                ViewBag.designations = designations[Constants.WebConstants.Data].ToObject<List<Designation>>();
                ViewBag.roles = roles[Constants.WebConstants.Data].ToObject<List<Auth_Role>>();
                InputUser model = new InputUser();

                return PartialView(Constants.WebConstants.PartialViews.CreateUser, model);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionPermissionFilter]
        public async Task<IActionResult> CreateUser(InputUser inputUser)
        {
            if (ModelState.IsValid)
            {
                inputUser.userProfile.CompanyId = Guid.Parse(_sessionManager.CompanyId);
                inputUser.CompanyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
                var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_PostUser), inputUser, _sessionManager.BearerToken);


                var result = response.ContentAsType<ResultUser>();

                if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error || result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(result[Constants.WebConstants.ResponseType]);
                }
                if (result != null)
                {
                    return Json(result[Constants.WebConstants.ResponseType]);
                }
            }
            var item = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => x.Key).ToList();
            return Redirect(Constants.WebConstants.Urls.WEB_UserIndex);
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> EditUser(Guid id, string returnPageType)
        {
            try
            {
                string userapi = Constants.WebConstants.Urls.API_GetUser;
                string roleapi = Constants.WebConstants.Urls.API_GetAllRoles;
                string designationapi = Constants.WebConstants.Urls.API_GetAllDesignationsByLocation;
                Guid companyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
                if (returnPageType == Constants.WebConstants.PersonalSettings)
                {
                    userapi = Constants.WebConstants.Urls.API_GetPersonalUser;
                    roleapi = Constants.WebConstants.Urls.API_GetAllPersonalRoles;
                    designationapi = Constants.WebConstants.Urls.API_GetPersonalDesignations;
                }
                var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}?CompanyId={3}", _appSettings.Leemo_API_Config.BaseUrl,
                    userapi, id, _sessionManager.CompanyId), _sessionManager.BearerToken);

                var user = response.ContentAsType<ResultUser>();
                if (user[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(user[Constants.WebConstants.ResponseType]);
                }
                if (user[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
                {
                    return Json(user[Constants.WebConstants.ResponseType]);
                }

                var response1 = await HttpRequestFactory.Get(string.Format("{0}{1}?PageNumber={2}&PageSize={3}&CompanyLocationId={4}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetAllPersonalRoles, _appSettings.PageSettings.DefaultPageNumber, _appSettings.PageSettings.DefaultPageSize, _sessionManager.CompanyLocationID), _sessionManager.BearerToken);
                var profiles = response1.ContentAsType<List<Auth_Role>>();
                if (profiles[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(profiles[Constants.WebConstants.ResponseType]);
                }
                if (profiles[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
                {
                    return Json(user[Constants.WebConstants.ResponseType]);
                }

                var response2 = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetPersonalDesignations, companyLocationId), _sessionManager.BearerToken);

                var roles = response2.ContentAsType<List<Designation>>();
                if (roles[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(roles[Constants.WebConstants.ResponseType]);
                }
                if (roles[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
                {
                    return Json(user[Constants.WebConstants.ResponseType]);
                }


                if (profiles != null && roles != null && user != null)
                {
                    InputUserAndAddresses model = new InputUserAndAddresses();
                    model.InputUser = new InputUpdateUser();
                    model.InputAddress = new InputAddress();
                    ViewBag.roles = roles[Constants.WebConstants.Data].ToObject<List<Designation>>();
                    ViewBag.profiles = profiles[Constants.WebConstants.Data].ToObject<List<Auth_Role>>();


                    ResultUserAndAddresses currentUser = user[Constants.WebConstants.Data].ToObject<ResultUserAndAddresses>();
                    model.InputUser.userProfile = new InputUpdateUserProfile();
                    string reportingUserId = Convert.ToString(currentUser.ResultUser.UserProfile.DesignationId);

                    ViewBag.reportingUser = await getParentRoleByRoleId(reportingUserId, returnPageType, id.ToString());

                    if (currentUser.ResultUser.UserProfile.Mobile == "")
                    {
                        model.InputUser.userProfile.MobileNumber = null;

                    }
                    else if ((currentUser.ResultUser.UserProfile.Mobile).Contains('-'))
                    {
                        var Mobile = currentUser.ResultUser.UserProfile.Mobile.Trim().Split("-");
                        model.InputUser.userProfile.MobileNumber = Mobile[1].Trim();

                    }
                    else
                    {

                        model.InputUser.userProfile.MobileNumber = currentUser.ResultUser.UserProfile.Mobile;
                    }


                    model.InputUser.Id = currentUser.ResultUser.Id;
                    model.InputUser.UserName = currentUser.ResultUser.UserName;
                    model.InputUser.IsActive = currentUser.ResultUser.IsActive;
                    model.isUserCurrentBaseLocation = currentUser.ResultUser.isUserCurrentBaseLocation;

                    if (currentUser.ResultUser.Auth_Roles != null)
                    {
                        model.InputUser.profiles = currentUser.ResultUser.Auth_Roles.Select(x => x.Id).ToList();
                        ViewBag.RoleName = currentUser.ResultUser.Auth_Roles.Select(x => x.Name).FirstOrDefault();
                    }

                    if (currentUser.ResultUser.UserProfile.CountryCode != null)
                    {
                        model.InputUser.userProfile.CountryCode = currentUser.ResultUser.UserProfile.CountryCode;
                    }


                    if (currentUser.ResultUser.UserProfile != null)
                    {
                        model.InputUser.userProfile.FirstName = currentUser.ResultUser.UserProfile.FirstName;
                        model.InputUser.userProfile.LastName = currentUser.ResultUser.UserProfile.LastName;
                        model.InputUser.userProfile.DesignationId = currentUser.ResultUser.UserProfile.DesignationId;
                        model.InputUser.userProfile.DesignaionName = currentUser.ResultUser.Designation.Name;
                        if (currentUser.ResultUser.UserProfile.ReportingToUserId == null)
                        {
                            model.InputUser.userProfile.ReportingToUserId = Guid.Empty;
                        }
                        else
                        {
                            model.InputUser.userProfile.ReportingToUserId = currentUser.ResultUser.UserProfile.ReportingToUserId;
                        }
                        model.InputUser.userProfile.Description = currentUser.ResultUser.UserProfile.Description;
                        if (Convert.ToString(currentUser.ResultUser.UserProfile.DateOfBirth.Date) != Convert.ToString(default(DateTime).Date))
                        {
                            model.InputUser.userProfile.DOBDay = Convert.ToString(currentUser.ResultUser.UserProfile.DateOfBirth.Day);
                            model.InputUser.userProfile.DOBMonth = Convert.ToString(currentUser.ResultUser.UserProfile.DateOfBirth.Month);
                            model.InputUser.userProfile.DOBYear = Convert.ToString(currentUser.ResultUser.UserProfile.DateOfBirth.Year);
                        }

                        model.InputUser.userProfile.Alias = currentUser.ResultUser.UserProfile.Alias;
                        model.InputUser.userProfile.Phone = currentUser.ResultUser.UserProfile.Phone;
                        model.InputUser.userProfile.Website = currentUser.ResultUser.UserProfile.Website;
                        model.InputUser.userProfile.Fax = currentUser.ResultUser.UserProfile.Fax;
                        model.InputUser.userProfile.Language = currentUser.ResultUser.UserProfile.Language;
                        model.InputUser.userProfile.CountryLocale = currentUser.ResultUser.UserProfile.CountryLocale;
                        model.InputUser.userProfile.DateFormat = currentUser.ResultUser.UserProfile.DateFormat;
                        model.InputUser.userProfile.TimeFormat = currentUser.ResultUser.UserProfile.TimeFormat;
                        model.InputUser.userProfile.TimeZone = currentUser.ResultUser.UserProfile.TimeZone;

                        if (currentUser.ResultUser.UserProfile.CompanyId != null)
                            model.InputUser.userProfile.CompanyId = (Guid)currentUser.ResultUser.UserProfile.CompanyId;
                    }

                    if (currentUser.userAddress != null)
                    {
                        model.InputAddress.Id = currentUser.userAddress.Id;
                        model.InputAddress.ReferenceId = (Guid)currentUser.userAddress.ReferenceId;
                        model.InputAddress.AddressTypeId = (Guid)currentUser.userAddress.AddressTypeId;
                        model.InputAddress.Street = currentUser.userAddress.Street;
                        model.InputAddress.City = currentUser.userAddress.City;
                        model.InputAddress.State = currentUser.userAddress.State;
                        model.InputAddress.ZipCode = currentUser.userAddress.ZipCode;
                        model.InputAddress.Country = currentUser.userAddress.Country;
                        model.InputAddress.CreatedOn = currentUser.userAddress.CreatedOn;
                        model.InputAddress.ModifiedOn = currentUser.userAddress.ModifiedOn;
                        model.InputAddress.AddressLine1 = currentUser.userAddress.AddressLine1;
                    }
                    ViewBag.returnPageType = returnPageType;
                    return PartialView(Constants.WebConstants.PartialViews.EditUser, model);
                }
                return RedirectToAction(Constants.WebConstants.Actions.Index);
            }
            catch (Exception ex)
            {
                return RedirectToAction(Constants.WebConstants.Actions.Index);
            }
        }

        [ActionPermissionFilter]
        [HttpPost]
        public async Task<IActionResult> EditUser(InputUserAndAddresses updateUser)
        {
            try
            {
                updateUser.CompanyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
                string editApi = Constants.WebConstants.Urls.API_PutUser;
                if (updateUser.InputUser.returnFrom == Constants.WebConstants.PersonalSettings)
                {
                    editApi = Constants.WebConstants.Urls.API_EditPersonalUser;
                }
                updateUser.InputAddress.ReferenceId = updateUser.InputUser.Id;

                updateUser.InputUser.userProfile.Mobile = updateUser.InputUser.userProfile.CountryCodeNumber + "-" + updateUser.InputUser.userProfile.MobileNumber;
                if (!(String.IsNullOrEmpty(updateUser.InputUser.userProfile.DOBDay) && String.IsNullOrEmpty(updateUser.InputUser.userProfile.DOBDay) && String.IsNullOrEmpty(updateUser.InputUser.userProfile.DOBDay)))
                {
                    //updateUser.InputUser.userProfile.DateOfBirth = Convert.ToDateTime(string.Format("{0}/{1}/{2}", updateUser.InputUser.userProfile.DOBDay, updateUser.InputUser.userProfile.DOBMonth, updateUser.InputUser.userProfile.DOBYear));


                    if (updateUser.InputUser.userProfile.DOBMonth.Length < 2)
                    {
                        updateUser.InputUser.userProfile.DOBMonth = "0" + updateUser.InputUser.userProfile.DOBMonth;
                    }
                    if (updateUser.InputUser.userProfile.DOBDay.Length < 2)
                    {
                        updateUser.InputUser.userProfile.DOBDay = "0" + updateUser.InputUser.userProfile.DOBDay;
                    }
                    bool data = checkValidDate(int.Parse(updateUser.InputUser.userProfile.DOBYear), int.Parse(updateUser.InputUser.userProfile.DOBMonth), int.Parse(updateUser.InputUser.userProfile.DOBDay)).Result;

                    // var stringDate = string.Format("{0}/{1}/{2}", updateUser.InputUser.userProfile.DOBDay, updateUser.InputUser.userProfile.DOBMonth, updateUser.InputUser.userProfile.DOBYear);


                    if (data == true)
                    {
                        updateUser.InputUser.userProfile.DateOfBirth = new DateTime(int.Parse(updateUser.InputUser.userProfile.DOBYear), int.Parse(updateUser.InputUser.userProfile.DOBMonth), int.Parse(updateUser.InputUser.userProfile.DOBDay));
                    }
                    else
                    {
                        return Json("3");
                    }

                }

                if (ModelState.IsValid)
                {
                    var response = await HttpRequestFactory.Put(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                    editApi, updateUser.InputUser.Id), updateUser, _sessionManager.BearerToken);
                    var result = response.ContentAsType<ResultUpdateUser>();
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }

                    if (result != null)
                    {
                        if (result[Constants.WebConstants.Data] != null)
                        {
                            if (Convert.ToString(updateUser.InputUser.Id) == _sessionManager.ID)
                            {
                                _sessionManager.LoginName = String.Format("{0}&{1}", updateUser.InputUser.userProfile.FirstName, updateUser.InputUser.userProfile.LastName);
                            }

                            if (result[Constants.WebConstants.ResponseType] > 0)
                            {
                                return Json(result[Constants.WebConstants.ResponseType]);
                            }
                            if (updateUser.InputUser.returnFrom == Constants.WebConstants.PersonalSettings)
                            {

                                return Redirect(Constants.WebConstants.Urls.WEB_PersonalDetails);
                            }

                            return Redirect(Constants.WebConstants.Urls.WEB_UserIndex);
                        }
                    }
                }
                if (updateUser.InputUser.returnFrom == Constants.WebConstants.PersonalSettings)
                {
                    return Redirect(Constants.WebConstants.Urls.WEB_PersonalDetails);
                }
                return Redirect(Constants.WebConstants.Urls.WEB_UserIndex);
            }
            catch (Exception ex)
            {
                return Redirect(Constants.WebConstants.Urls.WEB_UserIndex);
            }
        }

        [ActionPermissionFilter]
        public async Task<Boolean> checkValidDate(int year, int month, int day)
        {
            try
            {
                var dates = new DateTime(year, month, day);
                return true;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return false;
            }
        }
        public async Task<IActionResult> Details()
        {
            try
            {
                var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}?CompanyId={3}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetPersonalUser, _sessionManager.ID, _sessionManager.CompanyId), _sessionManager.BearerToken);
                var result = response.ContentAsType<ResultUserAndAddresses>();
                if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(result[Constants.WebConstants.ResponseType]);
                }
                if (result != null)
                {
                    ViewBag.returnPageType = Constants.WebConstants.PersonalSettings;
                    ViewBag.MaxImageSize = _appSettings.MaxImageSize;
                    return View(result[Constants.WebConstants.Data].ToObject<ResultUserAndAddresses>());
                }
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> DetailsPersonal()
        {
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}?CompanyId={3}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_GetPersonalUser, _sessionManager.ID, _sessionManager.CompanyId), _sessionManager.BearerToken);
            var result = response.ContentAsType<ResultUserAndAddresses>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            if (result != null)
            {
                ViewBag.returnPageType = Constants.WebConstants.PersonalSettings;
                return PartialView(Constants.WebConstants.PartialViews.PersonalDetails, result[Constants.WebConstants.Data].ToObject<ResultUserAndAddresses>());
            }
            return View();
        }

        [HttpPost]
        [ActionPermissionFilter]
        public async Task<object> UploadProfileImage(Leemo.Web.Models.Common.ImageUpload model, string returnPageType)
        {
            string uploadApi = Constants.WebConstants.Urls.API_User_UpdateProfileImage;
            if (model.EditImageModalTarget == "#uploadPersonalImageModal")
            {
                uploadApi = Constants.WebConstants.Urls.API_UpdatePersonalProfileImage;
            }
            string message = "";
            if (model.Id == Guid.Empty)
                return message;

            if (model.ImageFile != null)
            {
                long FileSize = model.ImageFile.Length;
                if (FileSize < _appSettings.MaxImageSize)
                {
                    InputUpdateProfileImage userImageinput = new InputUpdateProfileImage();
                    string filepath = string.Concat(_appSettings.Resources_BaseDir, _appSettings.ProfileImagesPath);
                    string filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    string extension = Path.GetExtension(model.ImageFile.FileName).ToLower();
                    filename = filename + DateTime.Now.ToString("yyssmmfff") + extension;
                    if (extension == Constants.Extensions.PNG || extension == Constants.Extensions.JPG || extension == Constants.Extensions.JPEG)
                    {
                        //API call for updating Image name in UserProfile
                        userImageinput.UserId = model.Id;
                        userImageinput.ImageName = filename;
                        userImageinput.CompanyId = Guid.Parse(_sessionManager.CompanyId);
                        var response = await HttpRequestFactory.Put(
                            string.Concat(_appSettings.Leemo_API_Config.BaseUrl,
                            uploadApi), userImageinput, _sessionManager.BearerToken);
                        var result = response.ContentAsType<InputUpdateProfileImage>();
                        if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                        {
                            return Json(result[Constants.WebConstants.ResponseType]);
                        }
                        if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
                        {
                            return Json(result[Constants.WebConstants.ResponseType]);
                        }

                        //Saving Image in Physical Path /Resources/ProfileImages/
                        string path = Path.Combine(filepath, filename);
                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            model.ImageFile.CopyTo(filestream);
                        }

                        if (model.EditImageModalTarget == "#uploadPersonalImageModal" || Convert.ToString(model.Id) == _sessionManager.ID)
                        {
                            _sessionManager.UserProfileImage = filename;
                        }
                        message = Constants.Messages.UserProfileImageUpdated;
                        return filename;
                    }
                    else
                    {
                        message = "0";
                    }
                }
                else
                {
                    message = "1";
                }
            }
            return message;
        }

        [ActionPermissionFilter]
        public async Task<string> GetUserEmail(string Email)
        {
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}?email={2}&companyid={3}&companyLocationId={4}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_GetUserByEmailAndCompanyLocationAndCompanyId, Email, _sessionManager.CompanyId, _sessionManager.CompanyLocationID), _sessionManager.BearerToken);

            var result = response.ContentAsType<ResultUser>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            if (result[Constants.WebConstants.Data] != null)
            {
                //string message = Constants.Messages.UserAlreadyExists;
                var data = result[Constants.WebConstants.Data];
                string str = data["Flag"];
                return str;
            }
            return null;
        }

        [ActionPermissionFilter]
        public async Task<List<ResultGroupUser>> getParentRoleByRoleId(string DesignationId, string returnPageType, string UserId)
        {
            string getapi = Constants.WebConstants.Urls.API_GetDesignationByID;
            if (returnPageType == Constants.WebConstants.PersonalSettings)
            {
                getapi = Constants.WebConstants.Urls.API_GetDesignationByIDPersonal;
            }
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}?CompanyId={3}", _appSettings.Leemo_API_Config.BaseUrl,
              Constants.WebConstants.Urls.API_GetDesignationByIDPersonal, DesignationId, _sessionManager.CompanyId), _sessionManager.BearerToken);

            List<ResultGroupUser> reporingUserList = new List<ResultGroupUser>();

            var result = response.ContentAsType<User>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }

            if (result != null)
            {
                var dataUser = result[Constants.WebConstants.Data];

                if (result[Constants.WebConstants.Data] != null)
                {


                    for (var i = 0; i < dataUser.Count; i++)
                    {
                        var model = new ResultGroupUser();
                        model.UserId = dataUser[i].id;
                        model.UserName = dataUser[i].userName;
                        model.FirstName = dataUser[i].userProfile.firstName;
                        model.LastName = dataUser[i].userProfile.lastName;

                        reporingUserList.Add(model);
                    }
                }
                if (UserId != null && UserId != String.Empty)
                {
                    reporingUserList = reporingUserList.Where(x => x.UserId != Guid.Parse(UserId)).ToList();
                }
                return reporingUserList;
            }

            return reporingUserList;
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> PersonalChangePassword()
        {
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}?CompanyId={3}", _appSettings.Leemo_API_Config.BaseUrl,
               Constants.WebConstants.Urls.API_GetPersonalUser, _sessionManager.ID, _sessionManager.CompanyId), _sessionManager.BearerToken);
            var result = response.ContentAsType<ResultUserAndAddresses>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            ResultUserAndAddresses resultUser = result[Constants.WebConstants.Data].ToObject<ResultUserAndAddresses>();
            if (resultUser != null)
            {
                InputChangePassword model = new InputChangePassword();
                model.Email = CommonFunction.EncodeData(resultUser.ResultUser.UserName);
                return PartialView(Constants.WebConstants.PartialViews.ChangePassword, model);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionPermissionFilter]
        public async Task<IActionResult> PersonalChangePassword(InputChangePassword inputChangePassword)
        {
            if (ModelState.IsValid)
            {
                if (inputChangePassword.NewPassword.ToLower().Trim() == inputChangePassword.ConfirmPassword.ToLower().Trim())
                {
                    inputChangePassword.Email = CommonFunction.DecodeData(inputChangePassword.Email);
                    inputChangePassword.CompanyId = Guid.Parse(_sessionManager.CompanyId);
                    var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                            Constants.WebConstants.Urls.API_ChangePassword), inputChangePassword, _sessionManager.BearerToken);
                    var result = response.ContentAsType<InputChangePassword>();
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }

                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.IncorrectOldPassword)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }

                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.Insert)
                    {
                        InputChangePassword model = new InputChangePassword();
                        return PartialView(Constants.WebConstants.PartialViews.ChangePassword, model);
                    }
                }
                return Json(Constants.ResponseType.Error);
            }
            return Json(Constants.ResponseType.Error);
        }

        [ActionPermissionFilter]
        public async Task<List<ResultUserByEmailandCompanyID>> GetCompanyUsersExceptCurrentCompanyLocation(string username)
        {
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}?email={2}&companyid={3}&companyLocationId={4}", _appSettings.Leemo_API_Config.BaseUrl,
              Constants.WebConstants.Urls.API_GetCompanyUsersExceptCurrentCompanyLocation, username, _sessionManager.CompanyId, _sessionManager.CompanyLocationID), _sessionManager.BearerToken);

            List<ResultUserByEmailandCompanyID> ExistingUserList = new List<ResultUserByEmailandCompanyID>();

            var result = response.ContentAsType<ResultUserByEmailandCompanyID>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }

            if (result != null)
            {
                var dataUser = result[Constants.WebConstants.Data];
                if (result[Constants.WebConstants.Data] != null)
                {
                    for (var i = 0; i < dataUser.Count; i++)
                    {
                        var model = new ResultUserByEmailandCompanyID();
                        model.Id = dataUser[i].id;
                        model.Email = dataUser[i].email;
                        model.CompanyLocationId = dataUser[i].companyLocationId;

                        ExistingUserList.Add(model);
                    }
                }
                return ExistingUserList;
            }

            return ExistingUserList;

        }

        [ActionPermissionFilter]
        public async Task<IActionResult> GetExistingUserData(string UserId, string CompanyLocationId)
        {
            try
            {
                var response = await HttpRequestFactory.Get(string.Format("{0}{1}?UserId={2}&CompanyLocationId={3}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetExistingUserData, UserId, CompanyLocationId), _sessionManager.BearerToken);

                var user = response.ContentAsType<InputUser>();
                if (user[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(user[Constants.WebConstants.ResponseType]);
                }
                var model = user[Constants.WebConstants.Data].ToObject<InputUser>();

                InputUser inputUser = model;
                var response1 = await HttpRequestFactory.Get(string.Format("{0}{1}?PageNumber={2}&PageSize={3}&CompanyLocationId={4}", _appSettings.Leemo_API_Config.BaseUrl,
               Constants.WebConstants.Urls.API_GetAllPersonalRoles, _appSettings.PageSettings.DefaultPageNumber, _appSettings.PageSettings.DefaultPageSize, _sessionManager.CompanyLocationID), _sessionManager.BearerToken);

                var roles = response1.ContentAsType<List<Auth_Role>>();
                if (roles[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(roles[Constants.WebConstants.ResponseType]);
                }
                if (roles[Constants.WebConstants.ResponseType] == Constants.ResponseType.Error)
                {
                    return Json(roles[Constants.WebConstants.ResponseType]);
                }
                if (inputUser != null)
                {
                    List<Designation> objDesignationList = new List<Designation>();
                    objDesignationList.Add(new Designation
                    {
                        Id = (Guid)inputUser.ExistingUserData.Id,
                        Name = inputUser.ExistingUserData.DesignationName,
                    });
                    ViewBag.roles = roles[Constants.WebConstants.Data].ToObject<List<Auth_Role>>();
                    ViewBag.designations = objDesignationList;
                    if (inputUser.userProfile.ReportingToUserId != null && inputUser.userProfile.ReportingToUserId != Guid.Empty)
                    {
                        string reportingUserId = Convert.ToString(inputUser.userProfile.DesignationId);
                        ViewBag.ReportingToUser = await getParentRoleByRoleId(reportingUserId, "", UserId);
                    }
                    return PartialView(Constants.WebConstants.PartialViews.CreateUser, inputUser);
                }

                return RedirectToAction(Constants.WebConstants.Actions.Index);
            }
            catch (Exception ex)
            {
                return RedirectToAction(Constants.WebConstants.Actions.Index);
            }
        }
    }
}

