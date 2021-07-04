using System;
using System.Collections.Generic;
using Leemo.Model;
using Leemo.Model.Domain;

/// <summary>
/// Represents service project namespace
/// </summary>
namespace Leemo.Service.Interface
{
    public interface IUserAddressService
    {
        public IEnumerable<UserAddress> GetUserAddresses();
        public UserAddress GetUserAddress(Guid Id);
        void CreateUserAddress(UserAddress userAddress);
        void EditUserAddress(UserAddress userAddress);
    }
}
