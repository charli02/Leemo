using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using TPSS.Common;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Model.WrapperModels;
using Leemo.Web.Filters;
using Leemo.Web.HttpClient;

namespace Leemo.Web.Controllers
{
    public class LocationController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly SessionManager _sessionManager;

        public LocationController(IOptions<AppSettings> appSettings, SessionManager sessionManager)
        {
            _appSettings = appSettings.Value;
            _sessionManager = sessionManager;
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> Index()
        {
            bool sessionCheck = false;
            List<ResultLocation> model = new List<ResultLocation>();
            List<Auth_FeatureListWithGeneralCodeByUserIdResult> PermissionData = (List<Auth_FeatureListWithGeneralCodeByUserIdResult>)HttpContext.Items["PermissionData"];
            if (PermissionData != null && PermissionData.Count > 0)
            {
                sessionCheck = true;
                var RoleName = PermissionData[0].RoleName;
                var addPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.Locations && x.CodeValue == Constants.PermissionConstants.CodeValue.Add).ToList();
                var viewPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.Locations && x.CodeValue == Constants.PermissionConstants.CodeValue.View).ToList();
                var editPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.Locations && x.CodeValue == Constants.PermissionConstants.CodeValue.Update).ToList();
                if (addPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    ViewBag.addLocationPermission = true;
                }
                if (editPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    ViewBag.editLocationPermission = true;
                }
                if (viewPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    ViewBag.viewLocationPermission = true;
                    var response = await HttpRequestFactory.Get(string.Format("{0}{1}?CompanyId={2}", _appSettings.Leemo_API_Config.BaseUrl,
                   Constants.WebConstants.Urls.API_GetAllCompanyLocation, _sessionManager.CompanyId), _sessionManager.BearerToken);

                    var result = response.ContentAsType<List<CompanyLocation>>();

                    if (result != null)
                    {
                        model = result[Constants.WebConstants.Data].ToObject<List<ResultLocation>>();
                    }
                    var response1 = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetLocationCountsByCompanyId, _sessionManager.CompanyId), _sessionManager.BearerToken);
                    var result1 = response1.ContentAsType<Dictionary<string, int>>();
                    if (result1 != null)
                    {
                        ViewBag.AllLocations = Convert.ToInt32(result1["data"].All);
                        ViewBag.ActiveLocations = Convert.ToInt32(result1["data"].Active);
                        ViewBag.inActiveLocations = ViewBag.AllLocations - ViewBag.ActiveLocations;
                        return View(model);
                    }
                }
            }
            if (!sessionCheck)
            {
                return RedirectToAction("Login", Constants.WebConstants.Controllers.Account);
            }
            return View(model);
            //return Json(Constants.ResponseType.AccessDenied);
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> CreateLocation()
        {
            return PartialView(Constants.WebConstants.PartialViews.CreateLocation, null);
        }

        [HttpPost]
        [ActionPermissionFilter]
        public async Task<IActionResult> CreateLocation(InputLocationandAddress location)
        {
            if (ModelState.IsValid)
            {
                location.inputLocation.IsActive = true;
                location.inputLocation.CompanyId = Guid.Parse(_sessionManager.CompanyId);
                location.inputLocation.CreatedBy = Guid.Parse(_sessionManager.ID);
                var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_PostCompanyLocation), location, _sessionManager.BearerToken);

                var result = response.ContentAsType<InputLocationandAddress>();
                if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(result[Constants.WebConstants.ResponseType]);
                }
                if (result != null)
                {
                    ViewBag.Message = result[Constants.WebConstants.Message];
                    if (result[Constants.WebConstants.Data] != null)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                        //return Redirect(Constants.WebConstants.Urls.WEB_UserIndex);
                    }
                }
            }
            return Redirect(Constants.WebConstants.Urls.WEB_UserIndex);
        }

        [HttpPost]
        [ActionPermissionFilter]
        public async Task<IActionResult> UpdateLocation(Guid id, InputLocationandAddress location)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //location.inputLocation.IsActive = true;
                    location.inputLocation.ModifiedBy = Guid.Parse(_sessionManager.ID);

                    var response = await HttpRequestFactory.Put(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                      Constants.WebConstants.Urls.API_UpdateCompanyLocation, id), location, _sessionManager.BearerToken);

                    var result = response.ContentAsType<InputLocationandAddress>();
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                    if (result != null)
                    {
                        ViewBag.Message = result[Constants.WebConstants.Message];
                        if (result[Constants.WebConstants.Data] != null)
                        {
                            return Json(result[Constants.WebConstants.ResponseType]);
                        }
                    }
                }
                return Redirect(Constants.WebConstants.Urls.WEB_UserIndex);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(AccountController.Login), Constants.WebConstants.Controllers.Account);
            }
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> LocationList(string QuerySearch = "", int GetActiveLocations = 1)
        {
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}?PageNumber={2}&PageSize={3}&GetActiveLocations={4}&QuerySearch={5}&CompanyId={6}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_GetAllCompanyLocation, _appSettings.PageSettings.DefaultPageNumber, _appSettings.PageSettings.DefaultPageSize, GetActiveLocations, QuerySearch, _sessionManager.CompanyId), _sessionManager.BearerToken);

            var result = response.ContentAsType<List<ResultLocation>>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            var response1 = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                 Constants.WebConstants.Urls.API_GetLocationCountsByCompanyId, _sessionManager.CompanyId), _sessionManager.BearerToken);
            var result1 = response1.ContentAsType<Dictionary<string, int>>();
            if (result1 != null)
            {
                ViewBag.AllLocations = Convert.ToInt32(result1["data"].All);
                ViewBag.ActiveLocations = Convert.ToInt32(result1["data"].Active);
                ViewBag.inActiveLocations = ViewBag.AllLocations - ViewBag.ActiveLocations;
            }
            if (result != null)
            {
                return PartialView(Constants.WebConstants.PartialViews.AllLocation, result[Constants.WebConstants.Data].ToObject<List<ResultLocation>>());
            }
            return PartialView(Constants.WebConstants.PartialViews.AllLocation, null);
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> GetLocationById(Guid id)
        {
            List<Auth_FeatureListWithGeneralCodeByUserIdResult> PermissionData = (List<Auth_FeatureListWithGeneralCodeByUserIdResult>)HttpContext.Items["PermissionData"];
            if (PermissionData != null && PermissionData.Count > 0)
            {
                var RoleName = PermissionData[0].RoleName;
                var viewPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.Locations && x.CodeValue == Constants.PermissionConstants.CodeValue.View).ToList();
                var editPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.Locations && x.CodeValue == Constants.PermissionConstants.CodeValue.Update).ToList();
                if (editPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    ViewBag.editLocationPermission = true;
                }
                if (viewPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    ViewBag.viewLocationPermission = true;
                    var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetLocationById, id), _sessionManager.BearerToken);
                    ResultLocationAndAddress model = new ResultLocationAndAddress();
                    var result = response.ContentAsType<ResultLocationAndAddress>();
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                    if (result != null)
                    {
                        return PartialView(Constants.WebConstants.PartialViews.LocationDetails, result[Constants.WebConstants.Data].ToObject<ResultLocationAndAddress>());
                    }
                }
            }
            //return View();
            return Json(Constants.ResponseType.AccessDenied);
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> EditLocation(Guid id)
        {
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetLocationById, id), _sessionManager.BearerToken);

            InputLocationandAddress model = new InputLocationandAddress();


            var result = response.ContentAsType<InputLocationandAddress>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }

            if (result != null)
            {
                model.Addresses = result[Constants.WebConstants.Data].address.ToObject<InputAddress>();
                model.inputLocation = result[Constants.WebConstants.Data].resultLocation.ToObject<InputLocation>();
                return PartialView(Constants.WebConstants.PartialViews.CreateLocation, model);
            }
            return View();
        }

        public async Task<IActionResult> CompanyLocation()
        {
            List <ResultLocation> model = new List<ResultLocation>();
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                 Constants.WebConstants.Urls.API_GetCompanyLocationByUserId, _sessionManager.ID), _sessionManager.BearerToken);

            var result = response.ContentAsType<List<ResultLocation>>();
            if (result != null)
            {
                model = result[Constants.WebConstants.Data].ToObject<List<ResultLocation>>();

                if (model != null && model.Count() > 0)
                {
                    if (_sessionManager.CompanyLocationID != null && _sessionManager.CompanyLocationName != null)
                    {
                        ViewBag.SelectedCompanyLocationID = _sessionManager.CompanyLocationID;
                        return View(model);
                    }

                    _sessionManager.CompanyLocationID = model.Where(x => x.IsBaseLocation).Select(x => x.Id).FirstOrDefault().ToString();
                    _sessionManager.CompanyLocationName = model.Where(x => x.IsBaseLocation).Select(x => x.LocationName).FirstOrDefault().ToString();
                    if (model.Count == 1)
                    {
                        _sessionManager.SingleLocation = Convert.ToString(true);
                        return RedirectToAction("ResfreshToken", "Account", new { CLID = model[0].Id });
                        //return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        _sessionManager.SingleLocation = Convert.ToString(false);
                    }
                }
            return View(model);
            }
            else
            {
                return RedirectToAction("Login", Constants.WebConstants.Controllers.Account);
            }
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> ChangeCompanyLocation(Guid id, string locationName)
        {
            if (id != null && id != Guid.Empty && locationName != "")
            {
                _sessionManager.CompanyLocationID = Convert.ToString(id);
                _sessionManager.CompanyLocationName = Convert.ToString(locationName);

                return RedirectToAction("ResfreshToken", "Account", new { CLID = id });
            }
            return RedirectToAction("Login", "Account");
        }

        [ActionPermissionFilter]
        public async Task<string> GetLocationName(string locationName)
        {
            var companyLocationId = _sessionManager.CompanyLocationID;
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}?locationName={2}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_GetLocationName, locationName), _sessionManager.BearerToken);
            var result = response.ContentAsType<ResultLocation>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            if (result != null)
            {
                if (result[Constants.WebConstants.Data] != null)
                {
                    string message = Constants.Messages.LocationAlreadyExist;
                    return message;
                }
                return Constants.Messages.Success;
            }
            return Constants.Messages.EnterName;
        }

        [ActionPermissionFilter]
        public async Task<Boolean> CountCompanyLocationByUser()
        {
            var isMultipleLocations = false;
            List<ResultLocation> model = new List<ResultLocation>();
            try { 
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                 Constants.WebConstants.Urls.API_GetCompanyLocationByUserId, _sessionManager.ID), _sessionManager.BearerToken);

            var result = response.ContentAsType<List<ResultLocation>>();
            if (result != null)
            {
                model = result[Constants.WebConstants.Data].ToObject<List<ResultLocation>>();

                if (model != null && model.Count() > 0)
                {
                    if (model.Count > 1)
                    {
                        _sessionManager.SingleLocation = Convert.ToString(false);
                        isMultipleLocations = true;
                    }
                        else
                        {
                            _sessionManager.SingleLocation = Convert.ToString(true);
                        }
                }
            }
            }
            catch (Exception ex)
            {

            }
            return isMultipleLocations;
        }
    }
}
    
