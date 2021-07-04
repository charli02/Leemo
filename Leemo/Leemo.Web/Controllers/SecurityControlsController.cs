using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPSS.Common;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Model.WrapperModels;
using Leemo.Web.Filters;
using Leemo.Web.HttpClient;

namespace Leemo.Web.Controllers
{
    public class SecurityControlsController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly SessionManager _sessionManager;
        public SecurityControlsController(IOptions<AppSettings> appSettings, SessionManager sessionManager)
        {
            _appSettings = appSettings.Value;
            _sessionManager = sessionManager;
        }
        [ActionPermissionFilter]
        public async Task<IActionResult> Index()
        {
            try
            {
                bool sessionCheck = false;
                var model = new SecurityProfile();
                List<Auth_FeatureListWithGeneralCodeByUserIdResult> PermissionData = (List<Auth_FeatureListWithGeneralCodeByUserIdResult>)HttpContext.Items["PermissionData"];
                if (PermissionData != null && PermissionData.Count > 0)
                {
                    var response1 = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}?CompanyLocationId={3}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetProductsOfCompany, _sessionManager.CompanyId, _sessionManager.CompanyLocationID), _sessionManager.BearerToken);
                    var result1 = response1.ContentAsType<List<Product>>();
                    IEnumerable<Product> products = result1["data"].ToObject<List<Product>>();
                    var productNames = products.Select(x=>x.ProductName).ToList();
                    //ViewBag.Products = productNames;
                    ViewBag.Products = products;
                    sessionCheck = true;
                    var RoleName = PermissionData[0].RoleName;
                    var viewPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.View).ToList();
                    var addPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.Add).ToList();
                    var editPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.Update).ToList();
                    var viewUsersPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.ViewUsers).ToList();
                    var deletePermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.Delete).ToList();
                    var viewSecurityPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.ViewPermissions).ToList();
                    if (addPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        ViewBag.AddSecurityRolePermission = true;
                    }
                    if (editPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        TempData["EditSecurityRolePermission"] = true;
                    }
                    if (viewUsersPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        TempData["ViewUsersSecurityRolePermission"] = true;
                    }
                    if (deletePermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        TempData["DeleteSecurityRolePermission"] = true;
                    }
                    if (viewSecurityPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        TempData["ViewAuthRolePermission"] = true;
                    }
                    if (viewPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        TempData["ViewSecurityRolePermission"] = true;
                        var response = await HttpRequestFactory.Get(string.Format("{0}{1}?PageNumber={2}&PageSize={3}&CompanyLocationId={4}&CompanyId={5}",
                            _appSettings.Leemo_API_Config.BaseUrl,
                        Constants.WebConstants.Urls.API_RolesJoinedUser, _appSettings.PageSettings.DefaultPageNumber, _appSettings.PageSettings.DefaultPageSize,
                        _sessionManager.CompanyLocationID, _sessionManager.CompanyId), _sessionManager.BearerToken);
                        var All_AuthJoinedUser = response.ContentAsType<List<ResultRole>>();
                        if (All_AuthJoinedUser[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                        {
                            return Json(All_AuthJoinedUser[Constants.WebConstants.ResponseType]);
                        }
                        model.ResultProfile = All_AuthJoinedUser[Constants.WebConstants.Data].ToObject<List<ResultRole>>();
                        TempData["Auth_RoleName"] = _sessionManager.USERAuthRole;
                    }
                    else
                    {
                        //return Json(Constants.ResponseType.AccessDenied);
                        return View(model);
                        //return RedirectToAction("Login", Constants.WebConstants.Controllers.Account);
                    }
                }
                //else
                //{
                //    return View(model);
                //}
                if (!sessionCheck && PermissionData == null)
                {
                    return RedirectToAction("Login", Constants.WebConstants.Controllers.Account);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", Constants.WebConstants.Controllers.Account);
            }
        }

        #region Roles

        [ActionPermissionFilter]
        public async Task<IActionResult> SecurityProfile()
        {
            List<Auth_FeatureListWithGeneralCodeByUserIdResult> PermissionData = (List<Auth_FeatureListWithGeneralCodeByUserIdResult>)HttpContext.Items["PermissionData"];
            if (PermissionData != null && PermissionData.Count > 0)
            {
                var RoleName = PermissionData[0].RoleName;
                var viewPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.View).ToList();
                var addPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.Add).ToList();
                var editPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.Update).ToList();
                var viewUsersPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.ViewUsers).ToList();
                var deletePermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.Delete).ToList();
                var viewSecurityPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.ViewPermissions).ToList();
                if (addPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    ViewBag.AddSecurityRolePermission = true;
                }
                if (editPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    TempData["EditSecurityRolePermission"] = true;
                }
                if (viewUsersPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    TempData["ViewUsersSecurityRolePermission"] = true;
                }
                if (deletePermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    TempData["DeleteSecurityRolePermission"] = true;
                }
                if (viewSecurityPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    TempData["ViewAuthRolePermission"] = true;
                }
                if (viewPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    TempData["ViewSecurityRolePermission"] = true;
                    var response = await HttpRequestFactory.Get(string.Format("{0}{1}?PageNumber={2}&PageSize={3}&CompanyLocationId={4}&CompanyId={5}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_RolesJoinedUser, _appSettings.PageSettings.DefaultPageNumber, _appSettings.PageSettings.DefaultPageSize,_sessionManager.CompanyLocationID,_sessionManager.CompanyId), _sessionManager.BearerToken);
                    var All_AuthJoinedUser = response.ContentAsType<List<ResultRole>>();
                    if (All_AuthJoinedUser[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                    {
                        return Json(All_AuthJoinedUser[Constants.WebConstants.ResponseType]);
                    }
                    var model = new SecurityProfile();
                    model.ResultProfile = All_AuthJoinedUser[Constants.WebConstants.Data].ToObject<List<ResultRole>>();
                    TempData["Auth_RoleName"] = _sessionManager.USERAuthRole;
                    if (All_AuthJoinedUser != null)
                    {
                        return PartialView(Constants.WebConstants.PartialViews.SecurityProfile, model.ResultProfile);
                    }
                }
            }            
            return PartialView(Constants.WebConstants.PartialViews.SecurityProfile, null);
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> CreateNewProfile()
        {
            SecurityProfile model = new SecurityProfile();
            return PartialView(Constants.WebConstants.PartialViews.NewProfile, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionPermissionFilter]
        public async Task<IActionResult> CreateNewProfile(InputProfile inputProfile)
        {
            if (ModelState.IsValid)
            {
                inputProfile.CreatedBy = Guid.Parse(_sessionManager.ID);
                inputProfile.CompanyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
                inputProfile.CreatedOn = DateTime.Now;
                var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_CreateRole), inputProfile, _sessionManager.BearerToken);
                var result = response.ContentAsType<Auth_Role>();
                if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(result[Constants.WebConstants.ResponseType]);
                }
                if (result != null)
                {
                    return Json(result[Constants.WebConstants.ResponseType]);
                }
            }
            return Redirect(Constants.WebConstants.Urls.WEB_UserIndex);
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> ProfilePermissions(Guid id, string auth_role,Guid ProductId)
        {
            try
            {
                List<Auth_FeatureListWithGeneralCodeByUserIdResult> PermissionData = (List<Auth_FeatureListWithGeneralCodeByUserIdResult>)HttpContext.Items["PermissionData"];
                if (PermissionData != null && PermissionData.Count > 0)
                {
                    var RoleName = PermissionData[0].RoleName;
                    var updateSecurityPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.UpdatePermissions).ToList();
                    var viewUsersPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.ViewUsers).ToList();
                    var viewSecurityPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Roles && x.CodeValue == Constants.PermissionConstants.CodeValue.ViewPermissions).ToList();
                    if (updateSecurityPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        if (auth_role != _sessionManager.USERAuthRole && auth_role.ToLower() != Constants.WebConstants.Owner)
                        {
                            ViewBag.EditSecurityPermission = true;
                        }
                    }
                    if (viewUsersPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        ViewBag.ViewUsersSecurityRolePermission = true;
                    }
                    if (viewSecurityPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        var response = await HttpRequestFactory.Get(string.Format("{0}{1}?id={2}&userId={3}&ProductId={4}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetProfilePermissionsByAuth_RoleId, id, _sessionManager.ID, ProductId), _sessionManager.BearerToken);
                        var ProfilePermission = response.ContentAsType<List<Auth_FeatureListWithGeneralCodeByUserIdResult>>();
                        if (ProfilePermission[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                        {
                            return Json(ProfilePermission[Constants.WebConstants.ResponseType]);
                        }
                        if (ProfilePermission != null)
                        {
                            ViewBag.auth_role = auth_role;
                            ViewBag.auth_roleId = id;
                            List<Auth_FeatureListWithGeneralCodeByUserIdResult> model = new List<Auth_FeatureListWithGeneralCodeByUserIdResult>();
                            List<Auth_FeatureListWithGeneralCode> auth_featureList = new List<Auth_FeatureListWithGeneralCode>();
                            model = ProfilePermission[Constants.WebConstants.Data].ToObject<List<Auth_FeatureListWithGeneralCodeByUserIdResult>>();
                            foreach (var feature in model.Where(x => x.GeneralCodeId != null).GroupBy(x => x.FeatureName).Select(grp => grp.First()))
                            {
                                auth_featureList.Add(new Auth_FeatureListWithGeneralCode()
                                {
                                    FeatureId = feature.AuthFeatureId,
                                    FeatureName = feature.FeatureName
                                });
                            };
                            foreach (var f in auth_featureList)
                            {
                                if (f.FeatureId == model.Where(x => x.AuthFeatureId == f.FeatureId).Select(x => x.AuthFeatureId).First())
                                    foreach (var c in model.Where(x => x.AuthFeatureId == f.FeatureId).GroupBy(x => x.GeneralCodeId).Select(x => x.First()))
                                    {
                                        f.GeneralCodes.Add(new Model.WrapperModels.GeneralCode()
                                        {
                                            CodeId = c.GeneralCodeId,
                                            CodeName = c.CodeValue,
                                            IsDeleted = c.IsDeleted
                                        });
                                        if (!(c.IsDeleted))
                                            f.ActiveFeatures += c.CodeValue + ", ";
                                    }
                                if (f.ActiveFeatures != null)
                                {
                                    f.ActiveFeatures = f.ActiveFeatures.Remove(f.ActiveFeatures.Length - 2);
                                    f.IsActive = true;
                                }
                            }
                            return PartialView(Constants.WebConstants.PartialViews.ProfilePermissions, auth_featureList);
                        }
                    }
                }

                return PartialView(Constants.WebConstants.PartialViews.SecurityProfile, null);
                //return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(AccountController.Login), Constants.WebConstants.Controllers.Account);
            }
        }

        [HttpGet]
        [ActionPermissionFilter]
        public async Task<IActionResult> EditProfile(Guid id)
        {
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
            Constants.WebConstants.Urls.API_GetRoleById, id), _sessionManager.BearerToken);

            var profiles = response.ContentAsType<InputProfile>();
            var GetInputProfile = profiles[Constants.WebConstants.Data].ToObject<InputProfile>();
            if (profiles != null)
            {
                SecurityProfile model = new SecurityProfile();
                model.inputProfile = new InputProfile();
                model.inputProfile = GetInputProfile;
                return PartialView(Constants.WebConstants.PartialViews.NewProfile, model);
            }
            return View();
        }

        [HttpPost]
        [ActionPermissionFilter]
        public async Task<IActionResult> UpdateProfile(Guid id, InputProfile inputProfile)
        {
            try
            {
                inputProfile.ModifiedBy = Guid.Parse(_sessionManager.ID);
                var response = await HttpRequestFactory.Put(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                  Constants.WebConstants.Urls.API_UpdateRole, id), inputProfile, _sessionManager.BearerToken);

                var result = response.ContentAsType<Auth_Role>();
                if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(result[Constants.WebConstants.ResponseType]);
                }
                if (result != null)
                {
                    return Json(result[Constants.WebConstants.ResponseType]);
                }
                return Redirect(Constants.WebConstants.Urls.WEB_UserIndex);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(AccountController.Login), Constants.WebConstants.Controllers.Account);
            }
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> ProfileUsers(Guid id, string QuerySearch = "")
        {
            try
            {
                var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetRoleUsers, id), _sessionManager.BearerToken);
                var result = response.ContentAsType<List<ResultRoleUser>>();
                if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(result[Constants.WebConstants.ResponseType]);
                }
                if (result != null)
                {
                    List<ResultRoleUser> profileUsers = result[Constants.WebConstants.Data].ToObject<List<ResultRoleUser>>();
                    if (!string.IsNullOrEmpty(QuerySearch))
                    {
                        QuerySearch = QuerySearch.ToLower();
                        profileUsers = profileUsers.Where(a => (a.UserName.ToLower().Contains(QuerySearch) || a.Role.ToLower().Contains(QuerySearch))).ToList();
                    }
                    ViewBag.profileID = id;
                    return PartialView(Constants.WebConstants.PartialViews.ProfileUsers, profileUsers);
                }
                return PartialView(Constants.WebConstants.PartialViews.SecurityProfile, null);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(AccountController.Login), Constants.WebConstants.Controllers.Account);
            }
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> DeleteProfile(Guid id)
        {
            try
            {
                var response = await HttpRequestFactory.Delete(string.Format("{0}{1}?id={2}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_DeleteRoleUsers, id), _sessionManager.BearerToken);
                //var result = response.ContentAsType<List<Designation>>();
                var result = response.ContentAsType<object>();
                if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(result[Constants.WebConstants.ResponseType]);
                }
                if (result != null)
                {
                    var retVal = result[Constants.WebConstants.Data];
                    object data = new { returnValue = (int)retVal["returnValue"], errorMessage = (string)retVal["errorMessage"] };
                    return Json(data);
                    //return Json(result[Constants.WebConstants.ResponseType]);
                }
                return Json(null);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(AccountController.Login), Constants.WebConstants.Controllers.Account);
            }
        }

        [ActionPermissionFilter]
        public async Task<object> GetProfileName(string roleName)
        {
            try
            {
                var companyLocationId = _sessionManager.CompanyLocationID;
                var response = await HttpRequestFactory.Get(string.Format("{0}{1}?name={2}&companyLocationId={3}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetRoleByName, roleName, companyLocationId), _sessionManager.BearerToken);

                var result = response.ContentAsType<Auth_Role>();
                if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(result[Constants.WebConstants.ResponseType]);
                }
                if (result != null)
                {
                    if (result[Constants.WebConstants.Data] != null)
                    {
                        string message = Constants.Messages.RoleAlreadyExist;
                        return message;
                    }
                    return Constants.Messages.Success;
                }
                return Constants.Messages.EnterName;
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(AccountController.Login), Constants.WebConstants.Controllers.Account);
            }
        }
        #endregion

        #region Feature Permissions
        [HttpPost]
        [ActionPermissionFilter]
        public async Task<IActionResult> UpdateFeaturePermission(InputAuth_RoleFeatureMappingTemp obj)
        {
            obj.SessionId = Guid.Parse(_sessionManager.ID);
            obj.CreatedBy = Guid.Parse(_sessionManager.ID);
            var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_PostInsertUpdateAuth_RoleFeatureMappingTemp), obj, _sessionManager.BearerToken);
            var result = response.ContentAsType<Auth_RoleFeatureMappingTemp>();
            var rowData = result[Constants.WebConstants.Data].ToObject<List<Auth_RoleFeatureMappingTemp>>();
            var retObj = new
            {
                ResponseType = result[Constants.WebConstants.ResponseType],
                RowData = rowData
            };

            return Json(retObj);
        }

        [HttpPost]
        [ActionPermissionFilter]
        public async Task<IActionResult> BulkUpdateFeaturePermissions(InputAuth_RoleFeatureMappingTemp obj)
        {
            obj.SessionId = Guid.Parse(_sessionManager.ID);
            obj.CreatedBy = Guid.Parse(_sessionManager.ID);
            var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_PostBulkUpdateAuth_RoleFeatureMappingTemp), obj, _sessionManager.BearerToken);
            var result = response.ContentAsType<Auth_RoleFeatureMappingTemp>();
            var rowData = result[Constants.WebConstants.Data].ToObject<List<Auth_RoleFeatureMappingTemp>>();
             var retObj = new
             {
                 ResponseType = result[Constants.WebConstants.ResponseType],
                 RowData = rowData
             };

            return Json(retObj);
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> UpdateAuthRoleFeatureMappingChanges(Guid roleId)
        {
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}?roleId={2}&userId={3}", _appSettings.Leemo_API_Config.BaseUrl,
            Constants.WebConstants.Urls.API_PostAuth_RoleFeatureMappingChanges, roleId, _sessionManager.ID), _sessionManager.BearerToken);
            var result = response.ContentAsType<int>();
            return Json(result[Constants.WebConstants.ResponseType]);
        }
        #endregion

        #region Designation        

        string htmlString;
        [ActionPermissionFilter]
        public async Task<IActionResult> getDesignation()
        {
            try
            {
                List<Auth_FeatureListWithGeneralCodeByUserIdResult> PermissionData = (List<Auth_FeatureListWithGeneralCodeByUserIdResult>)HttpContext.Items["PermissionData"];
                if (PermissionData != null && PermissionData.Count > 0)
                {
                    var RoleName = PermissionData[0].RoleName;
                    var viewPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Designation && x.CodeValue == Constants.PermissionConstants.CodeValue.View).ToList();
                    var addPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Designation && x.CodeValue == Constants.PermissionConstants.CodeValue.Add).ToList();
                    var editPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.SecurityControls_Designation && x.CodeValue == Constants.PermissionConstants.CodeValue.Update).ToList();
                    if (addPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        ViewBag.AddDesignationPermission = true;
                    }
                    if (editPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        ViewBag.EditDesignationPermission = true;
                    }
                    if (viewPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        ViewBag.ViewDesignationPermission = true;
                        var response = await HttpRequestFactory.Get(string.Format("{0}{1}?PageNumber={2}&PageSize={3}&CompanyLocationId={4}", _appSettings.Leemo_API_Config.BaseUrl,
                        Constants.WebConstants.Urls.API_GetDesignationHierarchy, _appSettings.PageSettings.DefaultPageNumber, _appSettings.PageSettings.DefaultPageSize, _sessionManager.CompanyLocationID), _sessionManager.BearerToken);
                        var result = response.ContentAsType<List<DesignationHierarchy>>();
                        if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                        {
                            return Json(result[Constants.WebConstants.ResponseType]);
                        }
                        Designation model = new Designation();
                        List<DesignationHierarchy> obj_designationHierarchy = new List<DesignationHierarchy>();
                        obj_designationHierarchy.AddRange(result[Constants.WebConstants.Data].ToObject<List<DesignationHierarchy>>());
                        var designationHierarchyTree = buildDesignationTree(obj_designationHierarchy);
                        htmlString = "";
                        if (designationHierarchyTree.FirstOrDefault().Children.Count() > 1)
                            htmlString += "<li class=\"has-children\">";
                        else
                            htmlString += "<li>";

                        //htmlString += "<div class=\"accordion-toggle no-border\">Tech Prastish Pvt.Ltd.</div>";
                        htmlString += "<div class=\"accordion-toggle no-border\"  onmouseover=\"fnOnHoverShow(this)\" onmouseout=\"fnOnHoverHide(this)\" data-id=" + designationHierarchyTree.FirstOrDefault().Designation.Id + ">";
                        htmlString += designationHierarchyTree.FirstOrDefault().Designation.Name;
                        //if (designationHierarchyTree.FirstOrDefault().Designation.Description!=string.Empty && designationHierarchyTree.FirstOrDefault().Designation.Description != null) 
                        //{
                        //    htmlString += "<span>(" + designationHierarchyTree.FirstOrDefault().Designation.Description + ")</span>";
                        //}
                        htmlString += "<div class=\"role-edit-options\">";
                        htmlString += "<a href=\"javascript: void(0);\" class=\"btnSwap\" data-tippy-content=\"Add Designation\" onclick=\"newDesignationPopUp('" + designationHierarchyTree.FirstOrDefault().Designation.Id + "');\"><img src=\"/images/plus-d.svg\" /></a>";
                        htmlString += "<a href=\"javascript: void(0);\" class=\"btnSwap\" data-tippy-content=\"View Users\" onclick=\"DesignationUsers('" + designationHierarchyTree.FirstOrDefault().DesignationId + "');\"><img src=\"/images/view-u.svg\" /></a>";
                        htmlString += "</div>";
                        htmlString += "</div>";
                        buildDesignationTreeHTML(designationHierarchyTree.FirstOrDefault().Children);
                        htmlString += "</li>";
                        obj_designationHierarchy.FirstOrDefault().DesignationListHTML = htmlString;
                        return PartialView(Constants.WebConstants.PartialViews.DesignationDetails, obj_designationHierarchy);
                    }
                }
                return PartialView(Constants.WebConstants.PartialViews.DesignationDetails, null);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", Constants.WebConstants.Controllers.Account);
            }
        }

        private string buildDesignationTreeHTML(List<DesignationHierarchy> designations)
        {
            htmlString += "<div class=\"accordion-inner\"><ul>";
            if (designations.Count > 0)
            {
                foreach(var designation in designations)
                {
                    if ((designation.Children.Count() > 0 && designation != designations.Last()) || designations.Count() > 1)
                    {
                        if(designation == designations.First() || designations.Count() > 1)
                        {
                            htmlString += "<li class=\"has-children\">";
                        }
                        else
                            htmlString += "<li>";
                    }
                    else
                        htmlString += "<li>";

                    if (designation == designations.FirstOrDefault())
                        htmlString += "<div class=\"accordion-toggle\" onmouseover=\"fnOnHoverShow(this)\" onmouseout=\"fnOnHoverHide(this)\" data-id=" + designation.Designation.Id + ">";
                    else if(designation.Children.Count() == 0)
                        htmlString += "<div class=\"accordion-toggle no-border\" onmouseover=\"fnOnHoverShow(this)\" onmouseout=\"fnOnHoverHide(this)\" data-id=" + designation.Designation.Id + ">";
                    else
                        htmlString += "<div class=\"accordion-toggle no-border\" onmouseover=\"fnOnHoverShow(this)\" onmouseout=\"fnOnHoverHide(this)\" data-id=" + designation.Designation.Id + ">";

                    htmlString += designation.Designation.Name;
                    //if (designation.Designation.Description != string.Empty && designation.Designation.Description != null)
                    //{
                    //    htmlString += "<span>(" + designation.Designation.Description + ")</span>";
                    //}
                    htmlString += "<div class=\"role-edit-options\">";
                    htmlString += "<a href=\"javascript: void(0);\" class=\"btnSwap\" data-tippy-content=\"Add Designation\" onclick=\"newDesignationPopUp('" + designation.Designation.Id + "');\"><img src=\"/images/plus-d.svg\" /></a>";
                    htmlString += "<a href=\"javascript: void(0);\" class=\"btnSwap\" data-tippy-content=\"View Users\" onclick=\"DesignationUsers('" + designation.DesignationId + "');\"><img src=\"/images/view-u.svg\" /></a>";
                    htmlString += "<a href=\"javascript: void(0);\" data-tippy-content=\"Edit\" onclick=\" EditDesignation('" + designation.DesignationId + "')\"><img src=\"/images/edit_btn.svg\" /></a>";
                    htmlString += "<a href=\"javascript: void(0);\" data-tippy-content=\"Delete\" onclick=\" DeleteDesignation('" + designation.DesignationId + "')\"><img src=\"/images/delete_btn.svg\" /></a>";
                    htmlString += "</div>";
                    htmlString += "</div>";
                    if (designation.Children.Count() > 0)
                    {
                        buildDesignationTreeHTML(designation.Children);
                    }
                        
                    htmlString += "</li>";
                }
                
            }
            htmlString += "</ul></div>";
            return htmlString;
        }

        private List<DesignationHierarchy> buildDesignationTree(List<DesignationHierarchy> items)
        {
            items.ForEach(i => i.Children = items.Where(ch => ch.ParentDesignationId == i.DesignationId).ToList());
            return items.Where(i => i.ParentDesignationId == Guid.Parse(Constants.ApiConstants.RootDesignationId)).ToList();
        }

        [ActionPermissionFilter]
        public IActionResult CreateDesignation()
        {
           return PartialView(Constants.WebConstants.PartialViews.DesignationCreate, null);
        }

        [HttpPost]
        [ActionPermissionFilter]
        public async Task<IActionResult> CreateDesignation(Designation designation)
        {
            if (ModelState.IsValid)
            {
                designation.CompanyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
                var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                     Constants.WebConstants.Urls.API_InsertDesignation), designation, _sessionManager.BearerToken);
                var result = response.ContentAsType<List<Designation>>();
                if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(result[Constants.WebConstants.ResponseType]);
                }
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            return Json(Constants.ResponseType.Error);
        }
        
        [HttpGet]
        [ActionPermissionFilter]
        public async Task<IActionResult> getDesignationByID(Guid id)
        {
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
              Constants.WebConstants.Urls.API_GetDesignationById, id), _sessionManager.BearerToken);
            var result = response.ContentAsType<List<Designation>>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            var GetInputDesignation = result[Constants.WebConstants.Data].ToObject<Designation>();

            Designation model = new Designation();
            DesignationHierarchy model1 = new DesignationHierarchy();
            model1.Designation = new Designation();

            List<DesignationHierarchy> obj_designationHierarchy = new List<DesignationHierarchy>();
            model1.Designation = GetInputDesignation;
            model = GetInputDesignation;
            return PartialView(Constants.WebConstants.PartialViews.DesignationCreate, model);

        }

        [HttpPost]
        [ActionPermissionFilter]
        public async Task<IActionResult> UpdateDesignationInfo(Guid id,Designation designation)
        {
            if (ModelState.IsValid)
            {
                var response = await HttpRequestFactory.Put(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_UpdateDesignationById, id), designation, _sessionManager.BearerToken);
                var result = response.ContentAsType<List<Designation>>();
                if (result != null)
                {
                    return Json(result[Constants.WebConstants.ResponseType]);
                }
                return Redirect(Constants.WebConstants.Urls.WEB_UserIndex);
            }
            return Json(Constants.ResponseType.Error);
        }

        [ActionPermissionFilter]
        public async Task<object> DeleteDesignations(Guid id)
        {
            var response = await HttpRequestFactory.Delete(string.Format("{0}{1}?id={2}", _appSettings.Leemo_API_Config.BaseUrl,
            Constants.WebConstants.Urls.API_DeleteDesignations, id), _sessionManager.BearerToken);
            var result = response.ContentAsType<object>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            if (result != null)
            {
                var retVal = result[Constants.WebConstants.Data];
                object data = new { returnValue=(int)retVal["returnValue"], errorMessage = (string)retVal["errorMessage"] };
                return Json(data);
            }
            return Json(null);
        }

        [HttpPost]
        [ActionPermissionFilter]
        public async Task<IActionResult> UpdateDesignationHierarchy(DesignationHierarchy model)
        {
                var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_InsertDesignationHierarchy), model, _sessionManager.BearerToken);
                var result = response.ContentAsType<Designation>();
            if(result!= null)        
                return Json(result[Constants.WebConstants.ResponseType]);
            else
                return Json(Constants.ResponseType.Error);
        }

        [ActionPermissionFilter]
        public async Task<string> GetDesignationName(string designationName)
        {
            var CompanyLocationID = _sessionManager.CompanyLocationID;
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}?name={2}&companyLocationId={3}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_GetDesignationByName, designationName, CompanyLocationID), _sessionManager.BearerToken);
            var result = response.ContentAsType<ResultUser>();
            if (result != null)
            {
                if (result[Constants.WebConstants.Data] != null)
                {
                    string message = Constants.Messages.DesignationAlreadyExist;
                    return message;
                }
                return Constants.Messages.Success;
            }
            return Constants.Messages.EnterName;
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> DesignationUsers(Guid id, string QuerySearch = "")
        {
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}?DesignationId={2}&CompanyId={3}", _appSettings.Leemo_API_Config.BaseUrl,
                 Constants.WebConstants.Urls.API_GetDesignationUsers, id, _sessionManager.CompanyId), _sessionManager.BearerToken);
            var result = response.ContentAsType<List<ResultDesignationUser>>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            if (result != null)
            {
                if (result[Constants.WebConstants.Data] != null)
                {
                    List<ResultDesignationUser> designationUsers = result[Constants.WebConstants.Data].ToObject<List<ResultDesignationUser>>();
                    if (!string.IsNullOrEmpty(QuerySearch))
                    {
                        QuerySearch = QuerySearch.ToLower();
                        designationUsers = designationUsers.Where(a => a.UserName.ToLower().Contains(QuerySearch)).ToList();
                    }
                    ViewBag.designation = id;
                    return PartialView(Constants.WebConstants.PartialViews.DesignationUsers, designationUsers);
                }
            }
            return PartialView(Constants.WebConstants.PartialViews.DesignationUsers, null);
        }
        #endregion

        #region DO NOT DELETE
        //[HttpPost]
        //public async Task<IActionResult> CreateDesignation(Designation designation)
        //{
        //    DesignationHierarchy model = new DesignationHierarchy();
        //    if (ModelState.IsValid)
        //    {
        //        Designation destinationObj = new Designation();
        //        var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
        //             Constants.WebConstants.Urls.API_InsertDesignation), designation, _sessionManager.BearerToken);
        //        var result = response.ContentAsType<List<Designation>>();
        //        if (result[Constants.WebConstants.Data] != null)
        //        {
        //            var id = result[Constants.WebConstants.Data].id;
        //            var response1 = await HttpRequestFactory.Get(string.Format("{0}{1}?PageNumber={2}&PageSize={3}", _appSettings.Leemo_API_Config.BaseUrl,
        //            Constants.WebConstants.Urls.API_GetDesignationHierarchy, _appSettings.PageSettings.DefaultPageNumber, _appSettings.PageSettings.DefaultPageSize), _sessionManager.BearerToken);
        //            var result1 = response1.ContentAsType<List<DesignationHierarchy>>();
        //            var response2 = await HttpRequestFactory.Get(string.Format("{0}{1}?PageNumber={2}&PageSize={3}"
        //                    , _appSettings.Leemo_API_Config.BaseUrl
        //                    , Constants.WebConstants.Urls.API_GetDesignationHierarchy
        //                    , _appSettings.PageSettings.DefaultPageNumber
        //                    , _appSettings.PageSettings.DefaultPageSize), _sessionManager.BearerToken);
        //            var result2 = response2.ContentAsType<List<DesignationHierarchy>>();
        //            List<DesignationHierarchy> obj_designationHierarchy = new List<DesignationHierarchy>();
        //            obj_designationHierarchy.AddRange(result2[Constants.WebConstants.Data].ToObject<List<DesignationHierarchy>>());
        //            var designationHierarchyTree = buildDesignationTree(obj_designationHierarchy);
        //            var lastChildDesignationId = obj_designationHierarchy.Where(i => i.Children.Count == 0)
        //                                            .FirstOrDefault().DesignationId;
        //            model.DesignationId = id;
        //            model.ParentDesignationId = lastChildDesignationId;
        //            model.SortOrder = 0;
        //            var response3 = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
        //              Constants.WebConstants.Urls.API_InsertDesignationHierarchy), model, _sessionManager.BearerToken);
        //            var result3 = response3.ContentAsType<List<DesignationHierarchy>>();
        //            return Json(result3[Constants.WebConstants.ResponseType]);
        //        }
        //        else
        //        {
        //            return Json(new { ResponseType=result[Constants.WebConstants.ResponseType],Message= result[Constants.WebConstants.Message]});
        //        }
        //    }
        //    return Json(Constants.ResponseType.Error);
        //}
        //public async Task<IActionResult> EditDesignation()
        //{
        //    var response = await HttpRequestFactory.Get(string.Format("{0}{1}?PageNumber={2}&PageSize={3}"
        //        , _appSettings.Leemo_API_Config.BaseUrl
        //        , Constants.WebConstants.Urls.API_GetDesignationHierarchy
        //        , _appSettings.PageSettings.DefaultPageNumber
        //        , _appSettings.PageSettings.DefaultPageSize), _sessionManager.BearerToken);

        //    var result = response.ContentAsType<List<DesignationHierarchy>>();
        //    Designation model = new Designation();
        //    List<DesignationHierarchy> obj_designationHierarchy = new List<DesignationHierarchy>();
        //    obj_designationHierarchy.AddRange(result[Constants.WebConstants.Data].ToObject<List<DesignationHierarchy>>());
        //    var designationHierarchyTree = buildDesignationTree(obj_designationHierarchy);
        //    htmlString = "";
        //    buildDesignationTreeHTMLForEdit(designationHierarchyTree.FirstOrDefault().Children);
        //    designationHierarchyTree.FirstOrDefault().DesignationListHTML = htmlString;
        //    return PartialView("_EditDesignationHierarchy", designationHierarchyTree);
        //}

        //private string buildDesignationTreeHTMLForEdit(List<DesignationHierarchy> designations)
        //{
        //    if (designations.Count > 0)
        //    {
        //        htmlString += "<div class=\"role-list-content my-role\" draggable='true' desig=\"" + designations.FirstOrDefault().Designation.Id + "\" >";
        //        htmlString += designations.FirstOrDefault().Designation.Name + "<span>(" + designations.FirstOrDefault().Designation.Description + ")</span>";
        //        htmlString += "<span class=\"move-icon\">";
        //        htmlString += "     <img src=\"/images/move.svg\" alt=\"move\" title=\"move\">";
        //        htmlString += "</span>";
        //        htmlString += "<a href='javascript: void(0);' class='position-edit' onclick=\"EditDesignation('" + designations.FirstOrDefault().Designation.Id + "')\"> ";
        //        htmlString += "     <img src=\"/images/edit.svg\" alt=\"Edit\" title=\"Edit\"></a>";
        //        htmlString += "</div>";
        //        foreach (var child in designations)
        //        {
        //            if (child.Children.Count > 0)
        //            {
        //                buildDesignationTreeHTMLForEdit(child.Children);
        //            }
        //        }
        //    }
        //    return htmlString;
        //}

        //public async Task<IActionResult> UpdateDesignationTree(string designationIds)
        //{
        //    var designationHierarchies = new List<DesignationHierarchy>();
        //    var designationIdsArray = designationIds.Split(',', options: StringSplitOptions.RemoveEmptyEntries);
        //    string rootNode = designationIdsArray[0];
        //    string parentNode = rootNode;
        //    for (int i = 1; i < designationIdsArray.Count(); i++)
        //    {
        //        string childNode = designationIdsArray[i];
        //        var designationHierarchy = new DesignationHierarchy();
        //        designationHierarchy.DesignationId = Guid.Parse(childNode);
        //        designationHierarchy.ParentDesignationId = Guid.Parse(parentNode);
        //        designationHierarchies.Add(designationHierarchy);
        //        parentNode = childNode;
        //    }

        //    var response = await HttpRequestFactory.Post(string.Format("{0}{1}"
        //        , _appSettings.Leemo_API_Config.BaseUrl
        //        , Constants.WebConstants.Urls.API_ResetDesignationHierarchy), designationHierarchies, _sessionManager.BearerToken);
        //    var result = response.ContentAsType<bool>();
        //    return Json("{result:1}");
        //}
        #endregion
    }
}
