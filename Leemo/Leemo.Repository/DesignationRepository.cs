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
    /// Represents designation repository for its CRUD and other custom functions.
    /// </summary>
    public class DesignationRepository : RepositoryBase<Designation, LeemoDbContext>, IDesignationRepository
    {
        //private LeemoDbContext _context;

        public DesignationRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }
    }
}
