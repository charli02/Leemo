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
    public class GeneralCodeGroupRepository : RepositoryBase<GeneralCodeGroup, LeemoDbContext>, IGeneralCodeGroupRepository
    {
        //private LeemoDbContext _context;

        public GeneralCodeGroupRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }
   }
}
