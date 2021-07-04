using System;
using System.Collections.Generic;
using System.Text;

namespace Leemo.Model.WrapperModels
{
    public class Auth_FeatureListWithGeneralCode
    {
        public string FeatureName { get; set; }
        public Guid FeatureId { get; set; }
        public bool IsActive { get; set; }
        public string ActiveFeatures { get; set; }
        public List<GeneralCode> GeneralCodes { get; set; }
        public Auth_FeatureListWithGeneralCode()
        {
            GeneralCodes = new List<GeneralCode>();
        }
    }

    public class GeneralCode
    {
        public Guid CodeId { get; set; }
        public string CodeName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
