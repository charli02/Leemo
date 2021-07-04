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
    public class ProductLeadRepository: RepositoryBase<ProductLead, LeemoDbContext>, IProductLeadRepository
    {
        public ProductLeadRepository(LeemoDbContext context) : base(context)
        {
            
         }
        public ProductLead GetByCompanyName(string CompanyName)
        {
            return Context.ProductLead.Where(x => x.CompanyName.Trim().ToLower() == CompanyName.Trim().ToLower()).FirstOrDefault();
        }

        public ProductLead GetById(Guid Id)
        {
            return Context.ProductLead.Where(x => x.Id == Id).FirstOrDefault();
        }
        public ProductLead GetProductLeadByEmail(string email)
        {
           
                return Context.ProductLead.Where(x => x.Email.Trim().ToLower() == email.Trim().ToLower()).FirstOrDefault();
            
        }

    }
}
