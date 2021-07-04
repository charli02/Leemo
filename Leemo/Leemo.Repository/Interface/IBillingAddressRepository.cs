using Leemo.Model.Domain;
using Leemo.Model.ResultModels;
using System;
using System.Collections.Generic;
using System.Text;
using TPSS.Common.Interfaces;

namespace Leemo.Repository.Interface
{
    public interface IBillingAddressRepository : IRepository<Addresses>
    {

        public IEnumerable<ResultBillingAddress> GetBillingAddressByCompanyLocation(Guid CompanyLocationId, Guid currentAddressAddressTypeId);

    }
}
