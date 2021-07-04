using System;
using System.Collections.Generic;
using System.Linq;
using TPSS.Common;
using Leemo.Model;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Model.WrapperModels;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;

/// <summary>
/// Represents Leemo service project namespace
/// </summary>
namespace Leemo.Service
{
    /// <summary>
    /// Represnets comapny serivce class which interact with repository.
    /// </summary>
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyAddressRepository _companyAddressRepository;
        private readonly IAddressTypeRepository _addressTypeRepository;
        private readonly IAddressesRepository _addressRepository;
        private readonly IAddressesService _addressService;
        private readonly IAddressTypeService _addressTypeService;

        public CompanyService(ICompanyRepository companyRepository, ICompanyAddressRepository companyAddressRepository, IAddressTypeRepository addressTypeRepository, IAddressesRepository addressRepository, IAddressesService addressService, IAddressTypeService addressTypeService)
        {
            _companyRepository = companyRepository;
            _companyAddressRepository = companyAddressRepository;
            _addressTypeRepository = addressTypeRepository;
            _addressRepository = addressRepository;
            _addressService = addressService;
            _addressTypeService = addressTypeService;
        }

        public IEnumerable<Company> GetCompanies()
        {
            return _companyRepository.GetAll();
        }

        public ResultCompany GetCompany(Guid Id)
        {
            Company currentCompany = _companyRepository.GetById(Id);
            ResultCompany company = new ResultCompany();
            if (currentCompany != null)
            {
                company.Id = currentCompany.Id;
                company.Name = currentCompany.Name;
                company.EmployeeCount = currentCompany.EmployeeCount;
                company.Phone = currentCompany.Phone;
                company.Mobile = currentCompany.Mobile;
                company.Fax = currentCompany.Fax;
                company.Website = currentCompany.Website;
                company.Description = currentCompany.Description;
                company.Currency = currentCompany.Currency;
                company.TimeZone = currentCompany.TimeZone;
                company.CountryCode = currentCompany.CountryCode;
                company.Language = currentCompany.Language;
                company.ImageName = currentCompany.ImageName;
                company.DateFormat = currentCompany.DateFormat;
                company.TimeFormat = currentCompany.TimeFormat;


                return company;
            }
            return null;
        }

        public void CreateCompany(InputCompanyAndAddresses company)
        {
            Company companyToCreate = new Company();
            companyToCreate.Name = company.inputCompany.Name;
            companyToCreate.EmployeeCount = company.inputCompany.EmployeeCount;
            companyToCreate.Phone = company.inputCompany.Phone;
            companyToCreate.Mobile = company.inputCompany.Mobile;
            companyToCreate.Fax = company.inputCompany.Fax;
            companyToCreate.Website = company.inputCompany.Website;
            companyToCreate.Description = company.inputCompany.Description;
            companyToCreate.Currency = company.inputCompany.Currency;
            companyToCreate.TimeZone = company.inputCompany.TimeZone;
            companyToCreate.CreatedOn = DateTime.UtcNow;
            companyToCreate.Language = company.inputCompany.Language;
            companyToCreate.TimeFormat = company.inputCompany.TimeFormat;
            companyToCreate.DateFormat = company.inputCompany.DateFormat;
            _companyRepository.Add(companyToCreate);
            _companyRepository.Save();

            Addresses CompanyAddress = new Addresses();
                CompanyAddress.ReferenceId = companyToCreate.Id;
                CompanyAddress.Street = company.CompanyAddress.Street;
                CompanyAddress.AddressLine1 = company.CompanyAddress.AddressLine1;
                CompanyAddress.City = company.CompanyAddress.City;
                CompanyAddress.State = company.CompanyAddress.State;
                CompanyAddress.ZipCode = company.CompanyAddress.ZipCode;
                CompanyAddress.Country = company.CompanyAddress.Country;
                CompanyAddress.AddressTypeId = _addressTypeService.GetAddressTypeIdWithName(Constants.AddressTypeNames.CompanyAddress);
                CompanyAddress.CreatedOn = DateTime.UtcNow;



            _addressRepository.Add(CompanyAddress);
            _addressRepository.Save();
        }

        public void EditCompany(InputCompanyAndAddresses company)
        {
            Company companyToUpdate = _companyRepository.GetById((Guid)company.inputCompany.Id);
            Guid AddressTypeId = _addressTypeService.GetAddressTypeIdWithName(Constants.AddressTypeNames.CompanyAddress);
            Addresses addressToUpdate = _addressService.GetAddressByReference(company.CompanyLocationId,AddressTypeId);
            if (companyToUpdate != null)
            {
                companyToUpdate.Name = company.inputCompany.Name;
                companyToUpdate.EmployeeCount = company.inputCompany.EmployeeCount;
                companyToUpdate.Phone = company.inputCompany.Phone;
                companyToUpdate.Mobile = company.inputCompany.Mobile;
                companyToUpdate.CountryCode = company.inputCompany.CountryCode;
                companyToUpdate.Fax = company.inputCompany.Fax;
                companyToUpdate.Website = company.inputCompany.Website;
                companyToUpdate.Description = company.inputCompany.Description;
                companyToUpdate.Currency = company.inputCompany.Currency;
                companyToUpdate.TimeZone = company.inputCompany.TimeZone;
                companyToUpdate.ModifiedOn = DateTime.UtcNow;
                companyToUpdate.Language = company.inputCompany.Language;
                companyToUpdate.DateFormat = company.inputCompany.DateFormat;
                companyToUpdate.TimeFormat = company.inputCompany.TimeFormat;
                if (addressToUpdate != null)
                {
                    addressToUpdate.Street = company.CompanyAddress.Street;
                    addressToUpdate.City = company.CompanyAddress.City;
                    addressToUpdate.State = company.CompanyAddress.State;
                    addressToUpdate.Country = company.CompanyAddress.Country;
                    addressToUpdate.ZipCode = company.CompanyAddress.ZipCode;
                    addressToUpdate.ModifiedOn = DateTime.UtcNow;
                    addressToUpdate.Street = company.CompanyAddress.Street;
                    addressToUpdate.AddressTypeId = company.CompanyAddress.AddressTypeId;
                    addressToUpdate.ModifiedOn = DateTime.UtcNow;
                    addressToUpdate.AddressLine1 = company.CompanyAddress.AddressLine1;
                }
            }

            _companyRepository.Edit(companyToUpdate);
            _companyRepository.Save();
            _addressRepository.Edit(addressToUpdate);
            _addressRepository.Save();
        }

        //public List<Company> SearchCompanyByName(string CompanyName)
        //{
        //    return _companyRepository.SearchCompanyByName(CompanyName);
        //}

        public Company UpdateCompanyImage(InputUpdateCompanyImage updateCompanyImage)
        {
            Company currentcompany = _companyRepository.GetById(updateCompanyImage.CompanyId);
            if (currentcompany != null)
            {
                currentcompany.ImageName = updateCompanyImage.ImageName;
                currentcompany.ModifiedOn = DateTime.UtcNow;
                _companyRepository.Edit(currentcompany);
                _companyRepository.Save();
                return currentcompany;
            }
            return currentcompany;
        }
    }
}
