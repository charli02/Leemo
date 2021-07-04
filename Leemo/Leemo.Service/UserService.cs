using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TPSS.Common;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Model.WrapperModels;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;

/// <summary>
/// Represents Leemo service project namespace
/// </summary>
namespace Leemo.Service
{
    /// <summary>
    /// Represnets user serivce class which interact with repository.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly ICompanyLocationUserMappingRepository _companyLocationUserMappingRepository;
        private readonly ICompanyLocationUserMappingService _companyLocationUserMappingService;
        private readonly IUserRepository _userRepository;
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly IAuth_RoleUserMappingRepository _roleUserMappingRepository;
        private readonly IAuth_RoleRepository _roleRepository;
        private readonly AppSettings _appSettings;
        private readonly IAddressesService _addressService;
        private readonly IAddressesRepository _addressRepository;
        private readonly IAddressTypeService _addressTypeService;
        private readonly IDesignationRepository _designationRepository;
        private readonly ICompanyService _companyService;
        public UserService(ICompanyLocationUserMappingRepository companyLocationUserMappingRepository,
            ICompanyLocationUserMappingService companyLocationUserMappingService,
            IUserRepository userRepository,
            IUserAddressRepository userAddressRepository,
            IAuth_RoleUserMappingRepository roleUserMappingRepository,
            IAuth_RoleRepository roleRepository,
            IOptions<AppSettings> appSettings,
            IAddressesService addressService,
            IAddressesRepository addressRepository,
            IAddressTypeService addressTypeService,
            IDesignationRepository designationRepository,
            ICompanyService companyService)
        {
            _companyLocationUserMappingRepository = companyLocationUserMappingRepository;
            _companyLocationUserMappingService = companyLocationUserMappingService;
            _userRepository = userRepository;
            _userAddressRepository = userAddressRepository;
            _roleUserMappingRepository = roleUserMappingRepository;
            _roleRepository = roleRepository;
            _appSettings = appSettings.Value;
            _addressService = addressService;
            _addressRepository = addressRepository;
            _addressTypeService = addressTypeService;
            _designationRepository = designationRepository;
            _companyService = companyService;
        }

        public IEnumerable<ResultActiveUser> GetActiveUsers()
        {
            List<ResultActiveUser> users = new List<ResultActiveUser>();
            List<User> activeUsers = _userRepository.GetAll()
                                    .Where(x => x.IsActive == true)
                                    .OrderBy(x => x.CreatedOn)
                                    .ToList();
            foreach (var currentUser in activeUsers)
            {
                ResultActiveUser user = new ResultActiveUser();
                user.Id = currentUser.Id;
                user.UserName = currentUser.UserName;
                user.IsActive = currentUser.IsActive;
                users.Add(user);
            }
            return users;
        }

        public IEnumerable<ResultUser> GetUsers(Guid CompanyId)
        {
            List<ResultUser> users = new List<ResultUser>();
            IEnumerable<User> dbUsers = _userRepository.GetAll(CompanyId).OrderByDescending(x => x.CreatedOn);
            foreach (var currentUser in dbUsers)
            {
                ResultUser user = new ResultUser();
                user.Id = currentUser.Id;
                user.UserName = currentUser.UserName;
                user.IsActive = currentUser.IsActive;
                user.TotalUsers = dbUsers.Count();
                if (currentUser.UserProfile != null)
                    user.UserProfile = currentUser.UserProfile;

                if (currentUser.UserProfile != null)
                    user.Designation = _designationRepository.Get(currentUser.UserProfile.DesignationId);
                users.Add(user);
            }
            return users;
        }

        public User GetUserByEmail(string email, Guid CompanyId)
        {
            return _userRepository.GetAll(CompanyId).Where(x => x.UserName.Trim().ToLower() == email.Trim().ToLower()).FirstOrDefault();
        }

        public Dictionary<string, string> GetUserByEmailAndCompanyLocation(string email, Guid companyid, Guid companyLocationId)
        {
            var data = new Dictionary<string, string>();
            //If User Found In same CompnayLocation
            if (_companyLocationUserMappingService.GetUsersWithLocation(companyLocationId).Where(x => x.UserName.Trim().ToLower() == email.Trim().ToLower()).Count() > 0)
                data.Add("Flag", "1");
            else
            {
                //var model = _userRepository.GetAll().Where(x => x.UserProfile.CompanyId == companyid && x.UserName.Trim().ToLower() == email.Trim().ToLower()).FirstOrDefault();
                ////If User Found In Company but not in Same CompanyLocation
                //if (model != null)
                //{
                //    data.Add("UserId", model.Id.ToString());
                //    data.Add("CompanyId", model.UserProfile.CompanyId.ToString());
                //    data.Add("CompanyName", _companyService.GetCompany(companyid).Name);
                //    data.Add("Flag", "2");
                //}
                //else
                //    //If User not Found In Any Company 
                    data.Add("Flag", "0");
            }
            return data;
        }

        public IEnumerable<ResultUserByEmailandCompanyID> GetCompanyUsersExceptCurrentCompanyLocation(string email, Guid companyid, Guid companyLocationId)
        {

            return _userRepository.GetCompanyUsersExceptCurrentCompanyLocation(email, companyid, companyLocationId);
        }


        public ResultUser GetUser(Guid Id, Guid CompanyId, Guid? CompanyLocationId)
        {
            ResultUser user = new ResultUser();
            User currentUser = new User();
            if (CompanyLocationId == null)
                currentUser = _userRepository.GetById(Id, CompanyId, Guid.Empty);
            else
            {
                currentUser = _userRepository.GetById(Id, CompanyId, (Guid)CompanyLocationId);
                if (CompanyLocationId != Guid.Empty)
                {
                    user.isUserCurrentBaseLocation = _companyLocationUserMappingService.isCurrentUserBaseLocation((Guid)CompanyLocationId, Id);
                }
            }
            //User currentUser = _userRepository.GetById(Id,CompanyId , (Guid)CompanyLocationId);

            if (currentUser != null)
            {
                user.Id = currentUser.Id;
                user.UserName = currentUser.UserName;
                user.IsActive = currentUser.IsActive;
                user.isFirstLogin = currentUser.isFirstLogin != null ? currentUser.isFirstLogin : false;
                user.ForcePasswordReset = currentUser.ForcePasswordReset;

                if (currentUser.UserProfile != null)
                    user.UserProfile = currentUser.UserProfile;

                user.Designation = _designationRepository.Get(currentUser.UserProfile.DesignationId);

                if (CompanyLocationId == null)
                {
                    CompanyLocationId = _companyLocationUserMappingRepository.GetByIds(currentUser.Id).Where(x => x.isBaseLocation).FirstOrDefault().CompanyLocationId;
                }
                user.Auth_Roles = _roleRepository.GetAuth_RolesByUserId(currentUser.Id, (Guid)CompanyLocationId).ToList();


                if (currentUser.UserProfile.ReportingToUserId != null) {
                    var ReportingToUserId = currentUser.UserProfile.ReportingToUserId == null ? "" : currentUser.UserProfile.ReportingToUserId.ToString();

                    var userdata = _userRepository.GetExistingUserData(Guid.Parse(ReportingToUserId), (Guid)CompanyLocationId);
                    if (userdata != null)
                    {
                        var FirstName = userdata.userProfile.FirstName == null ? "" : userdata.userProfile.FirstName;
                        var LastName = userdata.userProfile.LastName == null ? "" : userdata.userProfile.LastName;
                        var Email = userdata.UserName == null ? "" :  "("+userdata.UserName+")";

                        user.UserProfile.ReportingToUserName = FirstName + " " + LastName + " " +  Email;
                    }
                }


                return user;
            }
            return null;
        }

        public ResultUser CreateUser(InputUser inputUser)
        {
            if (inputUser.IsExistingUser)
            {
                if (inputUser.roles != null)
                {
                    _roleUserMappingRepository.InsetAuth_RoleUserMapping(inputUser.roles, (Guid)inputUser.ExistingUserData.Id);
                }

                var companyLocationUserMapping = new InputCompanyLocationUserMapping();
                companyLocationUserMapping.CompanyLocationId = inputUser.CompanyLocationId;
                companyLocationUserMapping.UserId = (Guid)inputUser.ExistingUserData.Id;
                companyLocationUserMapping.isBaseLocation = false;
                companyLocationUserMapping.isFromNewLocation = true;
                _companyLocationUserMappingService.Insert(companyLocationUserMapping);

                return GetUser((Guid)inputUser.ExistingUserData.Id, inputUser.userProfile.CompanyId, inputUser.CompanyLocationId);
            }
            else
            {
                User user = new User();
                string randomPassword = CommonFunction.CreateRandomPassword(_appSettings.passwordSettings.PasswordLength, _appSettings.passwordSettings.RandomPasswordValidCharacters);

                string hashedPasswordWithSalt = CommonFunction.HashPassword(randomPassword, null, false);
                var passwordAndHash = hashedPasswordWithSalt.Split(':');
                if (passwordAndHash != null || passwordAndHash.Length > 1)
                {
                    user.IsActive = true;
                    user.UserName = inputUser.UserName;
                    user.PasswordHash = passwordAndHash[0];
                    user.PasswordSalt = passwordAndHash[1];
                    user.TempPasswordHash = passwordAndHash[0];
                    user.TempPasswordSalt = passwordAndHash[1];
                    user.TempPasswordExpiryDate = DateTime.Now;
                    user.ForcePasswordReset = true;
                    user.CreatedOn = DateTime.UtcNow;
                    user.isFirstLogin = true;
                    if (inputUser.userProfile != null)
                    {
                        user.UserProfile = new UserProfile();
                        user.UserProfile.FirstName = inputUser.userProfile.FirstName;
                        user.UserProfile.LastName = inputUser.userProfile.LastName;
                        user.UserProfile.DesignationId = inputUser.userProfile.DesignationId;
                        user.UserProfile.Alias = string.Empty;
                        user.UserProfile.Mobile = string.Empty;
                        user.UserProfile.CreatedOn = DateTime.UtcNow;
                        user.UserProfile.ReportingToUserId = inputUser.userProfile.ReportingToUserId == null ? Guid.Empty : inputUser.userProfile.ReportingToUserId.Value;
                        user.UserProfile.CompanyId = inputUser.userProfile.CompanyId;
                        user.UserProfile.Description = inputUser.userProfile.Description;
                    };
                }
                _userRepository.Add(user);
                _userRepository.Save();

                if (inputUser.roles != null)
                {
                    _roleUserMappingRepository.InsetAuth_RoleUserMapping(inputUser.roles, user.Id);
                }

                var companyLocationUserMapping = new InputCompanyLocationUserMapping();
                companyLocationUserMapping.CompanyLocationId = inputUser.CompanyLocationId;
                companyLocationUserMapping.UserId = user.Id;
                companyLocationUserMapping.isBaseLocation = true;
                _companyLocationUserMappingService.Insert(companyLocationUserMapping);


                //FUNCTIONALITY FOR SEND EMAIL TO USER FOR FIRST TIME TO RESET PASSWORD
                //string bodymsg = String.Format("Hello {0},{1}{2}Below you can find your temporary password.{3}___________________________________________________________________{4}{5}{6}{7}___________________________________________________________________{8}{9}If you have any question about how to change your password, visit our help center.{10}{11}Thanks & Regards,{12}Team Leemo{13}{14}Click the link below to login.{15}{16}{17}", user.UserProfile.FirstName, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, randomPassword, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, Environment.NewLine, _appSettings.sendingEmailSettings.WebUrl, Constants.WebConstants.Urls.WEB_AccountLogin);
                string bodymsg = String.Format("Hello {0},{1}{2}Below you can find your temporary password.{3}___________________________________________________________________{4}{5}{6}{7}___________________________________________________________________{8}{9}If you have any question about how to change your password, visit our help center.{10}{11}Thanks & Regards,{12}Team Leemo{13}{14}Click the link below to login.{15}{16}{17}", user.UserProfile.FirstName, "<br/>","<br/>","<br/>","<br/>","<br/>", randomPassword, "<br/>","<br/>","<br/>","<br/>","<br/>","<br/>","<br/>","<br/>","<br/>", _appSettings.sendingEmailSettings.WebUrl, Constants.WebConstants.Urls.WEB_AccountLogin);
                CommonFunction.sendEmail(_appSettings.sendingEmailSettings.From, user.UserName, bodymsg, Constants.EmailConstants.EmailSubjects.WelcomeEmail, _appSettings.sendingEmailSettings.Password, _appSettings.sendingEmailSettings.Host, _appSettings.sendingEmailSettings.Port, _appSettings.sendingEmailSettings.EnableSsl, _appSettings.sendingEmailSettings.IsBodyHtml, _appSettings.sendingEmailSettings.UseDefaultCredentials, _appSettings.sendingEmailSettings.alias);

                return GetUser(user.Id, inputUser.userProfile.CompanyId, inputUser.CompanyLocationId);
            }


        }

        public ResultUser EditUser(InputUserAndAddresses updateUser)
        {
            User currentUser = _userRepository.GetById(updateUser.InputUser.Id, updateUser.InputUser.userProfile.CompanyId, updateUser.CompanyLocationId);
            currentUser.Id = updateUser.InputUser.Id;
            currentUser.IsActive = updateUser.InputUser.IsActive;
            currentUser.UserName = currentUser.UserName;
            currentUser.PasswordSalt = currentUser.PasswordSalt;
            currentUser.PasswordHash = currentUser.PasswordHash;
            currentUser.TempPasswordHash = currentUser.TempPasswordHash;
            currentUser.TempPasswordSalt = currentUser.TempPasswordSalt;
            currentUser.TempPasswordExpiryDate = currentUser.TempPasswordExpiryDate;
            currentUser.CreatedOn = currentUser.CreatedOn;
            currentUser.ModifiedOn = DateTime.UtcNow;
            Addresses currentAddress = _addressRepository.GetAddressById(updateUser.InputAddress.Id);

            if (updateUser.InputUser.userProfile != null)
            {
                currentUser.UserProfile.FirstName = updateUser.InputUser.userProfile.FirstName;
                currentUser.UserProfile.LastName = updateUser.InputUser.userProfile.LastName;
                currentUser.UserProfile.DateOfBirth = updateUser.InputUser.userProfile.DateOfBirth;
                currentUser.UserProfile.DesignationId = updateUser.InputUser.userProfile.DesignationId;
                currentUser.UserProfile.Alias = updateUser.InputUser.userProfile.Alias == null ? "" : updateUser.InputUser.userProfile.Alias;
                currentUser.UserProfile.Phone = updateUser.InputUser.userProfile.Phone;
                currentUser.UserProfile.Mobile = updateUser.InputUser.userProfile.Mobile;
                currentUser.UserProfile.CountryCode = updateUser.InputUser.userProfile.CountryCode;
                currentUser.UserProfile.Fax = updateUser.InputUser.userProfile.Fax;
                currentUser.UserProfile.Website = updateUser.InputUser.userProfile.Website;
                currentUser.UserProfile.Language = updateUser.InputUser.userProfile.Language;
                currentUser.UserProfile.CountryLocale = updateUser.InputUser.userProfile.CountryLocale;
                currentUser.UserProfile.DateFormat = updateUser.InputUser.userProfile.DateFormat;
                currentUser.UserProfile.TimeFormat = updateUser.InputUser.userProfile.TimeFormat;
                currentUser.UserProfile.TimeZone = updateUser.InputUser.userProfile.TimeZone;
                currentUser.UserProfile.CompanyId = updateUser.InputUser.userProfile.CompanyId == Guid.Empty ? Guid.Parse("3FA85F64-5717-4562-B3FC-2C963F66AFA6") : updateUser.InputUser.userProfile.CompanyId; //Made static for now
                currentUser.UserProfile.ReportingToUserId = updateUser.InputUser.userProfile.ReportingToUserId == Guid.Empty ? null : updateUser.InputUser.userProfile.ReportingToUserId;
                currentUser.UserProfile.Description = updateUser.InputUser.userProfile.Description;
                currentUser.UserProfile.CreatedOn = currentUser.UserProfile.CreatedOn;
                currentUser.UserProfile.ModifiedOn = DateTime.UtcNow;
            }
            if (updateUser.InputAddress != null)
            {
                if (updateUser.InputAddress.Id == Guid.Empty)
                {
                    currentAddress = new Addresses();
                    currentAddress.ReferenceId = currentUser.Id;
                    currentAddress.AddressTypeId = _addressTypeService.GetAddressTypeIdWithName(Constants.AddressTypeNames.UserAddress);
                    currentAddress.Street = updateUser.InputAddress.Street;
                    currentAddress.City = updateUser.InputAddress.City;
                    currentAddress.State = updateUser.InputAddress.State;
                    currentAddress.ZipCode = updateUser.InputAddress.ZipCode;
                    currentAddress.Country = updateUser.InputAddress.Country;
                    currentAddress.CreatedOn = DateTime.UtcNow;
                    currentAddress.Id = updateUser.InputAddress.Id;
                    currentAddress.AddressLine1 = updateUser.InputAddress.AddressLine1;

                    _addressRepository.Add(currentAddress);
                    _addressRepository.Save();

                }
                else
                {
                    currentAddress.Id = updateUser.InputAddress.Id;
                    currentAddress.ReferenceId = updateUser.InputUser.Id;
                    currentAddress.AddressTypeId = updateUser.InputAddress.AddressTypeId;
                    currentAddress.Street = updateUser.InputAddress.Street;
                    currentAddress.City = updateUser.InputAddress.City;
                    currentAddress.State = updateUser.InputAddress.State;
                    currentAddress.ZipCode = updateUser.InputAddress.ZipCode;
                    currentAddress.Country = updateUser.InputAddress.Country;
                    currentAddress.CreatedOn = currentAddress.CreatedOn;
                    currentAddress.ModifiedOn = DateTime.UtcNow;
                    currentAddress.AddressLine1 = updateUser.InputAddress.AddressLine1;
                    _addressRepository.Edit(currentAddress);
                    _addressRepository.Save();
                }
            }

            _userRepository.Edit(currentUser);
            _userRepository.Save();

            //update Auth_Role of User
            if (updateUser.InputUser.profiles != null)
            {
                //if (_roleUserMappingRepository.DeleteAuth_RoleUsersMappingByUserId(currentUser.Id)) //For Deleting Multiple Roles_User mapping if needed
                if (_roleUserMappingRepository.DeleteAuth_RoleUsersMappingByLocationRolesUserId(updateUser.CompanyLocationId ,currentUser.Id))
                    _roleUserMappingRepository.InsetAuth_RoleUserMapping(updateUser.InputUser.profiles, currentUser.Id);
            }

            return GetUser(currentUser.Id, currentUser.UserProfile.CompanyId, updateUser.CompanyLocationId);
        }

        public ResultUser ChangePassword(InputChangePassword inputChangePassword, User currentUser)
        {
            ResultUser resultUser = new ResultUser();
            string hashedPasswordWithSalt = CommonFunction.HashPassword(inputChangePassword.NewPassword, null, false);
            var passwordAndHash = hashedPasswordWithSalt.Split(':');

            if (passwordAndHash != null || passwordAndHash.Length > 1)
            {
                currentUser.PasswordHash = passwordAndHash[0];
                currentUser.PasswordSalt = passwordAndHash[1];
                currentUser.ForcePasswordReset = false;
                currentUser.ModifiedOn = DateTime.UtcNow;
                currentUser.isFirstLogin = false;
                _userRepository.Edit(currentUser);
                _userRepository.Save();

                resultUser = GetUser(currentUser.Id, currentUser.UserProfile.CompanyId, inputChangePassword.CompanyLocationId);
            }
            return resultUser;
        }

        public string ForgetPassword(InputForgetPassword inputForgetPassword, User currentUser)
        {
            string randomPassword = CommonFunction.CreateRandomPassword(_appSettings.passwordSettings.PasswordLength, _appSettings.passwordSettings.RandomPasswordValidCharacters);
            string hashedPasswordWithSalt = CommonFunction.HashPassword(randomPassword, null, false);
            var temppasswordAndHash = hashedPasswordWithSalt.Split(':');
            ResultUser resultUser = new ResultUser();


            if (temppasswordAndHash != null || temppasswordAndHash.Length > 1)
            {
                currentUser.TempPasswordHash = temppasswordAndHash[0];
                currentUser.TempPasswordSalt = temppasswordAndHash[1];
                currentUser.TempPasswordExpiryDate = DateTime.UtcNow.AddMinutes(20);
                _userRepository.Edit(currentUser);
                _userRepository.Save();

                resultUser = GetUser(currentUser.Id, currentUser.UserProfile.CompanyId, inputForgetPassword.CompanyLocationId);
            }
            //return resultUser;
            return randomPassword;
        }


        public ResultUser ResetPassword(InputChangePassword inputChangePassword, User currentUser)
        {
            ResultUser resultUser = new ResultUser();
            string hashedPasswordWithSalt = CommonFunction.HashPassword(inputChangePassword.NewPassword, null, false);
            var passwordAndHash = hashedPasswordWithSalt.Split(':');

            if (passwordAndHash != null || passwordAndHash.Length > 1)
            {
                currentUser.PasswordHash = passwordAndHash[0];
                currentUser.PasswordSalt = passwordAndHash[1];
                currentUser.TempPasswordHash = null;
                currentUser.TempPasswordSalt = null;
                currentUser.TempPasswordExpiryDate = null;
                currentUser.ForcePasswordReset = false;
                currentUser.ModifiedOn = DateTime.UtcNow;
                currentUser.isFirstLogin = false;
                _userRepository.Edit(currentUser);
                _userRepository.Save();

                resultUser = GetUser(currentUser.Id, currentUser.UserProfile.CompanyId, inputChangePassword.CompanyLocationId);
            }
            return resultUser;
        }

        public string UpdateProfileImage(InputUpdateProfileImage updateProfileImage)
        {
            User currentUser = _userRepository.GetById(updateProfileImage.UserId, updateProfileImage.CompanyId, updateProfileImage.CompanyLocationId);
            if (currentUser != null)
            {
                currentUser.UserProfile.ImageName = updateProfileImage.ImageName;
                currentUser.UserProfile.ModifiedOn = DateTime.UtcNow;
                _userRepository.Edit(currentUser);
                _userRepository.Save();
                return updateProfileImage.ImageName;
            }
            return null;
        }

        public IEnumerable<User> GetUserByDesignationId(Guid? DesignationId, Guid CompanyId)

        {
            var UsersList = _userRepository.GetAll(CompanyId).Where(x => x.UserProfile != null && x.IsActive == true).ToList();
            var UserListWithAccociatedDesignation = UsersList.Where(x => x.UserProfile.DesignationId == DesignationId).ToList();
            return UserListWithAccociatedDesignation;
        }

        public IEnumerable<Auth_FeatureListWithGeneralCodeByUserIdResult> GetAuth_FeatureListWithGeneralCodeByUserId(Guid UserId, Guid CompanyLocationId)
        {
            using (SqlConnection conn = new SqlConnection(_appSettings.connectionStrings.Leemo_API_DbConnection))
            {
                DynamicParameters ds = new DynamicParameters();
                ds.Add("@UserId", UserId);
                ds.Add("@RoleId", null);
                ds.Add("@CompanyLocationId", CompanyLocationId);
                string query = "sp_GetAuth_FeatureListWithGeneralCodeByUserId";
                var result = conn.Query<Auth_FeatureListWithGeneralCodeByUserIdResult>(query, param: ds, commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public User GetUserByUserName(string username)
        {
            return _userRepository.GetByUserName(username);
        }

        public InputUser GetExistingUserData(Guid UserId, Guid CompanyLocationId)
        {
            return _userRepository.GetExistingUserData(UserId, CompanyLocationId);
        }


        public Dictionary<string, int> GetUserCounts(Guid companyLocationid)
        {
            return _userRepository.GetUserCounts(companyLocationid);
        }

    }
}
