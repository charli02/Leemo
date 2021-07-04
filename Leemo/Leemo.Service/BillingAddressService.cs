using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPSS.Common;

namespace Leemo.Service
{
   public  class BillingAddressService : IBillingAddressService
    {
        private readonly IBillingAddressRepository _billingAddressRepository;
        private readonly IAddressTypeService _addressTypeService;
        private readonly IAddressesService _addressService;

        public BillingAddressService(IBillingAddressRepository billingAddressRepository, IAddressTypeService addressTypeService, IAddressesService addressService)
        {
            _billingAddressRepository = billingAddressRepository;
            _addressTypeService = addressTypeService;
            _addressService = addressService;
        }

        public IEnumerable<ResultBillingAddress> GetBillingAddressByCompanyLocation(Guid CompanyLocationId) 
        {
            var  currentAddressAddressTypeId = _addressTypeService.GetAddressTypeIdWithName(Constants.AddressTypeNames.BillingAddress);
            var billingAddress = _billingAddressRepository.GetBillingAddressByCompanyLocation(CompanyLocationId, currentAddressAddressTypeId);
            return billingAddress;
        }

        public ResultBillingAddress GetBillingAddressById(Guid Id)
        {
            var address = _addressService.GetAddressById(Id);
            ResultBillingAddress billingAddress = new ResultBillingAddress();
            if (address != null)
            {
                billingAddress.Id = address.Id;
                billingAddress.Street = address.Street;
                billingAddress.AddressLine1 = address.AddressLine1;
                billingAddress.City = address.City;
                billingAddress.State = address.State;
                billingAddress.ZipCode = address.ZipCode;
                billingAddress.Country = address.Country;
                billingAddress.CreatedOn = address.CreatedOn;
                billingAddress.ModifiedOn = address.ModifiedOn;
                billingAddress.ReferenceId = address.ReferenceId;
                billingAddress.AddressTypeId = address.AddressTypeId;
            }
            return billingAddress;
        }

        public Addresses InsertBillingAddress(InputAddress inputAddress)
        {
            Addresses addressToCreate = new Addresses();
            if (inputAddress != null)
            {
                addressToCreate.AddressLine1 = inputAddress.AddressLine1;
                addressToCreate.Street = inputAddress.Street;
                addressToCreate.City = inputAddress.City;
                addressToCreate.State = inputAddress.State;
                addressToCreate.ZipCode = inputAddress.ZipCode;
                addressToCreate.Country = inputAddress.Country;
                addressToCreate.CreatedOn = DateTime.UtcNow;
                addressToCreate.ReferenceId = inputAddress.ReferenceId;
                addressToCreate.AddressTypeId = _addressTypeService.GetAddressTypeIdWithName(Constants.AddressTypeNames.BillingAddress);
            _billingAddressRepository.Add(addressToCreate);
            _billingAddressRepository.Save();
            }
            return addressToCreate;
        }

        public ResultBillingAddress UpdateBillingAddress(InputAddress inputAddress)
        {
            Addresses addressToUpdate = _addressService.GetAddressById(inputAddress.Id);

            if (addressToUpdate != null)
            {
                addressToUpdate.AddressLine1 = inputAddress.AddressLine1;
                addressToUpdate.Street = inputAddress.Street;
                addressToUpdate.City = inputAddress.City;
                addressToUpdate.State = inputAddress.State;
                addressToUpdate.ZipCode = inputAddress.ZipCode;
                addressToUpdate.Country = inputAddress.Country;
                addressToUpdate.CreatedOn = addressToUpdate.CreatedOn;
                addressToUpdate.ModifiedOn = DateTime.UtcNow;
                addressToUpdate.ReferenceId = addressToUpdate.ReferenceId;
                addressToUpdate.AddressTypeId = _addressTypeService.GetAddressTypeIdWithName(Constants.AddressTypeNames.BillingAddress);
                _billingAddressRepository.Edit(addressToUpdate);
                _billingAddressRepository.Save();
            }
            return GetBillingAddressById(inputAddress.Id);
        }

        public void DeleteBillingAddress(Guid Id)
        {
            if (Id != null)
            {
                Addresses addressToUpdate = _addressService.GetAddressById(Id);
                _billingAddressRepository.Delete(addressToUpdate);
            }
        }
    }
}
