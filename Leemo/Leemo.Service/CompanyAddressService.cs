using System;
using System.Collections.Generic;
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
    /// This class represents service for ciompany address.
    /// </summary>
    public class CompanyAddressService: ICompanyAddressService
    {
        private readonly ICompanyAddressRepository _companyAddressRepository;
        public CompanyAddressService(ICompanyAddressRepository companyAddressRepository)
        {
            _companyAddressRepository = companyAddressRepository;
        }

        //public IEnumerable<CompanyAddress> GetCompanyAddresses()
        public IEnumerable<Addresses> GetCompanyAddresses()
        {
            return _companyAddressRepository.GetAll();
        }

        //public CompanyAddress GetCompanyAddress(Guid Id)
        public Addresses GetCompanyAddress(Guid Id)
        {
            return _companyAddressRepository.Get(Id);
        }

        //public void CreateCompanyAddress(CompanyAddress companyAddress)
        public void CreateCompanyAddress(Addresses companyAddress)
        {
            _companyAddressRepository.Add(companyAddress);
            _companyAddressRepository.Save();
        }

        //public void EditCompanyAddress(CompanyAddress companyAddress)
        public void EditCompanyAddress(Addresses companyAddress)
        {
            _companyAddressRepository.Edit(companyAddress);
            _companyAddressRepository.Save();
        }

    }
}
