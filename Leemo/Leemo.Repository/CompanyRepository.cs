using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Leemo.Data;
using TPSS.Common.Implementations;
using Leemo.Model;
using Leemo.Repository.Interface;
using Leemo.Model.Domain;

/// <summary>
/// Represents Leemo repository project namespace
/// </summary>
namespace Leemo.Repository.Repository
{
    /// <summary>
    /// Represents company repository for its CRUD and other custom functions.
    /// </summary>
    public class CompanyRepository : RepositoryBase<Company,LeemoDbContext>, ICompanyRepository
    {
        //private LeemoDbContext _context;
        public CompanyRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }

        public new IEnumerable<Company> GetAll()
        {
            return Context.Company
                //.Include(d => d.CompanyAddress)
                //.ThenInclude(x => x.AddressType)
                .ToList();
        }
 
        public Company GetById(Guid id)
        {
            return Context.Company.Where(x => x.Id == id)
                //.Include(d => d.CompanyAddress)
                //.ThenInclude(x => x.AddressType)
                .FirstOrDefault();

        }

        //public List<Company> SearchCompanyByName(string CompanyName)
        //{
        //    using (SqlConnection connection = new SqlConnection(_appSettings.connectionStrings.Leemo_API_DbConnection))
        //    {
        //        DynamicParameters ds = new DynamicParameters();
        //        ds.Add("@CompanyName", CompanyName);

        //        var results = connection.Query<Employee>("sp_SearchCompanyByName", param: ds, commandType: CommandType.StoredProcedure, commandTimeout: 400).ToList();

        //        return results;
        //    }
        //}
    }
}
