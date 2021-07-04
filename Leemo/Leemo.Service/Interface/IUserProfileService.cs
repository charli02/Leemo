using System;
using System.Collections.Generic;
using Leemo.Model;
using Leemo.Model.Domain;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface IUserProfileService
    {
        public IEnumerable<UserProfile> GetUserProfiles();
        public UserProfile GetUserProfile(Guid Id);
        void CreateUserProfile(UserProfile userProfile);
        void EditUserProfile(UserProfile userProfile);
    }
}
