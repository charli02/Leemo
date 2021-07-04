using Leemo.Model.Domain;
using Leemo.Model.InputModels;
using Leemo.Model.ResultModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Service.Interface
{
    public interface IBillingAddressService
    {

        public IEnumerable<ResultBillingAddress> GetBillingAddressByCompanyLocation(Guid CompanyLocationId);
        ResultBillingAddress GetBillingAddressById(Guid Id);
        Addresses InsertBillingAddress(InputAddress inputAddress);
        ResultBillingAddress UpdateBillingAddress(InputAddress inputAddress);
        void DeleteBillingAddress(Guid Id);
    }
}
