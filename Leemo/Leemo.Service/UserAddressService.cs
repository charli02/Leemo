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
    /// Represnets useraddress serivce class which interact with repository.
    /// </summary>
    public class UserAddressService: IUserAddressService
    {
        private readonly IUserAddressRepository _userAddressRepository;
        public UserAddressService(IUserAddressRepository userAddressRepository)
        {
            _userAddressRepository = userAddressRepository;
        }
        public IEnumerable<UserAddress> GetUserAddresses()
        {
            return _userAddressRepository.GetAll();
        }
        public UserAddress GetUserAddress(Guid Id)
        {
            return _userAddressRepository.Get(Id);
        }
        public void CreateUserAddress(UserAddress userAddress)
        {
            _userAddressRepository.Add(userAddress);
            _userAddressRepository.Save();
        }
        public void EditUserAddress(UserAddress userAddress)
        {
            _userAddressRepository.Edit(userAddress);
            _userAddressRepository.Save();
        }
    }
}
