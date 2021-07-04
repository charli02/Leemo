using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;

namespace Leemo.Service
{
    public class AddressesService : IAddressesService
    {
        private readonly IAddressesRepository _addressRepository;

        public AddressesService(IAddressesRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public IEnumerable<Addresses> GetAddresses()
        {
            return _addressRepository.GetAll();
        }

        public Addresses GetAddressById(Guid Id)
        {
            return _addressRepository.GetAddressById(Id);
        }

        //public IEnumerable<Addresses> GetUserAddress(Guid UserId)
        //{
        //    return _addressRepository.GetAll().Where(x=>x.ReferenceId == UserId).ToList();
        //}

        public Addresses GetAddressByReference(Guid referenceId, Guid AddressTypeId)
        {
            return _addressRepository.GetAll().Where(x=>x.ReferenceId == referenceId && x.AddressTypeId == AddressTypeId).FirstOrDefault();
        }

        public Addresses CreateAddress(InputAddress address)
        {

            Addresses addresses = new Addresses();

            addresses.AddressTypeId = address.AddressTypeId;
            addresses.City = address.City;
            addresses.State = address.State;
            addresses.Street = address.Street;
            addresses.CreatedOn = DateTime.Now;
            addresses.Country = address.Country;
            addresses.ReferenceId = address.ReferenceId;
            addresses.ZipCode = address.ZipCode;
            addresses.AddressLine1 = address.AddressLine1;
            
            
            _addressRepository.Add(addresses);
            _addressRepository.Save();
            return addresses;
        }


        public void EditAddress(InputAddress address)
        {
            var currentAddress = _addressRepository.GetAddressById(address.Id);
            if (address != null)
            {
                currentAddress.Street = address.Street;
                currentAddress.City = address.City;
                currentAddress.State = address.State;
                currentAddress.ZipCode = address.ZipCode;
                currentAddress.Country = address.Country;
                currentAddress.CreatedOn = currentAddress.CreatedOn;
                currentAddress.ModifiedOn = DateTime.Now;
                currentAddress.ReferenceId = currentAddress.ReferenceId;
                currentAddress.AddressTypeId = currentAddress.AddressTypeId;
                currentAddress.AddressLine1 = address.AddressLine1;
            }
            _addressRepository.Edit(currentAddress);
            _addressRepository.Save();
        }
    }
}
