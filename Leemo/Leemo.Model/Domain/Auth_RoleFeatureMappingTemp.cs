using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.Domain
{
    public class Auth_RoleFeatureMappingTemp
    {
        public Guid FeatureId { get; set; }
        public Guid CodeId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid SessionId { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
