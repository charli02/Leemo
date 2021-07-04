using Leemo.Data;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;
using Leemo.Repository.Interface;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using TPSS.Common.Implementations;
using Microsoft.EntityFrameworkCore;
using Leemo.Model.WrapperModels;

namespace Leemo.Repository
{
    public class BillingAddressRepository : RepositoryBase<Addresses, LeemoDbContext>, IBillingAddressRepository
    {
        public BillingAddressRepository(LeemoDbContext context) : base(context)
        {
        }

        public new IEnumerable<ResultBillingAddress> GetBillingAddressByCompanyLocation(Guid CompanyLocationId, Guid currentAddressAddressTypeId) 
        {
            var data = from A in Context.Addresses
                       join CL in Context.CompanyLocation on A.ReferenceId equals CL.Id
                       join ADT in Context.AddressType on A.AddressTypeId equals ADT.Id
                       join C in Context.Company on CL.CompanyId equals C.Id
                       where
                       A.ReferenceId==CompanyLocationId && ADT.Id== currentAddressAddressTypeId
                       && CL.IsHeadOffice==true
                       select new ResultBillingAddress
                       {
                           Id = A.Id,
                           //CompanyName = C.Name,
                           LocationName = CL.LocationName,
                           AddressTypeName = ADT.Name,
                           IsHeadOffice = CL.IsHeadOffice,
                           AddressLine1 = A.AddressLine1,
                           City = A.City,
                           State = A.State,
                           ZipCode = A.ZipCode,
                           Country = A.Country,
                           Street = A.Street,

                       };
            var test = data.ToList();
            return test;
        }

    }
}
