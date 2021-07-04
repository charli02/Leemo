using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.Domain
{
    public class CompanyLocationUserMapping
    {
        public Guid CompanyLocationId { get; set; }
        public Guid UserId { get; set; }
        public Boolean isBaseLocation { get; set; }
    }
}
