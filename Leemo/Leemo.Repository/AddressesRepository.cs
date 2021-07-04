using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPSS.Common.Implementations;
using Leemo.Data;
using Leemo.Model.Domain;
using Leemo.Repository.Interface;

namespace Leemo.Repository
{
    public class AddressesRepository : RepositoryBase<Addresses, LeemoDbContext>, IAddressesRepository
    {
        //private LeemoDbContext _context;

        public AddressesRepository(LeemoDbContext context) : base(context)
        {
            //_context = context;
        }

        public IEnumerable<Addresses> GetAll()
        {
            return Context.Addresses.Include(atype => atype.AddressType).ToList();
        }

        public Addresses GetAddressById(Guid Id)
        {
            return Context.Addresses.Where(x => x.Id == Id).Include(atype => atype.AddressType).FirstOrDefault();
        }
    }
}
