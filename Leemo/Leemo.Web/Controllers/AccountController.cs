using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Web.HttpClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TPSS.Common;

namespace Leemo.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly SessionManager _sessionManager;


        public AccountController(IOptions<AppSettings> appSettings, SessionManager sessionManager)
        {
            _appSettings = appSettings.Value;
            _sessionManager = sessionManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(int? check)
        {
            if (check == 1)
            {
                ViewBag.LoginCheck = "Login";
            }
            if (!string.IsNullOrEmpty(_sessionManager.ID))
            {
                return RedirectToAction("Index", "User");
            }
            ViewBag.NewMessage = TempData[Constants.WebConstants.TempModelKeep];
            ViewBag.TempModelCheck = TempData[Constants.WebConstants.TempModelCheck];
            ViewBag.Changepwdmsg = TempData["changepwdmsg"];
            TempData.Remove("changepwdmsg");
            TempData.Remove(Constants.WebConstants.TempModelKeep);
            TempData.Remove(Constants.WebConstants.TempModelCheck);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(InputUserLogin inputUserLogin, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                _sessionManager.ResreshTokenData = CommonFunction.EncodeData(inputUserLogin.Email + "," + inputUserLogin.Password);
                inputUserLogin.CompanyLocationID = null;
                var response = await HttpRequestFactory.Post(
                string.Concat(_appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_UserLogin), inputUserLogin);
                var result = response.ContentAsType<ResultUser>();
                if (result != null)
                {
                    ViewBag.Message = result[Constants.WebConstants.Message];
                    if (result[Constants.WebConstants.Data] != null)
                    {
                        ResultUser resultUser = result[Constants.WebConstants.Data].ToObject<ResultUser>();
                        //REDIRECT USER TO CHANGE PASSWORD PAGE IF THE USER LOGIN FOR FIRST TIME 
                        if (resultUser.isFirstLogin != null && resultUser.isFirstLogin == true)
                        {
                            _sessionManager.BearerToken = resultUser.Token;
                            string enEmail = CommonFunction.EncodeData(resultUser.UserName);
                            return RedirectToAction(Constants.WebConstants.Actions.ChangePassword, new { p1 = enEmail });
                        }
                        _sessionManager.ID = Convert.ToString(resultUser.Id);
                        if (resultUser.UserProfile != null)
                        {
                            //Added full Name In Session
                            _sessionManager.LoginName = String.Format("{0}&{1}", resultUser.UserProfile.FirstName, resultUser.UserProfile.LastName);
                            _sessionManager.BearerToken = resultUser.Token;
                            _sessionManager.CompanyId = resultUser.UserProfile.CompanyId == null ? string.Empty : Convert.ToString(resultUser.UserProfile.CompanyId);
                            _sessionManager.UserProfileImage = string.IsNullOrEmpty(resultUser.UserProfile.ImageName) ? string.Empty : resultUser.UserProfile.ImageName;
                            _sessionManager.UserEmail = resultUser.UserName;
                            _sessionManager.USERAuthRole = resultUser.Auth_Roles.FirstOrDefault().Name;
                            var claims = new List<Claim>{
                                new Claim(ClaimTypes.Name, String.Format("{0}&{1}",resultUser.UserProfile.FirstName,resultUser.UserProfile.LastName))

                            };
                            var userIdentity = new ClaimsIdentity(claims, Constants.WebConstants.Login);

                            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                            await HttpContext.SignInAsync(principal);

                            return Redirect(Constants.WebConstants.Urls.WEB_CompanyLocation);
                        }
                    }
                }
            }
            return View();
        }


        public async Task<IActionResult> ResfreshToken(string CLID)
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                var inputUserLogin = new InputUserLogin();
                inputUserLogin.CompanyLocationID = Guid.Parse(CLID);
                var ResreshTokenData = CommonFunction.DecodeData(_sessionManager.ResreshTokenData);
                inputUserLogin.Email = ResreshTokenData.Split(",")[0];
                inputUserLogin.Password = ResreshTokenData.Split(",")[1];
                var response = await HttpRequestFactory.Post(
                string.Concat(_appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_UserLogin), inputUserLogin);
                var result = response.ContentAsType<ResultUser>();
                if (result != null)
                {
                    ViewBag.Message = result[Constants.WebConstants.Message];
                    if (result[Constants.WebConstants.Data] != null)
                    {
                        ResultUser resultUser = result[Constants.WebConstants.Data].ToObject<ResultUser>();
                        //REDIRECT USER TO CHANGE PASSWORD PAGE IF THE USER LOGIN FOR FIRST TIME 
                        if (resultUser.isFirstLogin != null && resultUser.isFirstLogin == true)
                        {
                            _sessionManager.BearerToken = resultUser.Token;
                            string enEmail = CommonFunction.EncodeData(resultUser.UserName);
                            return RedirectToAction(Constants.WebConstants.Actions.ChangePassword, new { p1 = enEmail });
                        }
                        _sessionManager.ID = Convert.ToString(resultUser.Id);
                        if (resultUser.UserProfile != null)
                        {
                            //Added full Name In Session
                            _sessionManager.LoginName = String.Format("{0}&{1}", resultUser.UserProfile.FirstName, resultUser.UserProfile.LastName);
                            _sessionManager.BearerToken = resultUser.Token;
                            _sessionManager.CompanyId = resultUser.UserProfile.CompanyId == null ? string.Empty : Convert.ToString(resultUser.UserProfile.CompanyId);
                            _sessionManager.UserProfileImage = string.IsNullOrEmpty(resultUser.UserProfile.ImageName) ? string.Empty : resultUser.UserProfile.ImageName;
                            _sessionManager.UserEmail = resultUser.UserName;
                            _sessionManager.USERAuthRole = resultUser.Auth_Roles.FirstOrDefault().Name;
                            var claims = new List<Claim>{
                                new Claim(ClaimTypes.Name, String.Format("{0}&{1}",resultUser.UserProfile.FirstName,resultUser.UserProfile.LastName))

                            };
                            var userIdentity = new ClaimsIdentity(claims, Constants.WebConstants.Login);

                            ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                            await HttpContext.SignInAsync(principal);

                            return Redirect(Constants.WebConstants.Urls.WEB_UserIndex);
                        }
                    }
                }
                return Json(new { a = 1 });
            }
            catch (Exception ex)
            {
                return Json(new { a = 1 });
            }
        }

        public async Task<IActionResult> Logout(int? isForceLogout)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _sessionManager.EmptySession();
            if (isForceLogout == 1)
            {
                return RedirectToAction(nameof(AccountController.Login), Constants.WebConstants.Controllers.Account, new { check = 1 });
            }
            return RedirectToAction(nameof(AccountController.Login), Constants.WebConstants.Controllers.Account);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(InputForgetPassword inputForgetPassword)
        {
            bool isSucceed = false;
            string message = "";
            if (ModelState.IsValid)
            {
                var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                                           Constants.WebConstants.Urls.API_ForgotPassword), inputForgetPassword, _sessionManager.BearerToken);
                var result = response.ContentAsType<String>();
                InputChangePassword model = new InputChangePassword();
                model.Email = inputForgetPassword.Email;
                model.OldPassword = result[Constants.WebConstants.Data];
                isSucceed = result[Constants.WebConstants.Succeeded];
                if (isSucceed == true)
                {
                    TempData[Constants.WebConstants.TempModelKeep] = Constants.Messages.CheckEmail;
                    TempData[Constants.WebConstants.TempModelCheck] = 1;
                }
                else
                    message = result[Constants.WebConstants.Message];
            }
            return Json(new { Success = isSucceed, Message = message });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string p1, string p2)
        {
            InputChangePassword inputChangePassword = new InputChangePassword();
            inputChangePassword.Email = p1;
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                     Constants.WebConstants.Urls.API_GetUserByOnlyEmail, CommonFunction.DecodeData(p1)), _sessionManager.BearerToken);
            var result = response.ContentAsType<User>();
            User data = result[Constants.WebConstants.Data].ToObject<User>();
            if (data.TempPasswordSalt == null && data.TempPasswordHash == null || data.TempPasswordExpiryDate < DateTime.UtcNow)
            {
                ViewBag.LinkExpire = true;
            }
            inputChangePassword.OldPassword = p2;
            return View(inputChangePassword);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(InputChangePassword inputChangePassword, string ConfirmPassword)
        {
            bool isSucceed = false;
            string message = "";
            try
            {
                inputChangePassword.Email = CommonFunction.DecodeData(inputChangePassword.Email);
                if (inputChangePassword.FirstLogin != "FirstLogin")
                {
                    inputChangePassword.OldPassword = CommonFunction.DecodeData(inputChangePassword.OldPassword);
                }
                if (inputChangePassword.FirstLogin == "FirstLogin")
                {
                    if (inputChangePassword.OldPassword == inputChangePassword.NewPassword)
                    {
                        message = Constants.Messages.SamePasswordMatch;
                        return Json(new { Success = isSucceed, Message = message });
                    }

                }
                if (inputChangePassword.NewPassword == ConfirmPassword)
                {

                    var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                                   Constants.WebConstants.Urls.API_ResetPassword), inputChangePassword, _sessionManager.BearerToken);

                    var result = response.ContentAsType<String>();
                    isSucceed = result[Constants.WebConstants.Succeeded];
                    if (isSucceed == false)
                    {
                        if (inputChangePassword.FirstLogin == "FirstLogin")
                        {
                            message = Constants.Messages.IncorrectOldPassword;
                        }
                        else
                        {
                            message = Constants.Messages.ResetLinkExpire;
                        }
                    }
                    else
                    {
                        message = result[Constants.WebConstants.Message];
                        TempData[Constants.WebConstants.TempModelKeep] = message;
                    }

                }
                else
                {
                    message = Constants.Messages.PasswordNotMatch;
                }
                return Json(new { Success = isSucceed, Message = message });
            }
            catch (Exception ex)
            {
                return Json(new { Success = isSucceed, Message = ex.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangePassword(string p1)
        {
            InputChangePassword inputChangePassword = new InputChangePassword();
            inputChangePassword.Email = p1;
            return View(inputChangePassword);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(InputChangePassword inputChangePassword)
        {
            bool isSucceed = false;
            string message = "";
            try
            {
                inputChangePassword.Email = CommonFunction.DecodeData(inputChangePassword.Email);
                if (inputChangePassword.NewPassword == inputChangePassword.ConfirmPassword && inputChangePassword.OldPassword != inputChangePassword.NewPassword)
                {
                    var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                            Constants.WebConstants.Urls.API_ChangePassword), inputChangePassword, _sessionManager.BearerToken);

                    var result = response.ContentAsType<string>();
                    if (result != null)
                    {
                        isSucceed = result[Constants.WebConstants.Succeeded];
                        message = result[Constants.WebConstants.Message];
                        if (isSucceed)
                            TempData["changepwdmsg"] = message;
                    }
                    else
                    {
                        message = Constants.Messages.InternalServerError;
                    }
                    return Json(new { Success = isSucceed, Message = message });
                }
                else
                {
                    if (inputChangePassword.OldPassword == inputChangePassword.NewPassword)
                        message = Constants.Messages.SamePasswordMatch;
                    if (inputChangePassword.NewPassword != inputChangePassword.ConfirmPassword)
                        message = Constants.Messages.PasswordNotMatch;
                }
                return Json(new { Success = isSucceed, Message = message });
            }
            catch (Exception ex)
            {
                return Json(new { Success = isSucceed, Message = ex.Message });
            }
        }
    }
}
