//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Leemo.Data;
//using TPSS.Common.Implementations;
//using Leemo.Model;
//using Leemo.Model.ResultModels;
//using Leemo.Repository.Interface;
//using Leemo.Model.Domain;

///// <summary>
///// Represents Leemo repository project namespace
///// </summary>
//namespace Leemo.Repository.Repository
//{
//    /// <summary>
//    /// Represents profile repository for its CRUD and other custom functions.
//    /// </summary>
//    public class ProfileRepository : RepositoryBase<Profile, LeemoDbContext>, IProfileRepository
//    {
//        private LeemoDbContext _context;

//        public ProfileRepository(LeemoDbContext context) : base(context)
//        {
//            _context = context;
//        }

//        public IEnumerable<Profile> GetProfilesByUserId(Guid userId)
//        {
//            var profiles = (from profile in _context.Auth_RoleFeatureMapping
//                            join profileMapping in _context.Auth_RoleUserMapping
//                            on profile.FeatureId equals profileMapping.
//                            where profileMapping.UserId == userId
//                            select profile
//                          ).ToList();

//            return profiles;
//        }

//        public IEnumerable<ResultProfileUser> GetProfileUsersByProfileId(Guid profileId)
//        {

//            var profileUsers = (from user in _context.User
//                            join profileUserMapping in _context.ProfileUserMapping
//                            on user.Id equals profileUserMapping.UserId

//                            join profile in _context.Profile
//                            on profileUserMapping.ProfileId equals profile.Id

//                            where profile.Id == profileId
//                                select new ResultProfileUser() { 
//                                     UserId = user.Id,
//                                     UserName = string.Concat(user.UserProfile.FirstName, " ", user.UserProfile.LastName),
//                                     IsActive = user.IsActive,
//                                     Role = user.UserProfile.Role.Name
//                                }
//                          ).ToList();

//            return profileUsers;
//        }
//    }
//}
