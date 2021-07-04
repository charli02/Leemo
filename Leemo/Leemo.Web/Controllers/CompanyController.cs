using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TPSS.Common;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Model.WrapperModels;
using Leemo.Web.Filters;
using Leemo.Web.HttpClient;

namespace Leemo.Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly SessionManager _sessionManager;

        public CompanyController(IOptions<AppSettings> appSettings, SessionManager sessionManager)
        {
            _appSettings = appSettings.Value;
            _sessionManager = sessionManager;
        }
        
        [ActionPermissionFilter]
        public async Task<IActionResult> IndexAsync()
        {
            try
            {
                if (_sessionManager.CompanyId != null && _sessionManager.CompanyId != "")
                {
                    List<Auth_FeatureListWithGeneralCodeByUserIdResult> PermissionData = (List<Auth_FeatureListWithGeneralCodeByUserIdResult>)HttpContext.Items["PermissionData"];
                    if (PermissionData != null && PermissionData.Count > 0)
                    {
                        var RoleName = PermissionData[0].RoleName;
                        var viewPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.OrganizationSettings && x.CodeValue == Constants.PermissionConstants.CodeValue.View).ToList();
                        var editPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.OrganizationSettings && x.CodeValue == Constants.PermissionConstants.CodeValue.Update).ToList();
                        if (editPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                        {
                            ViewBag.editOrganizationPermission = true;
                            TempData["editOrganizationPermission"] = true;
                        }
                        if (viewPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                        {
                            ViewBag.viewOrganizationPermission = true;
                            TempData["viewOrganizationPermission"] = true;
                            var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                            Constants.WebConstants.Urls.API_GetCompany, _sessionManager.CompanyId), _sessionManager.BearerToken);
                            var result = response.ContentAsType<ResultCompanyAndAddresses>();
                            if (result != null)
                            {
                                return View(result[Constants.WebConstants.Data].ToObject<ResultCompanyAndAddresses>());
                            }
                        }
                    }
                }
                return RedirectToAction(nameof(AccountController.Login), Constants.WebConstants.Controllers.Account);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(AccountController.Login), Constants.WebConstants.Controllers.Account);
            }
        }
        
        [HttpPost]
        [ActionPermissionFilter]
        public async Task<IActionResult> EditCompany(InputCompanyAndAddresses company)
        {
            try
            {
                company.inputCompany.Mobile = company.inputCompany.CountryCodeNumber + "-" + company.inputCompany.MobileNumber;

                if (ModelState.IsValid)
                {
                        company.CompanyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
                        var response = await HttpRequestFactory.Put(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                        Constants.WebConstants.Urls.API_PutCompany, company.inputCompany.Id), company, _sessionManager.BearerToken);

                        var result = response.ContentAsType<Company>();
                        if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                        {
                            return Json(result[Constants.WebConstants.ResponseType]);
                        }
                        if (result != null)
                        {
                            ViewBag.Message = result[Constants.WebConstants.Message];
                            if (result[Constants.WebConstants.Data] != null)
                            {

                                return Redirect(Constants.WebConstants.Urls.WEB_CompanyIndex + company.inputCompany.Id);
                            }
                        }
                }
                return Redirect(Constants.WebConstants.Urls.WEB_CompanyIndex + company.inputCompany.Id);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(AccountController.Login), Constants.WebConstants.Controllers.Account);
            }
        }

        [HttpPost]
        [ActionPermissionFilter]
        public async Task<object> UploadCompanyImage(Leemo.Web.Models.Common.ImageUpload model)
        {
            string message = "";
            if (model.Id == Guid.Empty)
                return message;
            if (model.ImageFile != null)
            {
                long FileSize = model.ImageFile.Length;
                if (FileSize < _appSettings.MaxImageSize)
                {
                    InputUpdateCompanyImage companyImageinput = new InputUpdateCompanyImage();
                    string filepath = string.Concat(_appSettings.Resources_BaseDir, _appSettings.CompanyImagesPath);
                string filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                string extension = Path.GetExtension(model.ImageFile.FileName).ToLower();
                    filename = filename + DateTime.Now.ToString("yyssmmfff") + extension; //TODO: Use the different format first and then managed the format in constant.
                    if (extension == Constants.Extensions.PNG || extension == Constants.Extensions.JPG || extension == Constants.Extensions.JPEG)
                    {

                        //API call for updating Image name in Company
                    companyImageinput.CompanyId = model.Id;
                        companyImageinput.ImageName = filename;
                        var response =await HttpRequestFactory.Put(
                            string.Concat(_appSettings.Leemo_API_Config.BaseUrl,
                            Constants.WebConstants.Urls.API_Company_UpdateCompanyImage), companyImageinput, _sessionManager.BearerToken);
                        var result = response.ContentAsType<object>();
                        if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                        {
                            return Json(result[Constants.WebConstants.ResponseType]);
                        }
                        //Saving Image in Physical Path /Resources/CompanyImages/
                        string path = Path.Combine(filepath, filename);
                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                        model.ImageFile.CopyTo(filestream);
                        }
                        message = Constants.Messages.CompanyImageUpdated;
                        return filename;
                    }
                    else
                    {
                        //message = Constants.Messages.InvalidFile;
                        message = "0";
                    }
                }

                else {
                    message = "1";
                }
                    
            }
            return message;
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> CompanyEditPopup(Guid id)
        {
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetCompany, id), _sessionManager.BearerToken);
            var result = response.ContentAsType<InputCompanyAndAddresses>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            ResultCompanyAndAddresses Resultmodel = result[Constants.WebConstants.Data].ToObject<ResultCompanyAndAddresses>();
            InputCompanyAndAddresses model = result[Constants.WebConstants.Data].ToObject<InputCompanyAndAddresses>();
            model.inputCompany = new InputCompany();
            if (Resultmodel != null)
            {
                model.inputCompany.Id = Resultmodel.resultCompany.Id;
                model.inputCompany.Name = Resultmodel.resultCompany.Name;
                model.inputCompany.EmployeeCount = Resultmodel.resultCompany.EmployeeCount;
                model.inputCompany.Phone = Resultmodel.resultCompany.Phone;
                model.inputCompany.Mobile = Resultmodel.resultCompany.Mobile;
                model.inputCompany.Fax = Resultmodel.resultCompany.Fax;
                model.inputCompany.Website = Resultmodel.resultCompany.Website;
                model.inputCompany.Description = Resultmodel.resultCompany.Description;
                model.inputCompany.Currency = Resultmodel.resultCompany.Currency;
                model.inputCompany.TimeZone = Resultmodel.resultCompany.TimeZone;
                model.inputCompany.Language = Resultmodel.resultCompany.Language;
                model.inputCompany.CountryCode = Resultmodel.resultCompany.CountryCode;
                model.inputCompany.TimeFormat = Resultmodel.resultCompany.TimeFormat;
                model.inputCompany.DateFormat = Resultmodel.resultCompany.DateFormat;
            }

            if (model.inputCompany.Mobile == "")
            {
                model.inputCompany.MobileNumber = null;
            }
            else if ((model.inputCompany.Mobile).Contains('-')) {
                var Mobile = model.inputCompany.Mobile.Trim().Split("-");
                model.inputCompany.MobileNumber = Mobile[1].Trim();
            }
            else {
                 model.inputCompany.MobileNumber = model.inputCompany.Mobile;
            }

            if (result != null)
            {
                return PartialView(Constants.WebConstants.PartialViews.EditCompany, model);
            }
            return View();
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> CompanyDetails()
        {
            if (_sessionManager.CompanyId != null && _sessionManager.CompanyId != "")
            {
                List<Auth_FeatureListWithGeneralCodeByUserIdResult> PermissionData = (List<Auth_FeatureListWithGeneralCodeByUserIdResult>)HttpContext.Items["PermissionData"];
                if (PermissionData != null && PermissionData.Count > 0)
                {
                    var RoleName = PermissionData[0].RoleName;
                    var viewPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.OrganizationSettings && x.CodeValue == Constants.PermissionConstants.CodeValue.View).ToList();
                    var editPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.OrganizationSettings && x.CodeValue == Constants.PermissionConstants.CodeValue.Update).ToList();
                    if (editPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        TempData["editOrganizationPermission"] = true;
                    }
                    if (viewPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        TempData["viewOrganizationPermission"] = true;
                        var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                            Constants.WebConstants.Urls.API_GetCompany, _sessionManager.CompanyId), _sessionManager.BearerToken);


                        var result = response.ContentAsType<ResultCompanyAndAddresses>();
                        if (result != null)
                        {
                            return PartialView(Constants.WebConstants.PartialViews.CompanyDetails, result[Constants.WebConstants.Data].ToObject<ResultCompanyAndAddresses>());
                        }
                    }
                }
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
