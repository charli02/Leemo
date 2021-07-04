//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Leemo.Data;
//using TPSS.Common.Implementations;
//using Leemo.Model;
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
//    public class ProfileUserMappingRepository : RepositoryBase<ProfileUserMapping, LeemoDbContext>, IProfileUserMappingRepository
//    {
//        private LeemoDbContext _context;

//        public ProfileUserMappingRepository(LeemoDbContext context) : base(context)
//        {
//            _context = context;
//        }

//        public bool DeleteProflieUsersMappingByUserId(Guid userId)
//        {
//            try
//            {
//                _context.ProfileUserMapping.RemoveRange(_context.ProfileUserMapping.Where(x => x.UserId == userId));
//                _context.SaveChanges();
//                return true;
//            }
//            catch (Exception ex)
//            {
//                return false;
//            }
//        }

//        public void InsetProfileUserMapping(IList<Guid> userProfiles, Guid userId)
//        {
//            _context.ProfileUserMapping.AddRange(
//               userProfiles.Select(m => new ProfileUserMapping
//               {
//                   ProfileId = Guid.Parse(Convert.ToString(m)),
//                   UserId = userId,
//                   CreatedOn = DateTime.UtcNow
//               }).ToList()
//            );
//            _context.SaveChanges();
//        }
//    }
//}
