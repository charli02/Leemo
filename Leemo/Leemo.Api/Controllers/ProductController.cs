using Leemo.Model.Domain;
using Leemo.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TPSS.Common;
using TPSS.Common.Wrappers;

namespace Leemo.Api.Controllers
{
    [Authorize]
    [Route(Constants.Attrributes.ApiDefaultRoute)]
    [ApiController]
    public class ProductController : BaseController
    {
        private List<object> _paraList = new List<object>();
        private string ErrMsg = "";
        private bool response = false;
        private readonly ICommonService _commonService;
        private readonly IProductService _productService;

        public ProductController(IProductService productService, ICommonService commonService)
        {
            _productService = productService;
            _commonService = commonService;
        }

        [HttpGet]
        [Route(Constants.Routes.GetProductsOfCompany)]
        public PagedResponse<Product> GetProductsOfCompany(Guid CompanyId, Guid CompanyLocationId)
        {
            IEnumerable<Product> products = _productService.GetProductByCompanyId(CompanyId,CompanyLocationId).OrderBy(x=>x.SortOrder);
            try
            {
                if (products != null && products.Count() > 0)
                {
                    response = Constants.ApiRequestResponse.ResponseSuccess;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(CompanyId), ErrMsg);
                    return PagedResponse<Product>.PagedList(
                        products,
                        Constants.Messages.Success,
                        true,
                        Enums.HttpStatusCode.OK);
                }
                else
                {
                    ErrMsg = Constants.Messages.NotDataExistsInTable;
                    _commonService.ApiRequestLogInDb(Request.Path.Value, response, getUserEmail(), CommonFunction.returnJsontoString(CompanyId), ErrMsg);
                    return PagedResponse<Product>.PagedList(
                        products,
                        Constants.Messages.NotDataExistsInTable,
                        true,
                        Enums.HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return PagedResponse<Product>.PagedList(
                    products,
                    Constants.Messages.Failed,
                    false,
                    Enums.HttpStatusCode.InternalServerError);
            }
        }
    }
}
