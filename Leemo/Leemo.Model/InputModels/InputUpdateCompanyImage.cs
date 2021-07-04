using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.InputModels
{
    public class InputUpdateCompanyImage
    {
        public Guid CompanyId { get; set; }
        public string ImageName { get; set; }
    }
}
