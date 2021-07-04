using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.InputModels
{
    public class InputUpdateProfileImage
    {
        public Guid UserId { get; set; }
        public string ImageName { get; set; }
        public Guid CompanyId { get; set; }
        public Guid CompanyLocationId { get; set; }

    }
}
