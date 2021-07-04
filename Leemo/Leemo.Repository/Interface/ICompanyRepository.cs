using System;
using System.Collections.Generic;
using TPSS.Common.Interfaces;
using Leemo.Model.Domain;

/// <summary>
/// Represents repository project namespace
/// </summary>
namespace Leemo.Repository.Interface
{
    public interface ICompanyRepository : IRepository<Company>
    {
        new IEnumerable<Company> GetAll();
        new Company GetById(Guid id);
    }
}
