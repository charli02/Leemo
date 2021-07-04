using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.Domain
{
    public class CompanyProductMapping
    {
        public Guid CompanyId { get; set; }
        public Guid ProductId { get; set; }
        public string DomainName { get; set; }
    }
}
