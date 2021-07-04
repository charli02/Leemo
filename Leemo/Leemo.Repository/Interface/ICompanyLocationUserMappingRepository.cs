using System;
using System.Collections.Generic;
using System.Text;
using TPSS.Common.Interfaces;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;

namespace Leemo.Repository.Interface
{
    public interface ICompanyLocationUserMappingRepository: IRepository<CompanyLocationUserMapping>
    {
        IEnumerable<CompanyLocationUserMapping> GetAll();
        CompanyLocationUserMapping GetByIds(Guid companyLocationId, Guid userId);
        IEnumerable<ResultUser> GetUsersByLocation(Guid companyLocationId);
        IEnumerable<CompanyLocation> GetCompanyLocationwithUserId(Guid userId);
        IEnumerable<CompanyLocationUserMapping> GetByIds( Guid userId);
    }
}
