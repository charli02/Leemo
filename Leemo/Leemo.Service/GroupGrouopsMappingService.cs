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
    /// Represnets group groups mapaping serivce class which interact with repository.
    /// </summary>
    public class GroupGrouopsMappingService : IGroupGrouopsMappingService
    {
        private readonly IGroupGroupsMappinngRepository _groupGroupsMappinngRepository;

        public GroupGrouopsMappingService(IGroupGroupsMappinngRepository groupGroupsMappinngRepository)
        {
            _groupGroupsMappinngRepository = groupGroupsMappinngRepository;
        }

        public void CreateGroupGroupsMapping(GroupGroupsMapping groupGroupsMapping)
        {
            _groupGroupsMappinngRepository.Add(groupGroupsMapping);
            _groupGroupsMappinngRepository.Save();
        }
    }
}
