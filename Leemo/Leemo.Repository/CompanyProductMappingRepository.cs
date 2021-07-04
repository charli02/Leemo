using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPSS.Common.Implementations;
using Leemo.Data;
using Leemo.Model.Domain;
using Leemo.Repository.Interface;

namespace Leemo.Repository
{
   public  class CompanyProductMappingRepository : RepositoryBase<CompanyProductMapping, LeemoDbContext>, ICompanyProductMappingRepository
    {

        //private XLMSDbContext _context;
        public CompanyProductMappingRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }

        public CompanyProductMapping GetProductLeadCheckAvailableDomain(string domainName)
        {

            return Context.CompanyProductMapping.Where(x => x.DomainName.Trim().ToLower() == domainName.Trim().ToLower()).FirstOrDefault();

        }
    }
}
