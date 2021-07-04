using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TPSS.Common;
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
    /// Represnets comapny serivce class which interact with repository.
    /// </summary>
    public class DesignationService : IDesignationService
    {
        private readonly IDesignationRepository _designationRepository;
        private readonly IDesignationHierarchyRepository _designationHierarchyRepository;
        private readonly AppSettings _appSettings;

        public DesignationService(IDesignationRepository designationRepository, IDesignationHierarchyRepository designationHierarchyRepository, IOptions<AppSettings> appSettings)
        {
            _designationRepository = designationRepository;
            _designationHierarchyRepository = designationHierarchyRepository;
            _appSettings = appSettings.Value;
        }

        public IEnumerable<Designation> GetDesignations()
        {
            return _designationRepository.GetAll();
        }

        public Designation GetDesignation(Guid Id)
        {
            return _designationRepository.Get(Id);
        }

        public void CreateDesignation(Designation designation)
        {
            // Insert Designation
            designation.IsActive = true;
            designation.CreatedOn = DateTime.UtcNow;
            _designationRepository.Add(designation);
            _designationRepository.Save();

            // Insert Designation Hierarchy 
            if (designation.ParentDesignationId != null && designation.ParentDesignationId != Guid.Empty)
            {
                DesignationHierarchy DesignationHierarchies = new DesignationHierarchy();
                DesignationHierarchies.ParentDesignationId = designation.ParentDesignationId;
                DesignationHierarchies.DesignationId = Guid.Parse(designation.Id.ToString());

                _designationHierarchyRepository.Add(DesignationHierarchies);
                _designationHierarchyRepository.Save();
            }

        }

        public Designation EditDesignation(Designation role)
        {
            Designation designationRole = _designationRepository.Get(Guid.Parse(role.Id.ToString()));
            designationRole.CreatedOn = designationRole.CreatedOn;
            designationRole.Name = role.Name;
            designationRole.Description = role.Description;
            designationRole.IsActive = designationRole.IsActive;
            designationRole.ModifiedOn= DateTime.UtcNow;
            _designationRepository.Edit(designationRole);
            _designationRepository.Save();

            return designationRole;
            //Designation currentgroup = _roleRepository.(updateGroupImage.GroupId);
            //if (currentgroup != null)
            //{
            //    currentgroup.ImageName = updateGroupImage.ImageName;
            //    currentgroup.ModifiedOn = DateTime.UtcNow;
            //    _groupRepository.Edit(currentgroup);
            //    _groupRepository.Save();
            //    return currentgroup;
            //}
            //return currentgroup;
        }

        public void DeleteDesignation(Designation designation)
        {
            if (designation != null)
            {
                designation.IsActive = false;
                designation.ModifiedOn = DateTime.UtcNow;

                _designationRepository.Edit(designation);
                _designationRepository.Save();
            }
        }

        public void DeleteDesignation(Guid DesignationId, out int retVal, out string errorMsg)
        {
            using (SqlConnection conn = new SqlConnection(_appSettings.connectionStrings.Leemo_API_DbConnection))
            {
                var reader = conn.QueryMultiple("[dbo].[sp_DeleteDesignation]", param: new { DesignationId = DesignationId }, commandType: CommandType.StoredProcedure);
                var ReturnValue = reader.Read<int>().FirstOrDefault();
                var ErrorMessage = reader.Read<string>().FirstOrDefault();

                retVal = ReturnValue; errorMsg = ErrorMessage.TrimEnd(',') + '.';
            }
        }

        public Designation GetDesignationByName(string name,Guid companyLocationId)
        {
           
            return _designationRepository.GetAll().Where(x => x.Name.Trim().ToLower() == name.Trim().ToLower() && x.CompanyLocationId == companyLocationId).FirstOrDefault();
        }
    }
}
