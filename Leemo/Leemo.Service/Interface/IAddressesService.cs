using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;

namespace Leemo.Service.Interface
{
    public interface IAddressesService
    {
        public IEnumerable<Addresses> GetAddresses();
        Addresses GetAddressById(Guid Id);
        //public IEnumerable<Addresses> GetUserAddress(Guid UserId);
        public Addresses GetAddressByReference(Guid referenceId,Guid AddressTypeId);
        // void CreateAddress(Addresses userAddress);
        Addresses CreateAddress(InputAddress userAddress);
        void EditAddress(InputAddress userAddress);

       

    }
}
