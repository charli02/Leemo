using Leemo.Model.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Service.Interface
{
    public interface IProductService
    {
        IEnumerable<Product> GetProductByCompanyId(Guid CompanyId, Guid CompanyLocationId);
    }
}
