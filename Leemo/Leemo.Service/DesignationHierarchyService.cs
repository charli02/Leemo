using System;
using System.Collections.Generic;
using System.Linq;
using TPSS.Common;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;

/// <summary>
/// Represents Leemo service project namespace
/// </summary>
namespace Leemo.Service
{
    /// <summary>
    /// Represnets comapny serivce class which interact with repository.
    /// </summary>
    public class DesignationHierarchyService : IDesignationHierarchyService
    {
        private readonly IDesignationHierarchyRepository _designationHierarchyRepository;
        private readonly IDesignationService _designationService;

        public DesignationHierarchyService(IDesignationHierarchyRepository designationHierarchyRepository, IDesignationService designationService)
        {
            _designationHierarchyRepository = designationHierarchyRepository;
            _designationService = designationService;
        }

        public IEnumerable<DesignationHierarchy> GetDesignationHierarchyList(Guid CompanyLocationId)
        {
            var designationHierarchyList = _designationHierarchyRepository.GetDesignationHierarchyList().Where(x=>x.Designation.IsActive==true && x.Designation.CompanyLocationId==CompanyLocationId).ToList();
            DesignationHierarchy parentnode = new DesignationHierarchy();

            
            parentnode.DesignationId = _designationService.GetDesignations().Where(x=>x.CompanyLocationId == CompanyLocationId && x.IsRoot ).FirstOrDefault().Id;
            parentnode.Designation = _designationService.GetDesignation(parentnode.DesignationId);

            designationHierarchyList.Add(parentnode);
            return designationHierarchyList;
        }

        public void SetPosition(DesignationHierarchy designationHierarchy)
        {
            DesignationHierarchy designationHierarchytoUpdate = _designationHierarchyRepository.GetAll().Where(
                x => x.DesignationId == designationHierarchy.DesignationId).FirstOrDefault();

            if (designationHierarchytoUpdate != null)
            {
                
                _designationHierarchyRepository.Delete(designationHierarchytoUpdate);
                _designationHierarchyRepository.Save();
                designationHierarchytoUpdate.ParentDesignationId = designationHierarchy.ParentDesignationId;
                //_designationHierarchyRepository.Edit(designationHierarchytoUpdate);
                _designationHierarchyRepository.Add(designationHierarchytoUpdate);
            }
            else
                _designationHierarchyRepository.Add(designationHierarchy);

            _designationHierarchyRepository.Save();
        }
        public Guid? GetParentDesignationId(Guid RoleId)
        {
            var list = _designationHierarchyRepository.GetDesignationHierarchyList().Where(x => x.DesignationId == RoleId).ToList();
            if (list.Count > 0) {
                return list.FirstOrDefault().ParentDesignationId == null ? Guid.Empty : list.FirstOrDefault().ParentDesignationId;
         
             //   return list.FirstOrDefault().ParentRoleId;
                
            }
            else
            return Guid.Empty;
        }

        public bool ResetDesignationHierarchies(IList<DesignationHierarchy> designationHierarchies)
        {
            var oldDesignationHierarchies = _designationHierarchyRepository.GetAll().ToList();
            _designationHierarchyRepository.DeleteRange(oldDesignationHierarchies);
            _designationHierarchyRepository.AddRange(designationHierarchies);
            _designationHierarchyRepository.Save();
            return true;
        }

       

    }
}
