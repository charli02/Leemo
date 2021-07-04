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
    public class GeneralCodeRepository : RepositoryBase<GeneralCode, LeemoDbContext>, IGeneralCodeRepository
    {
        //private LeemoDbContext _context;

        public GeneralCodeRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }
   }
}
