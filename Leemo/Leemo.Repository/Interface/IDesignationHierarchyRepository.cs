using System.Collections.Generic;
using TPSS.Common.Interfaces;
using Leemo.Model;
using Leemo.Model.Domain;

/// <summary>
/// Represents repository project namespace
/// </summary>
namespace Leemo.Repository.Interface
{
    public interface IDesignationHierarchyRepository : IRepository<DesignationHierarchy>
    {
        IEnumerable<DesignationHierarchy> GetDesignationHierarchyList();

        void CreateDesignationHierarchies(DesignationHierarchy designationHierarchy);
    }
}
