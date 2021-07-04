using System;
using System.Collections.Generic;
using Leemo.Model;
using Leemo.Model.Domain;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface IAddressTypeService
    {
        public IEnumerable<AddressType> GetAddressTypes();

        public AddressType GetAddressType(Guid Id);

        public Guid GetAddressTypeIdWithName(string addressType);

    }
}

