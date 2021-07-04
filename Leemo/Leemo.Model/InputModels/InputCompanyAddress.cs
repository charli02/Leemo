using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leemo.Model.InputModels
{
    public class InputCompanyAddress
    {
        public Guid Id { get; set; }

        [Required]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [MaxLength(200, ErrorMessage = "Street can be max 200 characters long.")]
        [Display(Name = "Street", Prompt = "Street")]
        public string Street { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "City can be max 20 characters long.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [Display(Name = "City", Prompt = "City")]
        public string City { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "State can be max 20 characters long.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [Display(Name = "State", Prompt = "State")]
        public string State { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = " ZipCode can be max 5 characters Small.")]
        [MaxLength(6, ErrorMessage = " Zip Code can be max 6 characters long.")]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Invalid Zip Code")]
        [Display(Name = "Zip Code", Prompt = "Zip Code")]
        public string ZipCode { get; set; }

        [Required]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [MaxLength(20, ErrorMessage = "Country Code can be max 20 characters long.")]
        [Display(Name = "Country", Prompt = "Country")]
        public string Country { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public Guid CompanyId { get; set; }
        public Guid AddressTypeId { get; set; }

    }
}
