using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Web.Filters;
using Leemo.Web.HttpClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPSS.Common;

namespace Leemo.Web.Controllers
{
    public class BillingController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly SessionManager _sessionManager;

        public BillingController(IOptions<AppSettings> appSettings, SessionManager sessionManager)
        {
            _appSettings = appSettings.Value;
            _sessionManager = sessionManager;
        }
        [ActionPermissionFilter]
        public async Task<IActionResult> Index()
        {
            var model = new List<ResultBillingAddress>();
            bool sessionCheck = false;
            List<Auth_FeatureListWithGeneralCodeByUserIdResult> PermissionData = (List<Auth_FeatureListWithGeneralCodeByUserIdResult>)HttpContext.Items["PermissionData"];
            if (PermissionData != null && PermissionData.Count > 0)
            {
                sessionCheck = true;
                var RoleName = PermissionData[0].RoleName;
                var addPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.BillingAddresses && x.CodeValue == Constants.PermissionConstants.CodeValue.Add).ToList();
                var viewPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.BillingAddresses && x.CodeValue == Constants.PermissionConstants.CodeValue.View).ToList();
                var editPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.BillingAddresses && x.CodeValue == Constants.PermissionConstants.CodeValue.Update).ToList();
                var deletePermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.BillingAddresses && x.CodeValue == Constants.PermissionConstants.CodeValue.Delete).ToList();
               
                if (addPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    ViewBag.addBillingAddressesPermission = true;
                }
                if (editPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    ViewBag.editBillingAddressesPermission = true;
                }
                if (deletePermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    ViewBag.deleteBillingAddressesPermission = true;
                }
                if (viewPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    ViewBag.viewBillingAddressesPermission = true;
                   
                    var companyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
                    var response = await HttpRequestFactory.Get(string.Format("{0}{1}?CompanyLocationId={2}", _appSettings.Leemo_API_Config.BaseUrl,
                        Constants.WebConstants.Urls.API_GetAllBillings, companyLocationId), _sessionManager.BearerToken);
                    var result = response.ContentAsType<List<ResultBillingAddress>>();
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                    model = result[Constants.WebConstants.Data].ToObject<List<ResultBillingAddress>>();
                    return View(model);
                }

            }
            if (!sessionCheck && PermissionData == null)
            {
                //return Redirect(Constants.WebConstants.Urls.WEB_BillingIndex);
                return RedirectToAction("Login", Constants.WebConstants.Controllers.Account); ;
            }
            return View(model);



        }
        [ActionPermissionFilter]
        public async Task<IActionResult> GetBillingAddresses()
        {
            try
            {
                bool sessionCheck = false;
                List<Auth_FeatureListWithGeneralCodeByUserIdResult> PermissionData = (List<Auth_FeatureListWithGeneralCodeByUserIdResult>)HttpContext.Items["PermissionData"];
                if (PermissionData != null && PermissionData.Count > 0)
                {
                    sessionCheck = true;
                    var RoleName = PermissionData[0].RoleName;
                    var addPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.BillingAddresses && x.CodeValue == Constants.PermissionConstants.CodeValue.Add).ToList();
                    var viewPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.BillingAddresses && x.CodeValue == Constants.PermissionConstants.CodeValue.View).ToList();
                    var editPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.BillingAddresses && x.CodeValue == Constants.PermissionConstants.CodeValue.Update).ToList();
                    var deletePermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.BillingAddresses && x.CodeValue == Constants.PermissionConstants.CodeValue.Delete).ToList();

                    if (addPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        ViewBag.addBillingAddressesPermission = true;
                    }
                    if (editPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        ViewBag.editBillingAddressesPermission = true;
                    }
                    if (deletePermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        ViewBag.deleteBillingAddressesPermission = true;
                    }
                    if (viewPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        ViewBag.viewBillingAddressesPermission = true;
                        Guid companyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
                        var response = await HttpRequestFactory.Get(string.Format("{0}{1}?CompanyLocationId={2}", _appSettings.Leemo_API_Config.BaseUrl,
                            Constants.WebConstants.Urls.API_GetAllBillings, companyLocationId), _sessionManager.BearerToken);
                        var result = response.ContentAsType<List<ResultBillingAddress>>();
                        if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                        {
                            return Json(result[Constants.WebConstants.ResponseType]);
                        }
                        if (result != null)
                        {
                            List<ResultBillingAddress> resultBillingAddress = result[Constants.WebConstants.Data].ToObject<List<ResultBillingAddress>>();
                            return PartialView(Constants.WebConstants.PartialViews.BillingList, resultBillingAddress);
                        }
                    }
                    
                }
                if (!sessionCheck)
                {
                    return RedirectToAction("Login", Constants.WebConstants.Controllers.Account); ;
                }
                return RedirectToAction("Login", Constants.WebConstants.Controllers.Account);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", Constants.WebConstants.Controllers.Account);

            }
        }
        [ActionPermissionFilter]
        public async Task<IActionResult> CreateBillingAddress()
        {
            return PartialView("_CreateBillingAddress", null);
        }
        [ActionPermissionFilter]
        [HttpPost]
        public async Task<IActionResult> CreateBillingAddress(InputAddress inputAddress)
        {
            try {
                if (ModelState.IsValid)
                {
                    var model = new InputAddress();
                    inputAddress.ReferenceId = Guid.Parse(_sessionManager.CompanyLocationID);
                    var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                         Constants.WebConstants.Urls.API_PostBillingAddresses), inputAddress, _sessionManager.BearerToken);
                    var result = response.ContentAsType<InputAddress>();
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                    model = result[Constants.WebConstants.Data].ToObject<InputAddress>();
                    return PartialView("_CreateBillingAddress", model);
                }
                return Redirect(Constants.WebConstants.Urls.WEB_BillingIndex);
            }
            catch(Exception ex)
            {
                return Redirect(Constants.WebConstants.Urls.WEB_BillingIndex);
            }
        }
        [ActionPermissionFilter]
        public async Task<IActionResult> EditBillingAddress(Guid Id)
        {
            try
            {
                if (Id != null)
                {
                    var model = new InputAddress();
                    var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                       Constants.WebConstants.Urls.API_GetBillingById, Id), _sessionManager.BearerToken);
                    var result = response.ContentAsType<InputAddress>();
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                    model = result[Constants.WebConstants.Data].ToObject<InputAddress>();
                    return PartialView("_CreateBillingAddress", model);
                }
                return Redirect(Constants.WebConstants.Urls.WEB_BillingIndex);
            }
            catch(Exception ex)
            {
                return View();
            }
        }
        [ActionPermissionFilter]
        [HttpPost]
        public async Task<IActionResult> EditBillingAddress(Guid Id,InputAddress inputAddress)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //inputAddress.ReferenceId = Guid.Parse(_sessionManager.CompanyId);
                    var model = new InputAddress();
                    var response = await HttpRequestFactory.Put(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                       Constants.WebConstants.Urls.API_UpdateBillingAddresses, Id), inputAddress, _sessionManager.BearerToken);
                    var result = response.ContentAsType<InputAddress>();
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                    if (result != null)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                }
                return Redirect(Constants.WebConstants.Urls.WEB_BillingIndex);
            }
            catch (Exception ex)
            {
                return Redirect(Constants.WebConstants.Urls.WEB_BillingIndex);
            }
        }
        [ActionPermissionFilter]
        public async Task<IActionResult> DeleteBillingAddress(Guid Id)
        {
            try
            {
                if (Id != null)
                {
                    var companyId = _sessionManager.CompanyId;
                    var model = new InputAddress();
                    var response = await HttpRequestFactory.Delete(string.Format("{0}{1}?id={2}", _appSettings.Leemo_API_Config.BaseUrl,
                       Constants.WebConstants.Urls.API_DeleteBillingAddresses, Id), _sessionManager.BearerToken);
                    var result = response.ContentAsType<InputAddress>();
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                    if (result != null)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                }
                return Redirect(Constants.WebConstants.Urls.WEB_BillingIndex);
            }
            catch (Exception ex)
            {
                return Redirect(Constants.WebConstants.Urls.WEB_BillingIndex);
            }
        }
    }


}
