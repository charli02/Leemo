//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Leemo.Model;
//using Leemo.Model.Domain;
//using Leemo.Repository.Interface;
//using Leemo.Service.Interface;

///// <summary>
///// Represents Leemo service project namespace
///// </summary>
//namespace Leemo.Service
//{
//    /// <summary>
//    /// Represnets profile serivce class which interact with repository.
//    /// </summary>
//    public class ProfileUserMappingService : IProfileUserMappingService
//    {
//        private readonly IProfileUserMappingRepository _profileUserMappingRepository;

//        public ProfileUserMappingService(IProfileUserMappingRepository profileUserMappingRepository)
//        {
//            _profileUserMappingRepository = profileUserMappingRepository;
//        }

//        public IEnumerable<ProfileUserMapping> GetProfileUserMappingList()
//        {
//            return _profileUserMappingRepository.GetAll();
//        }

//        public ProfileUserMapping GetProfileUserMapping(Guid Id)
//        {
//            return _profileUserMappingRepository.Get(Id);
//        }

//        public void CreateProfileUserMapping(ProfileUserMapping profileUserMapping)
//        {
//            _profileUserMappingRepository.Add(profileUserMapping);
//            _profileUserMappingRepository.Save();
//        }
//    }
//}
