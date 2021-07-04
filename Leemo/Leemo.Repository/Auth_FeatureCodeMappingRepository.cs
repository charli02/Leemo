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
    /// Represents address type repository for its CRUD and other custom functions.
    /// </summary>
    public class Auth_FeatureCodeMappingRepository : RepositoryBase<Auth_FeatureCodeMapping, LeemoDbContext>, IAuth_FeatureCodeMappingRepository
    {
        //private LeemoDbContext _context;

        public Auth_FeatureCodeMappingRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }
   }
}
