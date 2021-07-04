using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;

namespace Leemo.Service.Interface
{
    public interface ICompanyLocationService
    {
        public IEnumerable<ResultLocation> GetCompanyLocation();
        ResultLocation GetCompanyLocationById(Guid Id);
        CompanyLocation CreateCompanyLocation(InputLocation inputLocation);
        void EditCompanyLocation(InputLocation updateCompanyLocation);
        public IEnumerable<ResultLocation> GetLocationsByUserId(Guid UserId);
        public IEnumerable<ResultLocation> GetLocationsByCompanyId(Guid CompanyId);
        IEnumerable<ResultLocation> GetActiveorInActiveLocation(Guid companyId,bool filter);
        Dictionary<string, int> GetLocationCounts(Guid companyId);
        CompanyLocation UpdateHeadOffice(Guid id, bool isHeadOffice, Guid CompanyId);
        CompanyLocation GetLocationByName(string locationName);
    }
}
