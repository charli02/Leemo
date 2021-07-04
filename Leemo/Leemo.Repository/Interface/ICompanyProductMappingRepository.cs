using System;
using System.Collections.Generic;
using System.Text;
using TPSS.Common.Implementations;
using TPSS.Common.Interfaces;
using Leemo.Data;
using Leemo.Model.Domain;

namespace Leemo.Repository.Interface
{
    public interface ICompanyProductMappingRepository : IRepository<CompanyProductMapping>
    {
         CompanyProductMapping GetProductLeadCheckAvailableDomain(string domain);

    }
}
