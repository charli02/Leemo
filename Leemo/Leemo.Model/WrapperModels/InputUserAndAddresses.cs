using System;
using System.Collections.Generic;
using System.Text;
using Leemo.Model.Domain;
using Leemo.Model.InputModels;

namespace Leemo.Model.WrapperModels
{
    public class InputUserAndAddresses
    {
        public InputUpdateUser InputUser { get; set; }
        public InputAddress InputAddress { get; set; }
        public Guid CompanyLocationId { get; set; }
        public bool isUserCurrentBaseLocation { get; set; }
    }
}
