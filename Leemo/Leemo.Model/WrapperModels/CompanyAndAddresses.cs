using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.Domain;

namespace Leemo.Model.WrapperModels
{
    public class CompanyAndAddresses
    {
        public Company Company { get; set; }
        public Addresses CompanyAddress { get; set; }
    }
}
