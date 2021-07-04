using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;

/// <summary>
/// Represents Leemo service project namespace
/// </summary>
namespace Leemo.Service
{
    /// <summary>
    /// Represnets group role serivce class which interact with repository.
    /// </summary>
    public class GroupRoleService : IGroupRoleService
    {
        private readonly IGroupRoleRepository _groupRoleRepository;

        public GroupRoleService(IGroupRoleRepository groupRoleRepository)
        {
            _groupRoleRepository = groupRoleRepository;
        }

        public void CreateGroupRole(GroupDesignationMapping groupRole)
        {
            _groupRoleRepository.Add(groupRole);
            _groupRoleRepository.Save();
        }
    }
}
