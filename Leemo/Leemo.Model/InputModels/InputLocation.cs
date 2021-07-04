using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Leemo.Model.Domain;

namespace Leemo.Model.InputModels
{
    public class InputLocation
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Location Name field is required.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [MaxLength(75, ErrorMessage = "Location Name can be max 75 characters long.")]
        [Display(Name = "LocationName", Prompt = "LocationName")]
        public string LocationName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        //public Guid AddressId { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsHeadOffice { get; set; }
        public bool IsActive { get; set; }

        
    }
}
