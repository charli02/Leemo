using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.Domain
{
    public class HostingKeyword
    {
        public Guid Id { get; set; }

        public string Keyword { get; set; }

        public bool IsValid { get; set; }
    }
}
