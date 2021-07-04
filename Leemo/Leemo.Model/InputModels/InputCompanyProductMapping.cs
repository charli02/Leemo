using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leemo.Model.InputModels
{
   public  class InputCompanyProductMapping
    {
        public Guid CompanyId { get; set; }
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Please enter Domain Name.")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Only Alphabets are allowed")]
        [Display(Name = "DomainName", Prompt = "Domain Name")]
        [MaxLength(150, ErrorMessage = " Domain Name can be max 150 characters long.")]
        public string DomainName { get; set; }
    }
}
