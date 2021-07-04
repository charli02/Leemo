using System;
using System.Collections.Generic;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Model.WrapperModels;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface ICompanyService
    {
        public IEnumerable<Company> GetCompanies();

        public ResultCompany GetCompany(Guid Id);

        void CreateCompany(InputCompanyAndAddresses company);

        void EditCompany(InputCompanyAndAddresses company);

        //List<Company> SearchCompanyByName(string EmployeeName);

        public Company UpdateCompanyImage(InputUpdateCompanyImage updateCompanyImage);
    }
}
