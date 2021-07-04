using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
    [Authorize]
    public class GroupController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly SessionManager _sessionManager;

        public GroupController(IOptions<AppSettings> appSettings, SessionManager sessionManager)
        {
            _appSettings = appSettings.Value;
            _sessionManager = sessionManager;
        }
        [ActionPermissionFilter]
        public async Task<IActionResult> Index()
        {
            try
            {
                Guid companyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
                var model = new GroupsAndUsers();
                List<Auth_FeatureListWithGeneralCodeByUserIdResult> PermissionData = (List<Auth_FeatureListWithGeneralCodeByUserIdResult>)HttpContext.Items["PermissionData"];
                if (PermissionData != null && PermissionData.Count > 0)
                {
                    var RoleName = PermissionData[0].RoleName;
                    var addPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.Groups && x.CodeValue == Constants.PermissionConstants.CodeValue.Add).ToList();
                    var viewPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.Groups && x.CodeValue == Constants.PermissionConstants.CodeValue.View).ToList();
                    if (addPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        ViewBag.addGroupPermission = true;
                    }
                    if (viewPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                    {
                        ViewBag.viewGroupPermission = true;
                        var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                         Constants.WebConstants.Urls.API_GetGroupCounts, companyLocationId), _sessionManager.BearerToken);
                        var result = response.ContentAsType<Dictionary<string, int>>();
                        ViewBag.AllGroups = Convert.ToInt32(result["data"].All);
                        ViewBag.ActiveGroups = Convert.ToInt32(result["data"].Active);
                        ViewBag.InActiveGroups = Convert.ToInt32(result["data"].InActive);
                        //var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                        // Constants.WebConstants.Urls.API_GetAllGroupsByLocation,companyLocationId), _sessionManager.BearerToken);
                        // var result = response.ContentAsType<List<Group>>();
                        //model.ResultGroup = result[Constants.WebConstants.Data].ToObject<List<ResultGroup>>();
                        //ViewBag for Show active and all User
                        //ViewBag.ActiveGroups = model.ResultGroup.Count(x => x.IsActive);
                        //ViewBag.AllGroups = model.ResultGroup.FirstOrDefault();
                        //if (result != null)
                        //{
                        return View(model);
                        //}
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", Constants.WebConstants.Controllers.Account);
            }
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> CreateGroup()
        {
            return PartialView("_CreateGroup", new InputGroup());
        }

        [HttpPost]
        [ActionPermissionFilter]
        public async Task<IActionResult> CreateGroup(InputGroup group)
        {
            if (ModelState.IsValid)
            {
                group.GroupUsers = new List<InputGroupUser>();
                group.GroupRoles = new List<InputGroupRole>();
                group.GroupsMapping = new List<InputGroupGroupsMapping>();
                group.IsActive = true;
                group.CompanyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
                if (group.UserIds != null)
                {
                    foreach (var userids in group.UserIds)
                    {
                        List<InputGroupUser> users = new List<InputGroupUser>
                    {
                        new InputGroupUser{ UserId=userids }
                    };
                        group.GroupUsers.AddRange(users);
                    }
                }
                if (group.RoleIds != null)
                {
                    foreach (var rolesids in group.RoleIds)
                    {
                        List<InputGroupRole> roles = new List<InputGroupRole>
                    {
                        new InputGroupRole{ RoleId=rolesids }
                    };
                        group.GroupRoles.AddRange(roles);
                    }
                }
                if (group.GroupMappingIds != null)
                {
                    foreach (var mappinggroupids in group.GroupMappingIds)
                    {
                        List<InputGroupGroupsMapping> groups = new List<InputGroupGroupsMapping>
                    {
                        new InputGroupGroupsMapping{ MappedGroupId= mappinggroupids }
                    };
                        group.GroupsMapping.AddRange(groups);
                    }
                }
                var response = await HttpRequestFactory.Post(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_PostGroup), group, _sessionManager.BearerToken);                
                var result = response.ContentAsType<InputGroup>();
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

        [ActionPermissionFilter]
        public async Task<string> GetGroupName(string groupName)
        {
            var companyLocationId = _sessionManager.CompanyLocationID;
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}?groupName={2}&companyLocationId={3}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_GetGroupName, groupName, companyLocationId), _sessionManager.BearerToken);            
            var result = response.ContentAsType<ResultUser>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            if (result != null)
            {
                if (result[Constants.WebConstants.Data] != null)
                {
                    string message = Constants.Messages.GroupAlreadyExist;
                    return message;
                }
                return Constants.Messages.Success;
            }
            return Constants.Messages.EnterName;
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> GroupList(string QuerySearch = "", int GetActiveGroups = 1)
        {
            Guid companyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}?PageNumber={3}&PageSize={4}&GetActiveGroups={5}&QuerySearch={6}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_GetAllGroupsByLocation,companyLocationId, _appSettings.PageSettings.DefaultPageNumber, _appSettings.PageSettings.DefaultPageSize, GetActiveGroups, QuerySearch), _sessionManager.BearerToken);            
            var result = response.ContentAsType<List<ResultGroup>>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            //var response1 = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}?PageSize={3}&GetActiveGroups={4}&QuerySearch={5}", _appSettings.Leemo_API_Config.BaseUrl,
            //  Constants.WebConstants.Urls.API_GetAllGroupsByLocation,companyLocationId,1000,0,null), _sessionManager.BearerToken);

            var response1 = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetGroupCounts, companyLocationId), _sessionManager.BearerToken);
            var result1 = response1.ContentAsType<Dictionary<string, int>>();
            if (result1 != null)
            {
                ViewBag.AllGroups = Convert.ToInt32(result1["data"].All);
                ViewBag.ActiveGroups = Convert.ToInt32(result1["data"].Active);
                ViewBag.InActiveGroups = Convert.ToInt32(result1["data"].InActive);
            }
            if (result != null)
            {
                return PartialView(Constants.WebConstants.PartialViews.GroupList, result[Constants.WebConstants.Data].ToObject<List<ResultGroup>>());
            }
            return PartialView(Constants.WebConstants.PartialViews.GroupList, null);
        }
        
        [HttpPost]
        [ActionPermissionFilter]
        public async Task<object> UploadGroupImage(Leemo.Web.Models.Common.ImageUpload model)
        {
            string message = Constants.Messages.BadRequest;
            if (model.Id == Guid.Empty)
                return message;
            if (model.ImageFile != null)
            {
                long FileSize = model.ImageFile.Length;
                if (FileSize < _appSettings.MaxImageSize)
                {
                    InputUpdateGroupImage groupImageinput = new InputUpdateGroupImage();
                    string filepath = string.Concat(_appSettings.Resources_BaseDir, _appSettings.GroupImagesPath);
                    string filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    string extension = Path.GetExtension(model.ImageFile.FileName).ToLower();
                    filename = filename + DateTime.Now.ToString("yyssmmfff") + extension;
                    if (extension == Constants.Extensions.PNG || extension == Constants.Extensions.JPG || extension == Constants.Extensions.JPEG)
                    {
                        //API call for updating Image name in Group
                        groupImageinput.GroupId = model.Id;
                        groupImageinput.ImageName = filename;
                        var response =await HttpRequestFactory.Put(
                            string.Concat(_appSettings.Leemo_API_Config.BaseUrl,
                            Constants.WebConstants.Urls.API_Group_UpdateGroupImage), groupImageinput, _sessionManager.BearerToken);
                        var result = response.ContentAsType<object>();
                        if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                        {
                            return Json(result[Constants.WebConstants.ResponseType]);
                        }
                        //Saving Image in Physical Path /Resources/GroupImages/
                        string path = Path.Combine(filepath, filename);
                        using (var filestream = new FileStream(path, FileMode.Create))
                        {
                            model.ImageFile.CopyTo(filestream);
                        }
                        message = Constants.Messages.GroupImageUpdated;
                        return filename;
                    }
                    else
                    {
                        message = "0";
                    }
                }
                else { message = "1"; }
            }
            return message;
           
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> GroupDetails(Guid id)
        {
            List<Auth_FeatureListWithGeneralCodeByUserIdResult> PermissionData = (List<Auth_FeatureListWithGeneralCodeByUserIdResult>)HttpContext.Items["PermissionData"];
            if (PermissionData != null && PermissionData.Count > 0)
            {
                var RoleName = PermissionData[0].RoleName;
                var editPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.Groups && x.CodeValue == Constants.PermissionConstants.CodeValue.Update).ToList();
                var viewPermission = PermissionData.Where(x => x.FeatureName == Constants.PermissionConstants.FeatureName.Groups && x.CodeValue == Constants.PermissionConstants.CodeValue.View).ToList();
                if (editPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    ViewBag.editGroupPermission = true;
                }
                if (viewPermission.Count != 0 || RoleName.ToLower() == Constants.WebConstants.Owner)
                {
                    var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetGroup, id), _sessionManager.BearerToken);
                    var response1 = await HttpRequestFactory.Get(string.Format("{0}{1}?PageNumber={2}&PageSize={3}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetAllUsersPersonal, _appSettings.PageSettings.DefaultPageNumber, _appSettings.PageSettings.DefaultPageSize), _sessionManager.BearerToken);
                    var users = response1.ContentAsType<List<ResultUser>>();
                    var result = response.ContentAsType<ResultGroup>();
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                    if (result != null)
                    {
                        ViewBag.Users = users[Constants.WebConstants.Data].ToObject<List<ResultUser>>();
                        return PartialView(Constants.WebConstants.PartialViews.GroupDetails, result[Constants.WebConstants.Data].ToObject<ResultGroup>());
                    }
                }
            }
            //return PartialView(Constants.WebConstants.PartialViews.GroupDetails);
            return Json(Constants.ResponseType.AccessDenied);
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> EditGroup(Guid id)
        {
            try
            {
                var response = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_GetGroup, id), _sessionManager.BearerToken);
                var result = response.ContentAsType<object>();
                if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                {
                    return Json(result[Constants.WebConstants.ResponseType]);
                }
                var response3 = await HttpRequestFactory.Get(string.Format("{0}{1}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetAllGroups), _sessionManager.BearerToken);

                var groups = response3.ContentAsType<List<Group>>();
                var group = response.ContentAsType<InputGroup>();
                if (group[Constants.WebConstants.Data] != null)
                {
                    InputGroup currentGroup = group[Constants.WebConstants.Data].ToObject<InputGroup>();
                    return PartialView(Constants.WebConstants.PartialViews.CreateGroup, currentGroup);
                }
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", Constants.WebConstants.Controllers.Account);
            }
        }

        [HttpPost]
        [ActionPermissionFilter]
        public async Task<IActionResult> UpdateGroup(InputGroup group,Guid id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    group.GroupUsers = new List<InputGroupUser>();
                    group.GroupRoles = new List<InputGroupRole>();
                    group.GroupsMapping = new List<InputGroupGroupsMapping>();

                    if (group.UserIds != null)
                    {
                        foreach (var userids in group.UserIds)
                        {
                            List<InputGroupUser> users = new List<InputGroupUser>
                    {
                        new InputGroupUser{ UserId=userids }
                    };
                            group.GroupUsers.AddRange(users);
                        }
                    }
                    if (group.RoleIds != null)
                    {
                        foreach (var rolesids in group.RoleIds)
                        {
                            List<InputGroupRole> roles = new List<InputGroupRole>
                    {
                        new InputGroupRole{ RoleId=rolesids }
                    };
                            group.GroupRoles.AddRange(roles);
                        }
                    }
                    if (group.GroupMappingIds != null)
                    {
                        foreach (var mappinggroupids in group.GroupMappingIds)
                        {
                            List<InputGroupGroupsMapping> groups = new List<InputGroupGroupsMapping>
                    {
                        new InputGroupGroupsMapping{ MappedGroupId= mappinggroupids }
                    };
                            group.GroupsMapping.AddRange(groups);
                        }
                    }
                    var response = await HttpRequestFactory.Put(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_UpdateGroup, id), group, _sessionManager.BearerToken);
                    var result = response.ContentAsType<ResultGroup>();
                    if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
                    {
                        return Json(result[Constants.WebConstants.ResponseType]);
                    }
                    if (result != null)
                    {
                        ViewBag.Message = result[Constants.WebConstants.Message];
                        if (result[Constants.WebConstants.Data] != null)
                        {
                            if (result[Constants.WebConstants.ResponseType] >= 0)
                            {
                                return Json(result[Constants.WebConstants.ResponseType]);
                            }
                            return PartialView(Constants.WebConstants.PartialViews.GroupDetails, result[Constants.WebConstants.Data].ToObject<ResultGroup>());
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
        public async Task<IActionResult> GroupInformation()
        {
            var response = await HttpRequestFactory.Get(string.Format("{0}{1}?PageNumber={2}&PageSize={3}&QuerySearch={4}", _appSettings.Leemo_API_Config.BaseUrl,
                Constants.WebConstants.Urls.API_GetAllGroups, _appSettings.PageSettings.DefaultPageNumber, _appSettings.PageSettings.DefaultPageSize, ""), _sessionManager.BearerToken);
            var result = response.ContentAsType<ResultGroup>();
            if (result[Constants.WebConstants.ResponseType] == Constants.ResponseType.AccessDenied)
            {
                return Json(result[Constants.WebConstants.ResponseType]);
            }
            if (result != null)
            {
                ViewBag.returnPageType = Constants.WebConstants.GroupSettings;
                return PartialView(Constants.WebConstants.PartialViews.GroupList, result[Constants.WebConstants.Data].ToObject<List<ResultGroup>>());
            }
            return View();
        }

        [ActionPermissionFilter]
        public async Task<IActionResult> GetGroupSourceData(int source)
        {
            try
            {
                Guid companyLocationId = Guid.Parse(_sessionManager.CompanyLocationID);
                if (source == 1)
                {
                    var response1 = await HttpRequestFactory.Get(string.Format("{0}{1}?PageNumber={2}&PageSize={3}&CompanyLocationId={4}", _appSettings.Leemo_API_Config.BaseUrl,
                        Constants.WebConstants.Urls.API_GetAllUsersPersonal, _appSettings.PageSettings.DefaultPageNumber, _appSettings.PageSettings.DefaultPageSize,companyLocationId), _sessionManager.BearerToken);
                    var users = response1.ContentAsType<List<ResultUser>>();
                    if (users != null)
                    {
                        List<ResultUser> allusers = users[Constants.WebConstants.Data].ToObject<List<ResultUser>>();
                        var userData = allusers.Select(x=>new { x.Id, x.UserName }).ToList();
                        return Json(userData);
                    }
                }
                if (source == 2)
                {
                    var response2 = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetPersonalDesignations, companyLocationId), _sessionManager.BearerToken);
                    var roles = response2.ContentAsType<List<Designation>>();
                    if (roles != null)
                    {
                        List<Designation> allroles = roles[Constants.WebConstants.Data].ToObject<List<Designation>>();
                        var roleData = allroles.Select(x => new { x.Id, x.Name }).ToList();
                        return Json(allroles);
                    }
                }
                if (source == 3)
                {
                    var response3 = await HttpRequestFactory.Get(string.Format("{0}{1}/{2}", _appSettings.Leemo_API_Config.BaseUrl,
                    Constants.WebConstants.Urls.API_GetAllGroupsByLocation,companyLocationId), _sessionManager.BearerToken);
                    var groups = response3.ContentAsType<List<Group>>();
                    if (groups != null)
                    {
                        List<Group> allgroups = groups[Constants.WebConstants.Data].ToObject<List<Group>>();
                        var groupData = allgroups.Where(x=>x.IsActive==true).Select(x => new { x.Id, x.Name }).ToList();
                        //return Json(allgroups);
                        return Json(groupData);
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                return Json("");
            }
        }

    }
}
