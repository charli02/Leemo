using System;
using System.Collections.Generic;
using TPSS.Common.Interfaces;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;

/// <summary>
/// Represents repository project namespace
/// </summary>
namespace Leemo.Repository.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetAll(Guid CompanyId);
        User GetById(Guid Id, Guid CompanyId , Guid CompanyLocationId);
        User GetByUserName(string UserName);
        IEnumerable<ResultUserByEmailandCompanyID> GetCompanyUsersExceptCurrentCompanyLocation(string email, Guid companyid, Guid companyLocationId);
        InputUser GetExistingUserData(Guid UserId, Guid CompanyLocationId);
        Dictionary<string, int> GetUserCounts(Guid companyLocationid);
    }
}
