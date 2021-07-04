using Leemo.Data;
using Leemo.Model.Domain;
using Leemo.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using TPSS.Common.Implementations;
using System.Linq;

namespace Leemo.Repository
{
    public class ProductRepository : RepositoryBase<Product, LeemoDbContext>, IProductRepository
    {
        public ProductRepository(LeemoDbContext context) : base(context)
        {
        }

        public IEnumerable<Product> GetProductByCompanyId(Guid CompanyId)
        {
            var resultProduct = from product in Context.Product
                                join companyProduct in Context.CompanyProductMapping on product.Id equals companyProduct.ProductId
                                where companyProduct.CompanyId == CompanyId
                                select product;

            return resultProduct;
        }

    }
}
