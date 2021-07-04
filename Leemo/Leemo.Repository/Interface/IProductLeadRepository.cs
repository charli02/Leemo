using System;
using System.Collections.Generic;
using System.Text;
using TPSS.Common.Interfaces;
using Leemo.Model.Domain;

namespace Leemo.Repository.Interface
{
    public interface IProductLeadRepository : IRepository<ProductLead>
    {

        ProductLead GetById(Guid Id);

        ProductLead GetByCompanyName(string CompanyName);
        ProductLead GetProductLeadByEmail(string email);
        
    }
}
