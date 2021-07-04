using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Leemo.Model.Domain
{
    public class Auth_FeatureListWithGeneralCodeResult
    {
        public string AdminMenuName { get; set; }

        public string FeatureName { get; set; }

        public string CodeName { get; set; }

        public string CodeValue { get; set; }

        public Guid AuthFeatureId { get; set; }

        public Guid? GeneralCodeId { get; set; }

        [NotMapped]
        public bool Enabled { get; set; }

    }
}
