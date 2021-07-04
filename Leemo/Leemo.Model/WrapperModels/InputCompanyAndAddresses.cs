using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.InputModels;

namespace Leemo.Model.WrapperModels
{
    public class InputCompanyAndAddresses
    {
        public InputCompany inputCompany { get; set; }
        public InputAddress CompanyAddress { get; set; }
        public Guid CompanyLocationId { get; set; }
    }
}
