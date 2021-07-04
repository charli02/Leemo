using System;
using System.Collections.Generic;
using System.Linq;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;

namespace Leemo.Service
{
    public class CompanyLocationUserMappingService: ICompanyLocationUserMappingService
    {
        private readonly ICompanyLocationUserMappingRepository _companyLocationUserMappingRepo;
        public CompanyLocationUserMappingService(ICompanyLocationUserMappingRepository companyLocationUserMappingRepo)
        {
            _companyLocationUserMappingRepo = companyLocationUserMappingRepo;
        }

        public CompanyLocationUserMapping GetCompanyLocationUserMapping(Guid companyLocationId, Guid userId)
        {
            return _companyLocationUserMappingRepo.GetByIds(companyLocationId, userId);
        }

        public CompanyLocationUserMapping Insert(InputCompanyLocationUserMapping inputcompanyLocationUserMapping)
        {
            CompanyLocationUserMapping dataToCreated = new CompanyLocationUserMapping();
            if (inputcompanyLocationUserMapping != null)
            {
                dataToCreated.CompanyLocationId = inputcompanyLocationUserMapping.CompanyLocationId;
                dataToCreated.UserId = inputcompanyLocationUserMapping.UserId;
                //dataToCreated.isBaseLocation = inputcompanyLocationUserMapping.isBaseLocation;
                if (inputcompanyLocationUserMapping.isFromNewLocation == true)
                {
                    dataToCreated.isBaseLocation = false;
                }
                else
                {
                    dataToCreated.isBaseLocation = true;
                }
            }
            _companyLocationUserMappingRepo.Add(dataToCreated);
            _companyLocationUserMappingRepo.Save();
            return dataToCreated;
        }

        public CompanyLocationUserMapping Update(InsertUpdateCompanyLocationUserMapping updatecompanyLocationUserMapping)
        {
            var currentData = _companyLocationUserMappingRepo.GetByIds(updatecompanyLocationUserMapping.OldCompanyLocationId,
                                                                       updatecompanyLocationUserMapping.OldUserId);
            if (currentData != null) 
            {
                _companyLocationUserMappingRepo.Delete(currentData);
                _companyLocationUserMappingRepo.Save();
                currentData.CompanyLocationId = updatecompanyLocationUserMapping.CompanyLocationId;
                currentData.UserId = updatecompanyLocationUserMapping.UserId;
                //currentData.isBaseLocation = updatecompanyLocationUserMapping.isBaseLocation;
                currentData.isBaseLocation = currentData.isBaseLocation;
            }
            _companyLocationUserMappingRepo.Add(currentData);
            _companyLocationUserMappingRepo.Save();
            return currentData;
        }

        public IEnumerable<ResultUser> GetUsersWithLocation(Guid companyLocationId)
        {
            return _companyLocationUserMappingRepo.GetUsersByLocation(companyLocationId);
        }
        public IEnumerable<CompanyLocation> GetCompanyLocationWithUserId(Guid userId)
        {
            return _companyLocationUserMappingRepo.GetCompanyLocationwithUserId(userId);
        }

        public IEnumerable<CompanyLocationUserMapping> GetByIds(Guid userId)
        {
            return _companyLocationUserMappingRepo.GetByIds(userId);
        }

        public bool isCurrentUserBaseLocation(Guid CompanyLocationId, Guid UserId)
        {
            bool isBaseLocation = GetCompanyLocationUserMapping(CompanyLocationId, UserId).isBaseLocation;
            return isBaseLocation;
        }
    }
}
