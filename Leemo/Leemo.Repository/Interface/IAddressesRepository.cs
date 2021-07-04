using System;
using System.Collections.Generic;
using System.Text;
using TPSS.Common.Interfaces;
using Leemo.Model.Domain;

namespace Leemo.Repository.Interface
{
    public interface IAddressesRepository : IRepository<Addresses>
    {
        IEnumerable<Addresses> GetAll();
        Addresses GetAddressById(Guid Id);
    }
}
