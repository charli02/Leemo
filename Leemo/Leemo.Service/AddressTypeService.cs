using System;
using System.Collections.Generic;
using System.Linq;
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
    public class AddressTypeService : IAddressTypeService
    {
        private readonly IAddressTypeRepository _addressTypeRepository;
        public AddressTypeService(IAddressTypeRepository addressTypeRepository)
        {
            _addressTypeRepository = addressTypeRepository;
        }

        public IEnumerable<AddressType> GetAddressTypes()
        {
            return _addressTypeRepository.GetAll();
        }

        public AddressType GetAddressType(Guid Id)
        {
            return _addressTypeRepository.Get(Id);
        }

        public Guid GetAddressTypeIdWithName(string addressType)
        {
            Guid addressTypeId = Guid.Empty;
            var addressTypeList = _addressTypeRepository.GetAll();
            if (addressTypeList.Count() > 0)
            {
                addressTypeId = addressTypeList.Where(x => x.Name == addressType).FirstOrDefault().Id;
            }
            return addressTypeId;
        }
    }
}
