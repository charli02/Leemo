using Leemo.Model;
using Leemo.Model.Domain;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface IGroupRoleService
    {
        void CreateGroupRole(GroupDesignationMapping groupRole);
    }
}
