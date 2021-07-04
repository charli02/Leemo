using Leemo.Model.Domain;
using Leemo.Repository.Interface;
using Leemo.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leemo.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICompanyLocationService _locationService;

        public ProductService(IProductRepository productRepository, ICompanyLocationService locationService)
        {
            _productRepository = productRepository;
            _locationService = locationService;
        }

        public IEnumerable<Product> GetProductByCompanyId(Guid CompanyId, Guid CompanyLocationId)
        {
            var products = _productRepository.GetProductByCompanyId(CompanyId);
            Guid HeadOffice = _locationService.GetLocationsByCompanyId(CompanyId).Where(x => x.IsHeadOffice == true).FirstOrDefault().Id;
            if (CompanyLocationId != HeadOffice)
            {
                products = products.Where(x => x.IsLocationBased);
            }
            return products;
        }
    }
}
