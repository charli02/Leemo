using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;

namespace Leemo.Model.WrapperModels
{
    public class InputLocationandAddress
    {
        public InputAddress Addresses { get; set; }
        public InputLocation inputLocation { get; set; }
        //public IEnumerable<CompanyLocation> companyLocationList { get; set; }

    }
}
