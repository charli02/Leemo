using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using LeemoCompanyRegistrationWeb.Web.HttpClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPSS.Common;

namespace LeemoCompanyRegistrationWeb.Controllers
{
    public class CompanyRegistrationController : Controller
    {
        private readonly AppSettings _appSettings;
        public CompanyRegistrationController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public IActionResult Index()
        {
           
            return View();
        }

        public async Task<IActionResult> CompleteCompanyProfile(string id)
        {
            if (id != null)
            {
                var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                            Constants.WebConstants.Urls.API_GetVerifyProductLead, id));
                var result = response.ContentAsType<UpdateInputProductLead>();


                if (result != null)
                {


                    if (result[Constants.WebConstants.Data] != null) {
                        UpdateInputProductLead model = result[Constants.WebConstants.Data].ToObject<UpdateInputProductLead>();
                        return View(model);
                    }
                   
                }

                return RedirectToAction(nameof(ErrorController.PageNotFound), Constants.WebConstants.Controllers.Error);
            }

            return RedirectToAction(nameof(ErrorController.PageNotFound), Constants.WebConstants.Controllers.Error);


        }
      

        public async Task<IActionResult> GetAvailableDomain(string domainName)
        {
            if (domainName != null || domainName != "")
            {
                if (domainName.Length >= _appSettings.DomainMinLength && domainName.Length <= _appSettings.DomainMaxLength)
                {
                    if (CommonFunction.RegexPatternCheckforDomain(domainName.Trim().ToLower()) == true)
                    {
                        List<string> Data = new List<string>();
                        var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                                Constants.WebConstants.Urls.API_GetProductLeadCheckAvailableDomain, domainName));
                        var result = response.ContentAsType<InputUser>();
                        var reponseType = Convert.ToString(result[Constants.WebConstants.ResponseType]);
                        if (reponseType == Constants.ResponseType.AlreadyExists.ToString())
                        {
                            if (result[Constants.WebConstants.Data] != null)
                                Data = result[Constants.WebConstants.Data].ToObject<List<string>>();
                        }
                        //return reponseType;

                        return Json(new { Success = reponseType, Message = Data });
                    }

                }
            }
            return null;
        }


        public async Task<IActionResult> Address(Guid Id)
        {


            if (Id != null)
            {
                var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                        Constants.WebConstants.Urls.API_GetCompanySetupById, Id));
                var result = response.ContentAsType<ResultProductLead>();
                if (result != null)
                {

                    if (result[Constants.WebConstants.Data] != null)
                    {
                        ResultProductLead model = result[Constants.WebConstants.Data].ToObject<ResultProductLead>();

                        UpdateInputProductLead updateProductLead = new UpdateInputProductLead();

                        updateProductLead.Id = model.Id;
                        updateProductLead.City = model.City;
                        updateProductLead.State = model.State;
                        updateProductLead.Country = model.Country;
                        updateProductLead.AddressLine1 = model.AddressLine1;
                        updateProductLead.AddressLine2 = model.AddressLine2;
                        updateProductLead.DomainName = model.DomainName;
                        updateProductLead.ZipCode = model.ZipCode;
                        updateProductLead.Fax = model.Fax;
                        updateProductLead.Phone = model.Phone;

                        //return Json(new { Success = result[Constants.WebConstants.ResponseType], Message = result[Constants.WebConstants.Message] });
                        return PartialView(Constants.WebConstants.PartialViews.VerifyAddress, updateProductLead);

                    }
                }
            }
            return PartialView(Constants.WebConstants.PartialViews.VerifyAddress, null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderInfo(UpdateInputProductLead updateInputProductLead)
        {


            if (ModelState.IsValid) { 
                var response = await HttpRequestFactory.Put(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                           Constants.WebConstants.Urls.API_PostCompanySetupUpdate, updateInputProductLead.Id), updateInputProductLead);
                var result = response.ContentAsType<ResultProductLead>();

                if (result != null)
                {

                    if (result[Constants.WebConstants.Data] != null)
                    {
                        ResultProductLead model = result[Constants.WebConstants.Data].ToObject<ResultProductLead>();

                        UpdateInputProductLead updateProductLead = new UpdateInputProductLead();

                        updateProductLead.Id = model.Id;
                        updateProductLead.City = model.DomainName;
                        updateProductLead.State = model.State;
                        updateProductLead.Country = model.Country;
                        updateProductLead.AddressLine1 = model.AddressLine1;
                        updateProductLead.AddressLine2 = model.AddressLine2;
                        updateProductLead.DomainName = model.DomainName;
                        updateProductLead.ZipCode = model.ZipCode;
                        updateProductLead.Fax = model.Fax;


                        //return Json(new { Success = result[Constants.WebConstants.ResponseType], Message = result[Constants.WebConstants.Message] });
                        return PartialView(Constants.WebConstants.PartialViews.OrderInfo, updateProductLead);

                    }
                }

                return PartialView(Constants.WebConstants.PartialViews.VerifyAddress, updateInputProductLead);

            }


            //var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
            //             Constants.WebConstants.Urls.API_GetCompanySetupById, updateInputProductLead.Id));
            //var result = response.ContentAsType<ResultProductLead>();






            return PartialView(Constants.WebConstants.PartialViews.OrderInfo, updateInputProductLead);
        }

        
        public  IActionResult Comments(UpdateInputProductLead updateInputProductLead)
        {
            //if (ModelState.IsValid)
            //{

            //    var response = await HttpRequestFactory.Put(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
            //               Constants.WebConstants.Urls.API_PostCompanySetupUpdate, updateInputProductLead.Id), updateInputProductLead);
            //    var result = response.ContentAsType<ResultProductLead>();

            //    if (result != null)
            //    {
            //        //return Json(new { Success = result[Constants.WebConstants.ResponseType], Message = result[Constants.WebConstants.Message] });
            //        return PartialView(Constants.WebConstants.PartialViews.Comments, updateInputProductLead);
            //    }

            //    return PartialView(Constants.WebConstants.PartialViews.OrderInfo, updateInputProductLead);

            //}

            return PartialView(Constants.WebConstants.PartialViews.Comments, updateInputProductLead);
        }
        public IActionResult PasswordGenrate(UpdateInputProductLead updateInputProductLead)
        {

            return PartialView(Constants.WebConstants.PartialViews.PasswordGenrate, updateInputProductLead);

        }



    }
}
