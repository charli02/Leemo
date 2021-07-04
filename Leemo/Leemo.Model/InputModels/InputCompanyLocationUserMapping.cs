using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Leemo.Model.InputModels
{
    public class InputCompanyLocationUserMapping
    {
        public Guid CompanyLocationId { get; set; }
        public Guid UserId { get; set; }
        public Boolean isBaseLocation { get; set; }
        [NotMapped]
        public bool isFromNewLocation { get; set; }
    }
}
