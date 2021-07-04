using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;

namespace Leemo.Model.WrapperModels
{
    public class ResultCompanyAndAddresses
    {
        public ResultCompany resultCompany { get; set; }
        public Addresses CompanyAddress { get; set; }
    }
}
