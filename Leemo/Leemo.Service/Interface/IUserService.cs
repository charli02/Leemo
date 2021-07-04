using System;
using System.Collections.Generic;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Model.WrapperModels;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface IUserService
    {
        public IEnumerable<ResultActiveUser> GetActiveUsers();
        public IEnumerable<ResultUser> GetUsers(Guid CompanyId);
        public User GetUserByEmail(string email, Guid CompanyId);
        User GetUserByUserName(string username);
        public ResultUser GetUser(Guid Id, Guid CompanyId, Guid? CompanyLocationId);
        ResultUser CreateUser(InputUser inputUser);
        ResultUser EditUser(InputUserAndAddresses updateUser);
        ResultUser ChangePassword(InputChangePassword inputChangePassword, User currentUser);
        string ForgetPassword(InputForgetPassword inputForgetPassword, User currentUser);
        ResultUser ResetPassword(InputChangePassword inputChangePassword, User currentUser);
        string UpdateProfileImage(InputUpdateProfileImage updateProfileImage);
        IEnumerable<User> GetUserByDesignationId(Guid? DesignationId, Guid CompanyId);

        IEnumerable<Auth_FeatureListWithGeneralCodeByUserIdResult> GetAuth_FeatureListWithGeneralCodeByUserId(Guid UserId, Guid CompanyLocationId);
        Dictionary<string, string> GetUserByEmailAndCompanyLocation(string email, Guid companyid, Guid companyLocationId);
        IEnumerable<ResultUserByEmailandCompanyID> GetCompanyUsersExceptCurrentCompanyLocation(string email, Guid companyid, Guid companyLocationId);
        InputUser GetExistingUserData(Guid UserId, Guid CompanyLocationId);
        Dictionary<string, int> GetUserCounts(Guid companyLocationid);
    }
}
