using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.Domain
{
    public class Auth_FeatureListWithGeneralCodeByUserIdResult
    {
        public string AdminMenuName { get; set; }

        public string FeatureName { get; set; }

        public string CodeName { get; set; }

        public string CodeValue { get; set; }

        public string CodeGroupName { get; set; }

        public Guid UserId { get; set; }

        public string RoleName { get; set; }

        public Guid AuthFeatureId { get; set; }

        public Guid GeneralCodeId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
