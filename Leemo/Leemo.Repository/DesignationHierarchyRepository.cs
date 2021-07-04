using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Leemo.Data;
using TPSS.Common.Implementations;
using Leemo.Model;
using Leemo.Repository.Interface;
using Leemo.Model.Domain;
using System;

/// <summary>
/// Represents Leemo repository project namespace
/// </summary>
namespace Leemo.Repository.Repository
{
    /// <summary>
    /// Represents designation repository for its CRUD and other custom functions.
    /// </summary>
    public class DesignationHierarchyRepository : RepositoryBase<DesignationHierarchy, LeemoDbContext>, IDesignationHierarchyRepository
    {
        //private LeemoDbContext _context;

        public DesignationHierarchyRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }

        public IEnumerable<DesignationHierarchy> GetDesignationHierarchyList()
        {
            return Context.DesignationHierarchy
                .Include(x => x.Designation)
                .Include(x => x.ParentDesignation)
                .ToList();
        }

        public void CreateDesignationHierarchies(DesignationHierarchy designationHierarchy)
        {
            try
            {
                Context.DesignationHierarchy.Add(designationHierarchy);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            
            
        }
    }
}
