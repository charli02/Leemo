using Leemo.Model.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using TPSS.Common.Interfaces;

namespace Leemo.Repository.Interface
{
    public interface IProductRepository: IRepository<Product>
    {
        IEnumerable<Product> GetProductByCompanyId(Guid CompanyId);
    }
}
