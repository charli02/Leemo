using System;
using System.Collections.Generic;
using Leemo.Model;
using Leemo.Model.Domain;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface ICompanyAddressService
    {
        //public IEnumerable<CompanyAddress> GetCompanyAddresses();

        //public CompanyAddress GetCompanyAddress(Guid Id);

        //void CreateCompanyAddress(CompanyAddress companyAddress);

        //void EditCompanyAddress(CompanyAddress companyAddress);

        public IEnumerable<Addresses> GetCompanyAddresses();

        public Addresses GetCompanyAddress(Guid Id);

        void CreateCompanyAddress(Addresses companyAddress);

        void EditCompanyAddress(Addresses companyAddress);

    }
}

