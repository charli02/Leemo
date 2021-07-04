//using System;
//using System.Collections.Generic;
//using Leemo.Model;
//using Leemo.Model.Domain;
//using Leemo.Model.ResultModels;
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
//    public class ProfileService : IProfileService
//    {
//        private readonly IProfileRepository _profileRepository;

//        public ProfileService(IProfileRepository profileRepository)
//        {
//            _profileRepository = profileRepository;
//        }

//        public IEnumerable<Profile> GetProfiles()
//        {
//            return _profileRepository.GetAll();
//        }

//        public Profile GetProfile(Guid Id)
//        {
//            return _profileRepository.Get(Id);
//        }

//        public void CreateProfile(Profile inputProfile)
//        {
//            Profile profile = new Profile();
//            profile.Name = inputProfile.Name;
//            profile.Description = inputProfile.Description;
//            profile.CreatedOn = DateTime.UtcNow;
//            profile.CreatedBy = inputProfile.CreatedBy;
//            profile.IsDeleted = false;
//            _profileRepository.Add(profile);
//            _profileRepository.Save();
//        }

//        public void EditProfile(Profile profileToUpdate, Profile currentProflie)
//        {
//            currentProflie.Name = profileToUpdate.Name;
//            currentProflie.Description = profileToUpdate.Description != null? profileToUpdate.Description : currentProflie.Description;
//            currentProflie.CreatedOn = currentProflie.CreatedOn;
//            currentProflie.CreatedBy = currentProflie.CreatedBy;
//            currentProflie.ModifiedBy = profileToUpdate.ModifiedBy;
//            currentProflie.ModifiedOn = DateTime.Now;

//            _profileRepository.Edit(currentProflie);
//            _profileRepository.Save();
//        }

//        public IEnumerable<Profile> GetProfilesByUserId(Guid userId)
//        {
//            return _profileRepository.GetProfilesByUserId(userId);
//        }

//        public IEnumerable<ResultProfileUser> GetProfileUsersByProfileId(Guid profileId)
//        {
//            return _profileRepository.GetProfileUsersByProfileId(profileId);
//        }

//        public void DeleteProfile(Profile profile)
//        {
//            profile.IsDeleted = true;
//            profile.ModifiedOn = DateTime.UtcNow;

//            _profileRepository.Edit(profile);
//            _profileRepository.Save();
//        }
//    }
//}
