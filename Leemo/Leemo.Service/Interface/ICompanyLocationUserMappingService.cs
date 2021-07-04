using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Repository.Interface;

namespace Leemo.Service.Interface
{
    public interface ICompanyLocationUserMappingService
    {
        CompanyLocationUserMapping GetCompanyLocationUserMapping(Guid companyLocationId, Guid userId);
        CompanyLocationUserMapping Insert(InputCompanyLocationUserMapping companyLocationUserMapping);
        CompanyLocationUserMapping Update(InsertUpdateCompanyLocationUserMapping companyLocationUserMapping);
        IEnumerable<ResultUser> GetUsersWithLocation(Guid companyLocationId);
        IEnumerable<CompanyLocation> GetCompanyLocationWithUserId(Guid userId);
        IEnumerable<CompanyLocationUserMapping> GetByIds(Guid userId);
        bool isCurrentUserBaseLocation(Guid CompanyLocationId, Guid UserId);
    }
}
