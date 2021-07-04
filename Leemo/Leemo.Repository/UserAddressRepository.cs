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
    /// Represents useraddress repository for its CRUD and other custom functions.
    /// </summary>
    public class UserAddressRepository: RepositoryBase<UserAddress, LeemoDbContext>, IUserAddressRepository
    {
        //private LeemoDbContext _context;
        public UserAddressRepository(LeemoDbContext context): base(context)
        {
            //_context = context;
        }
    }
}
