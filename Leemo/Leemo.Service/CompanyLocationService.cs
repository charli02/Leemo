using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using TPSS.Common;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Repository;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;

namespace Leemo.Service
{
    public class CompanyLocationService : ICompanyLocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IDesignationService _designationService;
        private readonly ICompanyService _companyService;
        private readonly IAuth_RoleService _roleService;
        private readonly ICompanyLocationUserMappingRepository _companyLocationUserMappingRepository;
        private readonly ICompanyLocationUserMappingService _companyLocationUserMappingService;
        private readonly AppSettings _appSettings;
        private readonly IAuth_RoleUserMappingRepository _auth_RoleUserMappingRepository;

        public CompanyLocationService(ILocationRepository locationRepository, 
            IDesignationService designationService, ICompanyService companyService, 
            IAuth_RoleService roleService, IOptions<AppSettings> appSettings,
            ICompanyLocationUserMappingRepository companyLocationUserMappingRepository,
            ICompanyLocationUserMappingService companyLocationUserMappingService,
            IAuth_RoleUserMappingRepository auth_RoleUserMappingRepository)
        {
            _locationRepository = locationRepository;
            _designationService = designationService;
            _companyService = companyService;
            _roleService = roleService;
            _companyLocationUserMappingRepository = companyLocationUserMappingRepository;
            _appSettings = appSettings.Value;
            _companyLocationUserMappingService = companyLocationUserMappingService;
            _auth_RoleUserMappingRepository = auth_RoleUserMappingRepository;
        }

        public IEnumerable<ResultLocation> GetCompanyLocation()

        {
            List<ResultLocation> model = new List<ResultLocation>();
                IEnumerable<CompanyLocation> companyLocationList = _locationRepository.GetAll().ToList();
                foreach (var item in companyLocationList)
                {
                    ResultLocation modelResult = new ResultLocation();
                    modelResult.Id = item.Id;
                    modelResult.LocationName = item.LocationName;
                    modelResult.IsActive = item.IsActive;
                    modelResult.IsHeadOffice = item.IsHeadOffice;
                    modelResult.ModifiedBy = item.ModifiedBy;
                    modelResult.ModifiedOn = item.ModifiedOn;
                    modelResult.CreatedOn = item.CreatedOn;
                    modelResult.CreatedBy = item.CreatedBy;
                    //modelResult.AddressId = item.AddressId;
                    modelResult.CompanyId = item.CompanyId;
                    model.Add(modelResult);
                }
            return model;
        }

        public ResultLocation GetCompanyLocationById(Guid Id)
        {
           
            CompanyLocation currentCompanyLocation = _locationRepository.GetCompanyLocationById(Id);
           
                ResultLocation modelResult = new ResultLocation();
                modelResult.Id = currentCompanyLocation.Id;
                modelResult.LocationName = currentCompanyLocation.LocationName;
                modelResult.IsActive = currentCompanyLocation.IsActive;
                modelResult.IsHeadOffice = currentCompanyLocation.IsHeadOffice;
                modelResult.ModifiedBy = currentCompanyLocation.ModifiedBy;
                modelResult.ModifiedOn = currentCompanyLocation.ModifiedOn;
                modelResult.CreatedOn = currentCompanyLocation.CreatedOn;
                modelResult.CreatedBy = currentCompanyLocation.CreatedBy;
                //modelResult.AddressId = currentCompanyLocation.AddressId;
                modelResult.CompanyId = currentCompanyLocation.CompanyId;

            return modelResult;
        }
       
        public CompanyLocation CreateCompanyLocation(InputLocation inputLocation)
        {
            CompanyLocation locationTocreate = new CompanyLocation();
            var existingLocation = _locationRepository.GetAll().Where(x => x.CompanyId == inputLocation.CompanyId).ToList();
            if (existingLocation.Count() == 0)
            {
                locationTocreate.IsHeadOffice = true;
            }
            
            locationTocreate.LocationName = inputLocation.LocationName;
            locationTocreate.CreatedBy = inputLocation.CreatedBy;
            locationTocreate.CreatedOn = DateTime.Now;
            locationTocreate.IsActive = true;
            locationTocreate.CompanyId = inputLocation.CompanyId;
            //locationTocreate.AddressId = inputLocation.AddressId;

            _locationRepository.Add(locationTocreate);
            _locationRepository.Save();

            Designation rootDesignation = new Designation();
            rootDesignation.Name = _companyService.GetCompany(locationTocreate.CompanyId).Name;
            rootDesignation.CompanyLocationId = locationTocreate.Id;
            rootDesignation.IsRoot = true;
            _designationService.CreateDesignation(rootDesignation);

            string owner = Constants.WebConstants.Owner;
            Auth_Role ownerRole = new Auth_Role();
            ownerRole.Name = owner.First().ToString().ToUpper() + owner.Substring(1);
            ownerRole.Description = ownerRole.Name;
            ownerRole.CreatedBy = (Guid)locationTocreate.CreatedBy;
            ownerRole.CompanyLocationId = locationTocreate.Id;
            var OwnerRole = _roleService.CreateAuth_Role(ownerRole);

            IList<Guid> userRoles = new List<Guid> { OwnerRole.Id };
            _auth_RoleUserMappingRepository.InsetAuth_RoleUserMapping(userRoles, (Guid)locationTocreate.CreatedBy);

            using (SqlConnection conn = new SqlConnection(_appSettings.connectionStrings.Leemo_API_DbConnection))
            {
                DynamicParameters ds = new DynamicParameters();
                ds.Add("@CreatedBy", OwnerRole.CreatedBy);
                ds.Add("@RoleId", OwnerRole.Id);
                string query = "sp_InsertNewLocationAuth_RoleFeatureMapping";
                var result = conn.Query(query, param: ds, commandType: CommandType.StoredProcedure).ToList();
            }

            InputCompanyLocationUserMapping insertUserLocationMapping = new InputCompanyLocationUserMapping();
            insertUserLocationMapping.CompanyLocationId = locationTocreate.Id;
            insertUserLocationMapping.UserId = (Guid)locationTocreate.CreatedBy;
            insertUserLocationMapping.isFromNewLocation = true;
            _companyLocationUserMappingService.Insert(insertUserLocationMapping);

            return locationTocreate;
        }

        public void EditCompanyLocation(InputLocation updateCompanyLocation)
        {
            var locationToupdate = _locationRepository.GetCompanyLocationById(updateCompanyLocation.Id);
            if (updateCompanyLocation != null)
            {
                locationToupdate.LocationName = updateCompanyLocation.LocationName;
                locationToupdate.IsHeadOffice = locationToupdate.IsHeadOffice;
                locationToupdate.CreatedBy = locationToupdate.CreatedBy;
                locationToupdate.CreatedOn = locationToupdate.CreatedOn;
                locationToupdate.ModifiedOn = DateTime.Now;
                locationToupdate.ModifiedBy = updateCompanyLocation.ModifiedBy;                
                locationToupdate.CompanyId = updateCompanyLocation.CompanyId;
                //locationToupdate.AddressId = updateCompanyLocation.AddressId;
                if (locationToupdate.IsHeadOffice)
                {
                    locationToupdate.IsActive = true;
                }
                else
                {
                    locationToupdate.IsActive = updateCompanyLocation.IsActive;
                }
            }
            _locationRepository.Edit(locationToupdate);
            _locationRepository.Save();
        }

        public IEnumerable<ResultLocation> GetLocationsByUserId(Guid UserId)
        {

            var UserLocationData = _companyLocationUserMappingRepository.GetByIds(UserId);
            List<ResultLocation> model = new List<ResultLocation>();
            foreach (var dataitem in UserLocationData) {
                IEnumerable<CompanyLocation> companyLocationList = _locationRepository.GetAll().Where(x => x.Id == dataitem.CompanyLocationId).ToList();
                //_locationRepository.GetLocationsByCompanyId(CompanyId).ToList();
                foreach (var item in companyLocationList)
                {
                    ResultLocation modelResult = new ResultLocation();
                    modelResult.Id = item.Id;
                    modelResult.LocationName = item.LocationName;
                    modelResult.IsHeadOffice = item.IsHeadOffice;
                    modelResult.IsActive = item.IsActive;
                    modelResult.IsBaseLocation= dataitem.isBaseLocation;
                    model.Add(modelResult);
                }
            }
            return model;
        }

        public IEnumerable<ResultLocation> GetLocationsByCompanyId(Guid CompanyId)
        {
            List<ResultLocation> model = new List<ResultLocation>();          
                IEnumerable<CompanyLocation> companyLocationList = _locationRepository.GetLocationsByCompanyId(CompanyId).ToList();
                //_locationRepository.GetLocationsByCompanyId(CompanyId).ToList();
                foreach (var item in companyLocationList)
                {
                    ResultLocation modelResult = new ResultLocation();
                modelResult.Id = item.Id;
                modelResult.LocationName = item.LocationName;
                modelResult.IsActive = item.IsActive;
                modelResult.IsHeadOffice = item.IsHeadOffice;
                modelResult.ModifiedBy = item.ModifiedBy;
                modelResult.ModifiedOn = item.ModifiedOn;
                modelResult.CreatedOn = item.CreatedOn;
                modelResult.CreatedBy = item.CreatedBy;
                //modelResult.AddressId = item.AddressId;
                modelResult.CompanyId = item.CompanyId;
                model.Add(modelResult);
                }
            
            return model;
        }

        public Dictionary<string, int> GetLocationCounts(Guid companyId)
        {
            var model = new Dictionary<string, int>();
            var data = _locationRepository.GetLocationsByCompanyId(companyId);
            model.Add("All", data.Count());
            model.Add("Active", data.Where(x => x.IsActive == true).Count());
            return model;
        }

        public IEnumerable<ResultLocation> GetActiveorInActiveLocation(Guid companyId,bool filter)
        {
            List<ResultLocation> model = new List<ResultLocation>();
            var companyLocationList = _locationRepository.GetActiveorInActiveLocation(companyId,filter);
            foreach (var item in companyLocationList)       
            {
                ResultLocation modelResult = new ResultLocation();
                modelResult.Id = item.Id;
                modelResult.LocationName = item.LocationName;
                modelResult.IsActive = item.IsActive;
                modelResult.IsHeadOffice = item.IsHeadOffice;
                modelResult.ModifiedBy = item.ModifiedBy;
                modelResult.ModifiedOn = item.ModifiedOn;
                modelResult.CreatedOn = item.CreatedOn;
                modelResult.CreatedBy = item.CreatedBy;
                //modelResult.AddressId = item.AddressId;
                modelResult.CompanyId = item.CompanyId;
                model.Add(modelResult);
            }
            return model;
        }

        public CompanyLocation UpdateHeadOffice(Guid id, bool isHeadOffice,Guid CompanyId)
        {
            //Update Previous HeadOffice
            var previousHeadOffice = _locationRepository.GetLocationsByCompanyId(CompanyId).Where(x => x.IsHeadOffice == true).FirstOrDefault();
            if (previousHeadOffice != null)
            {
                previousHeadOffice.IsHeadOffice = false;
            }
            _locationRepository.Edit(previousHeadOffice);
            _locationRepository.Save();

            //Update Current HeadOffice
            var locationToupdate = _locationRepository.GetCompanyLocationById(id);
            if (locationToupdate != null)
            {
                locationToupdate.IsHeadOffice = isHeadOffice;
            }
            _locationRepository.Edit(locationToupdate);
            _locationRepository.Save();
            return locationToupdate;
        }

        public CompanyLocation GetLocationByName(string locationName)
        {
            return _locationRepository.GetAll().Where(x => x.LocationName.Trim().ToLower() == locationName.Trim().ToLower()).FirstOrDefault();
        }
    }
}
