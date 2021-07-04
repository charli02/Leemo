using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.Domain;
using Leemo.Model.ResultModels;

namespace Leemo.Model.WrapperModels
{
    public class ResultLocationAndAddress
    {
        public ResultLocation ResultLocation { get; set; }
        public Addresses Address { get; set; }
        public ResultCompany resultCompany { get; set; }
    }

}
