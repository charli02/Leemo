using System;
using System.Collections.Generic;
using System.Text;
using TPSS.Common.Interfaces;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;

namespace Leemo.Repository.Interface
{
    public interface ILocationRepository : IRepository<CompanyLocation>
    {
        IEnumerable<CompanyLocation> GetAll();
        CompanyLocation GetCompanyLocationById(Guid Id);
        IEnumerable<CompanyLocation> GetLocationsByCompanyId(Guid CompanyId);
        IEnumerable<CompanyLocation> GetActiveorInActiveLocation(Guid companyId, bool filter);
    }
}
