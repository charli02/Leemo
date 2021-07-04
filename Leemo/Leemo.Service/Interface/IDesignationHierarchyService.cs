using System;
using System.Collections.Generic;
using Leemo.Model;
using Leemo.Model.Domain;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface IDesignationHierarchyService
    {
        IEnumerable<DesignationHierarchy> GetDesignationHierarchyList(Guid CompanyLocationId);

        void SetPosition(DesignationHierarchy designationHierarchy);

        Guid? GetParentDesignationId(Guid DesignationId);

        bool ResetDesignationHierarchies(IList<DesignationHierarchy> designationHierarchies);

        
    }
}
