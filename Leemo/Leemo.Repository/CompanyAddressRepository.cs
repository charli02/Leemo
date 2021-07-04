using Leemo.Data;
using TPSS.Common.Implementations;
using Leemo.Model;
using Leemo.Repository.Interface;
using Leemo.Model.Domain;

/// <summary>
/// Represents Leemo repository project namespace
/// </summary>
namespace Leemo.Repository.Repository
{
    /// <summary>
    /// Represents companyaddress repository for its CRUD and other custom functions.
    /// </summary>
    //public class CompanyAddressRepository : RepositoryBase<CompanyAddress, LeemoDbContext>, ICompanyAddressRepository
    public class CompanyAddressRepository : RepositoryBase<Addresses, LeemoDbContext>, ICompanyAddressRepository
    {
        //private LeemoDbContext _context;

        public CompanyAddressRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }
   }
}
