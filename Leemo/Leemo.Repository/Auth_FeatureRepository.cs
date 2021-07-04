using Leemo.Data;
using TPSS.Common.Implementations;
using Leemo.Model;
using Leemo.Repository.Interface;
using Leemo.Model.Domain;
using TPSS.Common.Interfaces;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

/// <summary>
/// Represents Leemo repository project namespace
/// </summary>
namespace Leemo.Repository.Repository
{
    /// <summary>
    /// Represents address type repository for its CRUD and other custom functions.
    /// </summary>
    public class Auth_FeatureRepository : RepositoryBase<Feature, LeemoDbContext>, IAuth_FeatureRepository
    {
        //private LeemoDbContext _context;

        public Auth_FeatureRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }

    }
}
