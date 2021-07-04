using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Represnets userprofile serivce class which interact with repository.
    /// </summary>
    public class UserProfileService: IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public IEnumerable<UserProfile> GetUserProfiles()
        {
            return _userProfileRepository.GetAll();
        }

        public UserProfile GetUserProfile(Guid Id)
        {
            return _userProfileRepository.Get(Id);
        }

        public void CreateUserProfile(UserProfile userProfile)
        {
            _userProfileRepository.Add(userProfile);
            _userProfileRepository.Save();
        }

        public void EditUserProfile(UserProfile userProfile)
        {
            _userProfileRepository.Edit(userProfile);
            _userProfileRepository.Save();
        }
    }
}
