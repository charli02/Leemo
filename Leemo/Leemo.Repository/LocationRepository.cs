using Microsoft.EntityFrameworkCore;
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
    public class LocationRepository : RepositoryBase<CompanyLocation, LeemoDbContext>, ILocationRepository
    {
        public LocationRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }

        public IEnumerable<CompanyLocation> GetAll()
        {
            //return Context.companyLocation.Include(atype => atype.AddressType).ToList();
            return Context.CompanyLocation.ToList();
        }

        public CompanyLocation GetCompanyLocationById(Guid Id)
        {
            return Context.CompanyLocation.Where(x => x.Id == Id).FirstOrDefault();
        }

        public IEnumerable<CompanyLocation> GetLocationsByCompanyId(Guid CompanyId)
        {
            return Context.CompanyLocation.Where(x => x.CompanyId == CompanyId);
        }
        public IEnumerable<CompanyLocation> GetActiveorInActiveLocation(Guid companyId, bool filter)
        {
            return Context.CompanyLocation.Where(x => x.CompanyId == companyId && x.IsActive == filter).ToList();
        }
    }
}
