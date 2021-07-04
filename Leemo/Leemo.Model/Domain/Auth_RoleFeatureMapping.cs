using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.Domain
{
    public class Auth_RoleFeatureMapping
    {
        public Guid FeatureId { get; set; }
        public Guid CodeId { get; set; }
        public Guid RoleId { get; set; }
    }
}
