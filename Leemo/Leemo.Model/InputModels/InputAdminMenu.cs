using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Leemo.Model.InputModels
{
    public class InputAdminMenu
    {
        public Guid? Id { get; set; }

       
        
        [Display(Name = "Company Name", Prompt = "Company name")]
        [MaxLength(50, ErrorMessage = " Company Name can be max 50 characters long.")]
        [Required(ErrorMessage = "Company Name Required")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        public string Name { get; set; }

        [Required]
        public int EmployeeCount { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Phone can be max 20 characters long.")]
        [MinLength(5, ErrorMessage = "Phone can be min 5 characters long.")]
        [Display(Name = "Phone", Prompt = "Phone")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Mobile Number")]
        public string Phone { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Mobile Number")]
        [Display(Name = "Mobile", Prompt = "Mobile")]
        [MaxLength(20, ErrorMessage = "Mobile can be max 20 characters long.")]
        public string Mobile { get; set; }


        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Invalid Fax Number")]
        [Display(Name = "Fax", Prompt = "fax")]
        [MaxLength(20, ErrorMessage = " Fax can be max 20 characters long.")]
        public string Fax { get; set; }

        [Required]
        [Display(Name = "Website", Prompt = "Website")]
        [MaxLength(200, ErrorMessage = " Website can be max 200 characters long.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        public string Website { get; set; }
        [Display(Name = "Description", Prompt = "Description")]
        [MaxLength(200, ErrorMessage = " Description can be max 200 characters long.")]
        public string Description { get; set; }

        [Required]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        public string Currency { get; set; }

        public string TimeZone { get; set; }

        public string Language { get; set; }

        public InputCompanyAddress CompanyAddress { get; set; }

    }
}
