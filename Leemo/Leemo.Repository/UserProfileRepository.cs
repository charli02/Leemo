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
    /// Represents userprofile repository for its CRUD and other custom functions.
    /// </summary>
    public class UserProfileRepository: RepositoryBase<UserProfile, LeemoDbContext>, IUserProfileRepository
    {
        //private LeemoDbContext _context;
        public UserProfileRepository(LeemoDbContext context): base(context)
        {
            //_context = context;
        }
    }
}
