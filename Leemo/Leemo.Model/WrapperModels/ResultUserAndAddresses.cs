using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;

namespace Leemo.Model.WrapperModels
{
    public class ResultUserAndAddresses
    {
        public ResultUser ResultUser { get; set; }
        public Addresses userAddress { get; set; }
        public ResultCompany resultCompany { get; set; }

    }
}
