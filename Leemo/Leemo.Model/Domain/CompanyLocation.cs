using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Leemo.Model.Domain
{
    public class CompanyLocation
    {
        public Guid Id { get; set; }

        [Required]
        public string LocationName { get; set; }

        public Boolean IsHeadOffice { get; set; }
        public Boolean IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        //public Guid AddressId { get; set; }
        public Guid CompanyId { get; set; }

    }
}
